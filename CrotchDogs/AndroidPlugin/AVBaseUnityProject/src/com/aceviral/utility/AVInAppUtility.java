package com.aceviral.utility;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.PriorityQueue;
import java.util.Set;

import com.aceviral.utility.inapp.BillingService;
import com.aceviral.utility.inapp.PurchaseDatabase;
import com.aceviral.utility.inapp.PurchaseObserver;
import com.aceviral.utility.inapp.ResponseHandler;
import com.aceviral.utility.inapp.BillingService.RequestPurchase;
import com.aceviral.utility.inapp.BillingService.RestoreTransactions;
import com.aceviral.utility.inapp.Consts.PurchaseState;
import com.aceviral.utility.inapp.Consts.ResponseCode;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.database.Cursor;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.widget.Toast;

public class AVInAppUtility {

	// Database handling vars
	private Cursor m_OwnedItemsCursor;
	private Set<String> m_OwnedItems = new HashSet<String>();
	private PurchaseDatabase m_PurchaseDatabase;
	private static final String TAG = "InApp";
	private static final String DB_INITIALIZED = "db_initialized";
	private static final String MANAGED_PREFS = "managedPrefs";

	private boolean hasRequestedInApp = false;

	// In-app network/threading vars
	private Handler m_Handler;
	private InAppPurchaseObserver m_InAppPurchaseObserver;
	private BillingService m_BillingService;

	// Purchase data vars
	private boolean m_PurchaseRequested = false;
	private int m_PurchaseMessage = -1;
	private ArrayList<String> m_PurchaseList = new ArrayList<String>();
	private PriorityQueue<String> m_PurchaseItemQueue = new PriorityQueue<String>();
	private static int m_PurchaseIndex = 0;

	// Preferences
	SharedPreferences m_Preferences;

	// Other required
	private Activity m_Activity = null;

	public AVInAppUtility(Activity activity) {
		AVUtility.DebugOut("AVInAppUtility", "CONSTRUCTING");
		m_Activity = activity;
		m_Preferences = m_Activity.getPreferences(Context.MODE_PRIVATE);
		onCreate();
	}
	
	private void OnPurchaseFailed(String purchaseID)
	{
		AVUtility.DebugOut("AVInAppUtility", "OnPurchaseFailed calling UnitySendMessage: (AVInAppUnity, PurchaseFailed, "+purchaseID+")");
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseFailed", purchaseID);
	}
	

	// -----------------
	// Interface
	// -----------------

	/**
	 * Sends a purchase request to the Google Play servers.
	 * 
	 * @param id
	 *            The ID of the purchase to request - must be already be set up
	 *            and <b>enabled</b> via Google admin panel
	 * @throws Exception
	 *             Throws if a purchase is already in progress
	 */
	public void requestPurchase(String id) throws Exception {
		hasRequestedInApp = true;
		// Check if we've recently requested an in-app purchase
		if (m_PurchaseRequested) {
			AVUtility.DebugOut("AVInAppManager: requestPurchase(" + id + ")",
					"Purchase already in progress - throwing exception");
			throw new Exception(
					"Purchase request already in progress - wait for a response from the server before making another.");
		}

		if (checkBillingSupported()) {
			// Send the request to the billing service
			if (!m_BillingService.requestPurchase(id, "" + m_PurchaseIndex)) {
				AVUtility.DebugOut("AVInAppManager: requestPurchase(" + id
						+ ")", "Unable to process in-app billing request");

				// Display message to user if failed
				AVUtility.MakeDialogBox("In-App Billing", "Unable to process in-app billing request.");
				OnPurchaseFailed(id);

				
				/*Toast.makeText(m_Activity,
						"Unable to process in-app billing request.",
						Toast.LENGTH_SHORT).show();*/
				m_PurchaseMessage = 1;

			} else {

				m_PurchaseIndex++;
				AVUtility.DebugOut("AVInAppManager: requestPurchase(" + id
						+ ")", "Billing request success");

				// Change message to 'not yet processed'
				m_PurchaseMessage = -1;
			}

		} else {
			AVUtility.DebugOut("AVInAppManager: requestPurchase(" + id + ")",
					"Billing is not available - cancelling purchase request");
			OnPurchaseFailed(id);
			m_PurchaseMessage = 1;
		}
	}

