package com.aceviral.inappbilling;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Set;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;

import com.aceviral.BillingInterface;
import com.aceviral.utility.AVUtility;
import com.unity3d.player.UnityPlayer;

public class InAppBilling implements BillingInterface
{
	// Debug tag, for logging
	static final String TAG = "In App Billing";

	// (arbitrary) request code for the purchase flow
	static final int RC_REQUEST = 10001;

	private final ArrayList<String> m_SkuList = new ArrayList<String>();
	private final ArrayList<String> m_ManagedSkuList = new ArrayList<String>(); // Non-Consumable purchases

	// The helper object
	IabHelper mHelper;
	Activity inAppActivity;

	ArrayList<String> inAppPrices;
	HashMap<String, String> m_IAPPrices = new HashMap<String, String>();

	private boolean encodedKeyHasBeenSet = false;

	private Activity m_Activity;
	
	public InAppBilling(Activity activity)
	{
		this.m_Activity = activity;
		onCreate(m_Activity);
	}


	// ############################################################
	// -----
	// Interface Methods
	// -----
	// ############################################################

	public void setEncodedPublicKey(String base64EncodedPublicKey)
	{
		encodedKeyHasBeenSet = true;
		mHelper.mSignatureBase64 = base64EncodedPublicKey;
	}

	public void addSkuKey(String sku, boolean isConsumable) {
		m_SkuList.add(sku);

		if(!isConsumable)
		{
			m_ManagedSkuList.add(sku);
		}

		Log.w("InAppBilling", "Added new sku number to list. Was consumable? " + isConsumable);
	}

	public void requestPurchase(String SKU){
		if(!encodedKeyHasBeenSet)
		{
			Log.e("InAppBilling", "Encoded key has not been set. Cannot continue.");
			return;
		}
		mHelper.launchPurchaseFlow(inAppActivity, SKU, RC_REQUEST, mPurchaseFinishedListener);
	}

	public void restorePurchases(){
		inAppActivity.runOnUiThread(new RestorePurchaseThread());
	}

	public void requestPurchasePrices()
	{
		if(m_IAPPrices.isEmpty())
		{
			inAppActivity.runOnUiThread(new RestorePurchaseThread());
		}
		
		Set<String> keySet = m_IAPPrices.keySet();
		for(String s : keySet){
			UnityPlayer.UnitySendMessage("AVInAppUnity", "OnPurchaseDataReceived", s + "#" + m_IAPPrices.get(s));
		}
	}

	private boolean skuKeyIsManaged(String sku) {

		return m_ManagedSkuList.contains(sku);
	}


	// ############################################################
	// -----
	// Callback Methods
	// -----
	// ############################################################
	private void OnConsumablePurchaseSuccess(String SKU){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseSuccess", SKU);
	}

	private void OnConsumablePurchaseFailed(String SKU){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseFailed", SKU);
	}

	private void OnManagedPurchaseSuccess(String SKU){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseSuccess", SKU);
	}

	// ############################################################
	// -----
	// Threaded Methods
	// -----
	// ############################################################

	private final class RestorePurchaseThread implements Runnable {
		@Override
		public void run() {
			mHelper.queryInventoryAsync(true, m_SkuList, mGotInventoryListener);
		}
	}


	// call in main activitys on create, will check for what items are owned and
	// if any consumables havent been consumed

	public void onCreate(Activity act)
	{
		inAppPrices = new ArrayList<String>();
		inAppActivity = act;

		// Create the helper, passing it our context and the public key to verify signatures with
		mHelper = new IabHelper(inAppActivity, "no key set");

		// enable debug logging (for a production application, you should set this to false).
		mHelper.enableDebugLogging(false);

		// Start setup. This is asynchronous and the specified listener
		// will be called once setup completes.
		mHelper.startSetup(new IabHelper.OnIabSetupFinishedListener() {
			@Override
			public void onIabSetupFinished(IabResult result) {
				if (result.isSuccess()) {
					restorePurchases();
				} else {
					whinge("Problem setting up in-app billing: " + result);
				}
			}
		});
	}

	// Listener that's called when we finish querying the items we own
	IabHelper.QueryInventoryFinishedListener mGotInventoryListener = new IabHelper.QueryInventoryFinishedListener() {
		@Override
		public void onQueryInventoryFinished(IabResult result,
				Inventory inventory) {
			if (result.isFailure()) {
				whinge("Failed to query current inventory");
				return;
			}
			
			Set<String> keySet = m_IAPPrices.keySet();
			for(String s : keySet){
				
			}

			for (String s : m_SkuList) {
				try {
					SkuDetails deets = inventory.getSkuDetails(s);
					if (deets != null) {
						String price = deets.getPrice();
						if (price.equals("")) {
							whinge("Price for " + s + " was null");
						} else {
							m_IAPPrices.put(s, price);
							UnityPlayer.UnitySendMessage("AVInAppUnity", "OnPurchaseDataReceived", s + "#" + price);
						}
					} else {
						whinge("Details for SKU ["+s+"] were null");
					}

				} catch (Exception e) {
					e.printStackTrace();
				}
			}

			for (String s : m_SkuList) {
				if (inventory.hasPurchase(s)) {
					if (skuKeyIsManaged(s)) {
						OnManagedPurchaseSuccess(s);
					} else {
						mHelper.consumeAsync(inventory.getPurchase(s),
								mConsumeFinishedListener);
					}

				}
			}
		}
	};

	// call this method on main activitys result, if returns false then perform
	// super.onActivityResult(int requestCode, int resultCode, Intent data)
	public boolean onActivityResult(int requestCode, int resultCode, Intent data) {
		return mHelper.handleActivityResult(requestCode, resultCode, data);
	}

	// Callback for when a purchase is finished
	// Check for what has been purchased here, call mHelper.consumeAsynch if consumable else dont
	IabHelper.OnIabPurchaseFinishedListener mPurchaseFinishedListener = new IabHelper.OnIabPurchaseFinishedListener() {
		@Override
		public void onIabPurchaseFinished(IabResult result, Purchase purchase) {

			if (result.isFailure())
			{
				OnConsumablePurchaseFailed(purchase == null ? "unknown" : purchase.getSku());
			}
			else
			{
				if (skuKeyIsManaged(purchase.getSku()))
				{
					OnManagedPurchaseSuccess(purchase.getSku());
				} else
				{
					mHelper.consumeAsync(purchase, mConsumeFinishedListener);
				}

			}
		}
	};

	public String getInAppPrice(int pos) {
		String returnString = "";
		try {
			returnString = inAppPrices.get(pos);
		} catch (Exception e) {

		}
		return returnString;
	}

	IabHelper.OnConsumeFinishedListener mConsumeFinishedListener = new IabHelper.OnConsumeFinishedListener() {
		@Override
		public void onConsumeFinished(Purchase purchase, IabResult result) {
			if (result.isSuccess()) {
				OnConsumablePurchaseSuccess(purchase.getSku());
			} else {
				whinge("Error while consuming: " + result);
			}
		}
	};

	// We're being destroyed. It's important to dispose of the helper here!
	// call this on main activitys on destroy
	public void onDestroy() {
		if (mHelper != null) {
			mHelper.dispose();
		}
		mHelper = null;
	}

	private void whinge(String message){
		AVUtility.DebugOut("InAppBilling", message);
	}
}