	/**
	 * Returns whether the user has already bought a managed in-app purchase.
	 * 
	 * @param id
	 *            The ID of the purchase to request - must be already be set up
	 *            and <b>enabled</b> via Google admin panel
	 * @return Whether the users account has previously bought this
	 *         <b>managed</b> in-app purchase
	 */
	public boolean hasPurchasedManaged(String id) {
		//AVUtility.DebugOut("InApp", "hasPuchasedManaged(" + id + ")");
		// SharedPreferences prefs =
		// m_Activity.getPreferences(Context.MODE_PRIVATE);
		return m_Preferences.getBoolean(id, false);
	}

	/**
	 * Returns the number of a particular <b>unmanaged(consumable)</b> in-app
	 * purchase that has been bought by the user.<br>
	 * Note that this number is not the amount of purchases left to process, but
	 * the amount the user has bought over the lifetime of the app.
	 * 
	 * @param id
	 *            The ID of the purchase to request - must be already be set up
	 *            and <b>enabled</b> via Google admin panel.
	 * @return Number of items of this ID this user has purchased over the
	 *         lifetime of the app.
	 */
	public int getAmountPurchased(String id) {
		return queryPurchaseAmount(id);
	}

	// ----------------------
	// Other functions
	// ----------------------

	public int getPurchaseMessage() {
		return m_PurchaseMessage;
	}

	public String[] getUnProcessed() {
		String[] retArray = new String[m_PurchaseList.size()];// (String[])
																// m_PurchaseList.toArray();
		for (int i = 0; i < retArray.length; i++) {
			retArray[i] = m_PurchaseList.get(i);
		}
		m_PurchaseList.clear();
		return retArray;
	}

	public String getPurchasedItemFromQueue() {
		return m_PurchaseItemQueue.poll();
	}

	// -----------------
	// Private functionality
	// -----------------
	@SuppressWarnings("deprecation")
	private void onCreate() {
		AVUtility.DebugOut("AVInAppManager: onCreate()", "onCreate called");
		m_Handler = new Handler();
		m_InAppPurchaseObserver = new InAppPurchaseObserver(m_Handler);
		m_BillingService = new BillingService();
		m_BillingService.setContext(m_Activity);

		m_PurchaseDatabase = new PurchaseDatabase(m_Activity);

		ResponseHandler.register(m_InAppPurchaseObserver);

		checkBillingSupported();

		m_OwnedItemsCursor = m_PurchaseDatabase.queryAllPurchasedItems();
		m_Activity.startManagingCursor(m_OwnedItemsCursor);

		m_BillingService.restoreTransactions();

	}

	public void onStart() {
		AVUtility.DebugOut("AVInAppUtility", "ONSTART");
		ResponseHandler.register(m_InAppPurchaseObserver);
	}

	/**
	 * Called when this activity is no longer visible.
	 */

	public void onStop() {
		ResponseHandler.unregister(m_InAppPurchaseObserver);
	}

	public void onDestroy() {
		if (m_PurchaseDatabase != null) {
			m_PurchaseDatabase.close();
		}
		if (m_BillingService != null) {
			m_BillingService.unbind();
		}
		try {
			ResponseHandler.unregister(m_InAppPurchaseObserver);
		} catch (Exception e) {
			Log.e("AVInAppUtility", "Exception caught in OnDestroy()");
			e.printStackTrace();
		}
	}

	public boolean checkBillingSupported() {
		if (!m_BillingService.checkBillingSupported()) {
			return false;
		}
		return true;
	}

	public void restoreDatabase() {

		boolean initialized = m_Preferences.getBoolean(DB_INITIALIZED, false);
		if (!initialized) {
			m_BillingService.restoreTransactions();
		}
		doInitializeOwnedItems();
	}

	// Querying
	private int queryPurchaseAmount(String purchaseID) {
		
		Cursor cursor = m_PurchaseDatabase.queryAllPurchasedItems();
		if (cursor == null) {
			AVUtility.DebugOut("AVInAppUtility", "ERROR: Cursor is null");
			return 0;
		}

		int productIdCol = cursor.getColumnIndexOrThrow(PurchaseDatabase.PURCHASED_PRODUCT_ID_COL);
		int productQ = cursor.getColumnIndexOrThrow(PurchaseDatabase.PURCHASED_QUANTITY_COL);
		int purchaseAmount = 0;
		while (cursor.moveToNext()) {
			String productId = cursor.getString(productIdCol);
			AVUtility.DebugOut(TAG, "processing productId: " + productId);
			if (productId.equals(purchaseID)) {
				purchaseAmount = cursor.getInt(productQ);
				AVUtility.DebugOut(TAG,productId + " amount:" + purchaseAmount);
			}
		}

		cursor.close();
		
		AVUtility.DebugOut("AVInAppUtility", "Querying purchase amount for: " + purchaseID + "("+purchaseAmount+")");
		
		return purchaseAmount;
	}

	public void doInitializeOwnedItems() {

		AVUtility.DebugOut("AVInAppUtility", "DoInitializeOwnedItems");
		Cursor cursor = m_PurchaseDatabase.queryAllPurchasedItems();
		if (cursor == null) {
			return;
		}

		final Set<String> ownedItems = new HashSet<String>();
		try {
			int productIdCol = cursor
					.getColumnIndexOrThrow(PurchaseDatabase.PURCHASED_PRODUCT_ID_COL);

			// int productQ =
			// cursor.getColumnIndexOrThrow(PurchaseDatabase.PURCHASED_QUANTITY_COL);
			// AVUtility.DebugOut("bought amount", ""+productIdCol);
			// int count = 0;
			while (cursor.moveToNext()) {
				String productId = cursor.getString(productIdCol);
				ownedItems.add(productId);

				// System.out.println("WE OWN " + productId + " " + count);
				// count += 1;

				// int amountOwned = cursor.getInt(productQ);

				// System.out.println("AMOUNT OWNED IS: " + amountOwned);

			}
		} finally {
			cursor.close();
		}

		// We will add the set of owned items in a new Runnable that runs on
		// the UI thread so that we don't need to synchronize access to
		// mOwnedItems.
		m_Handler.post(new Runnable() {
			public void run() {
				m_OwnedItems.addAll(ownedItems);
				// mCatalogAdapter.setOwnedItems(mOwnedItems);
			}
		});
	}

	// --------------------------
	// Observer
	// --------------------------

	private class InAppPurchaseObserver extends PurchaseObserver {
		public InAppPurchaseObserver(Handler handler) {
			super(m_Activity, handler);
		}

		@Override
		public void onBillingSupported(boolean supported) {
			AVUtility
					.DebugOut(
							"AVInAppManager: InAppPurchaseObserver: onBillingSupported",
							"Billing supported: " + supported);
			if (supported) {
				restoreDatabase();
			} else {
				if (hasRequestedInApp) {
					hasRequestedInApp = false;
					showConfirmBox(
							"In-App Billing",
							"Billing is not supported for this session. Please check for game updates and verify a google account is connected to this device.");
				}
			}
		}

		@SuppressWarnings("deprecation")
		public void onPurchaseStateChange(PurchaseState purchaseState,
				String itemId, int quantity, long purchaseTime,
				String developerPayload) {

			AVUtility
					.DebugOut(
							"AVInAppManager: InAppPurchaseObserver: onPurchaseStateChange()",
							"onPurchaseStateChange() itemId: " + itemId + " "
									+ purchaseState);

			if(purchaseState == PurchaseState.REFUNDED){
				AVUtility.DebugOut("AVInAppUtility", " PURCHASE REFUNDED: " + itemId);
			}
			
			if (purchaseState == PurchaseState.CANCELED) {
				AVUtility.DebugOut("AVInAppUtility", " PURCHASE CANCELED: " + itemId);
				m_PurchaseRequested = false;
				m_PurchaseMessage = 1;
				OnPurchaseFailed(itemId);
			}

			if (purchaseState == PurchaseState.PURCHASED) {
				AVUtility.DebugOut("AVInAppUtility", " PURCHASE SUCCESS: " + itemId);
				m_OwnedItems.add(itemId);
				// AVUtility.DebugOut("InApp", "Purchase Success");
				// SharedPreferences prefs =
				// m_Activity.getPreferences(Context.MODE_PRIVATE);
				Editor edit = m_Preferences.edit();
				edit.putBoolean(itemId, true);
				edit.commit();

				AVUtility.DebugOut(TAG, "bought " + itemId);

				m_PurchaseRequested = false;
				m_PurchaseMessage = 0;
				m_PurchaseList.add(itemId);
				m_PurchaseItemQueue.add(itemId);
			}

			m_OwnedItemsCursor.requery();
		}

		public void onRequestPurchaseResponse(RequestPurchase request,
				ResponseCode responseCode) {

			AVUtility.DebugOut("AVInAppManager: onRequestPurchaseResponse()",
					"Response recevied: " + responseCode.name());

			if (responseCode == ResponseCode.RESULT_OK) {

			} else if (responseCode == ResponseCode.RESULT_USER_CANCELED) {
				AVUtility.DebugOut("AVInAppUtility", " PURCHASE CANCELED: " + request.mProductId);
				m_PurchaseMessage = 1;
				OnPurchaseFailed(request.mProductId);
			} else {
				AVUtility.DebugOut("AVInAppUtility", " PURCHASE FAILED: " + request.mProductId);
				m_PurchaseMessage = 1;
				OnPurchaseFailed(request.mProductId);
			}
		}

		public void onRestoreTransactionsResponse(RestoreTransactions request,
				ResponseCode responseCode) {
			if (responseCode == ResponseCode.RESULT_OK) {

				AVUtility
						.DebugOut(TAG, "completed RestoreTransactions request");

				// Update the shared preferences so that we don't perform
				// a RestoreTransactions again.
				// SharedPreferences prefs =
				// m_Activity.getPreferences(Context.MODE_PRIVATE);
				SharedPreferences.Editor edit = m_Preferences.edit();
				edit.putBoolean(DB_INITIALIZED, true);
				edit.commit();
			} else {
				AVUtility.DebugOut(TAG, "RestoreTransactions error: "
						+ responseCode);
			}
		}

	}

	public void showConfirmBox(String title, String message) {
		// SharedPreferences settings = getSharedPreferences("AVAds", 0);
		// int opens = settings.getInt("times opened", 0);
		// boolean rated = settings.getBoolean("rated",false);
		// final Editor editor = settings.edit();
		// editor.putInt("times opened",opens+1);
		// editor.commit();

		// if(!rated)
		// {
		Message m = messageHandler.obtainMessage();
		Bundle bundle = new Bundle();
		bundle.putString("title", title);
		bundle.putString("message", message);
		m.setData(bundle);
		messageHandler.sendMessage(m);
		// }
	}

	private Handler messageHandler = new Handler() {
		public void handleMessage(Message msg) {
			String title = msg.getData().getString("title");
			final String packageString = msg.getData().getString("message");

			makeAlert(title, packageString);
		}
	};

	protected void makeAlert(String title, String message) {
		AlertDialog.Builder builder = new AlertDialog.Builder(m_Activity);
		builder.setTitle(title);
		builder.setMessage(message).setCancelable(false)
				.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int id) {

					}
				});
		AlertDialog alert = builder.create();
		try {
			alert.show();
		} catch (Exception e) {
			// System.out.println("ALERT FAILED");
		}
	}
}
