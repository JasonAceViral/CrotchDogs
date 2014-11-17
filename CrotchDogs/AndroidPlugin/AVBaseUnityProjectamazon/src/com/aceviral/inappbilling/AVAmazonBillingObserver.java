package com.aceviral.inappbilling;

import java.util.Map;

import android.os.AsyncTask;
import android.util.Log;

import com.amazon.inapp.purchasing.BasePurchasingObserver;
import com.amazon.inapp.purchasing.GetUserIdResponse;
import com.amazon.inapp.purchasing.GetUserIdResponse.GetUserIdRequestStatus;
import com.amazon.inapp.purchasing.Item;
import com.amazon.inapp.purchasing.Item.ItemType;
import com.amazon.inapp.purchasing.ItemDataResponse;
import com.amazon.inapp.purchasing.Offset;
import com.amazon.inapp.purchasing.PurchaseResponse;
import com.amazon.inapp.purchasing.PurchaseUpdatesResponse;
import com.amazon.inapp.purchasing.PurchasingManager;
import com.amazon.inapp.purchasing.Receipt;

/***
 * Observer class for Amazon In-App Billing
 * 
 * <p>
 * Deals with incoming purchase responses and runs Async tasks on them. <br>
 * Only AVAmazonBillingManager needs this class, so there's no need to use it
 * overwise.
 * 
 * <br>
 * Basically leave this class the hell alone unless you absolutely need to
 * change it due to errors.
 * 
 * <p>
 * Doesn't yet support subscriptions
 * 
 * @author Phil
 * 
 */
public class AVAmazonBillingObserver extends BasePurchasingObserver {
	private final String TAG = "AVAmazonBillingObserver";

	AVAmazonBillingManager m_BillingManager = null;

	public AVAmazonBillingObserver(AVAmazonBillingManager manager) {
		super(manager.getActivity());
		m_BillingManager = manager;
	}

	@Override
	public void onSdkAvailable(final boolean isSandboxMode) {
		Log.v(TAG, "onSdkAvailable recieved: Response -" + isSandboxMode);
		PurchasingManager.initiateGetUserIdRequest();
	}

	@Override
	public void onGetUserIdResponse(final GetUserIdResponse getUserIdResponse) {
		Log.v(TAG, "onGetUserIdResponse recieved: Response -" + getUserIdResponse);
		Log.v(TAG, "RequestId:" + getUserIdResponse.getRequestId());
		Log.v(TAG, "IdRequestStatus:" + getUserIdResponse.getUserIdRequestStatus());
		new GetUserIdAsyncTask().execute(getUserIdResponse);
	}

	@Override
	public void onItemDataResponse(final ItemDataResponse itemDataResponse) {
		Log.v(TAG, "onItemDataResponse recieved");
		Log.v(TAG, "ItemDataRequestStatus" + itemDataResponse.getItemDataRequestStatus());
		Log.v(TAG, "ItemDataRequestId" + itemDataResponse.getRequestId());
		new ItemDataAsyncTask().execute(itemDataResponse);
	}

	@Override
	public void onPurchaseResponse(final PurchaseResponse purchaseResponse) {
		Log.v(TAG, "onPurchaseResponse recieved");
		Log.v(TAG, "PurchaseRequestStatus:" + purchaseResponse.getPurchaseRequestStatus());
		new PurchaseAsyncTask().execute(purchaseResponse);
	}

	@Override
	public void onPurchaseUpdatesResponse(final PurchaseUpdatesResponse purchaseUpdatesResponse) {
		Log.v(TAG, "onPurchaseUpdatesRecived recieved: Response -" + purchaseUpdatesResponse);
		Log.v(TAG, "PurchaseUpdatesRequestStatus:" + purchaseUpdatesResponse.getPurchaseUpdatesRequestStatus());
		Log.v(TAG, "RequestID:" + purchaseUpdatesResponse.getRequestId());
		new PurchaseUpdatesAsyncTask().execute(purchaseUpdatesResponse);
	}

	// ### ASync Tasks ###

	private class GetUserIdAsyncTask extends AsyncTask<GetUserIdResponse, Void, Boolean> {
		@Override
		protected Boolean doInBackground(GetUserIdResponse... params) {
			GetUserIdResponse userIdResponse = params[0];
			if (userIdResponse.getUserIdRequestStatus() == GetUserIdRequestStatus.SUCCESSFUL) {
				m_BillingManager.setCurrentUser(userIdResponse.getUserId());
				return true;
			} else {
				return false;
			}
		}

		@Override
		protected void onPostExecute(final Boolean result) {
			super.onPostExecute(result);
			if (result) {
				PurchasingManager.initiatePurchaseUpdatesRequest(Offset.BEGINNING);
			}
		}
	}

	private class ItemDataAsyncTask extends AsyncTask<ItemDataResponse, Void, Void> {
		@Override
		protected Void doInBackground(final ItemDataResponse... params) {
			try {
				final ItemDataResponse itemDataResponse = params[0];
				if (itemDataResponse != null) {
					switch (itemDataResponse.getItemDataRequestStatus()) {
					case SUCCESSFUL_WITH_UNAVAILABLE_SKUS:
						ProcessIncomingItemData(itemDataResponse);
						break;
					case SUCCESSFUL:
						ProcessIncomingItemData(itemDataResponse);
						break;
					case FAILED:
						break;
					}
				}
			} catch (Exception e) {
				Log.e("AVAmazonBillingObserver", "Exception observed during ItemDataAsyncTask.doInBackground(): " + e.getMessage());
				e.printStackTrace();
			}
			return null;
		}
	}

	private void ProcessIncomingItemData(ItemDataResponse dataResponse){
		Map<String, Item> itemData = dataResponse.getItemData();
		if (itemData != null) {
			if (m_BillingManager != null && m_BillingManager.SKUSet != null) {
				for (String s : m_BillingManager.SKUSet) {
					m_BillingManager.onPurchaseSKUInformationGained(itemData.get(s));
				}
			} else {
				Log.w("AVAmazonBillingObserver", "m_BillingManager null reference issue");
			}
		} else {
			Log.w("AVAmazonBillingObserver", "Issue getting item data from ItemDataResponse");
		}
	}

	private class PurchaseAsyncTask extends AsyncTask<PurchaseResponse, Void, Boolean> {
		@Override
		protected Boolean doInBackground(PurchaseResponse... params) {
			if (m_BillingManager == null) {
				Log.e(TAG, "Billing Manager is NULL!!!! WTF?!@!??!!");
			}
			final PurchaseResponse purchaseResponse = params[0];
			final Receipt purchaseReceipt = purchaseResponse.getReceipt();
			switch (purchaseResponse.getPurchaseRequestStatus()) {
			case SUCCESSFUL:
				switch (purchaseReceipt.getItemType()) {
				case CONSUMABLE:
					m_BillingManager.onConsumablePurchaseSuccess(purchaseReceipt.getSku());
					break;
				case ENTITLED:
					m_BillingManager.onManagedPurchaseSuccess(purchaseReceipt.getSku());
					break;
				case SUBSCRIPTION:
					m_BillingManager.onSubscriptionPurchaseSuccess(purchaseReceipt.getSku());
					break;
				}
				return true;
			case FAILED:
				if (purchaseReceipt != null) {
					m_BillingManager.onPurchaseFailed(purchaseReceipt.getSku());
				} else {
					m_BillingManager.onPurchaseFailed(null);
				}

				return false;
			case INVALID_SKU:
				if (purchaseReceipt != null) {
					m_BillingManager.onPurchaseFailedWithInvalidSKU(purchaseReceipt.getSku());
				} else {
					m_BillingManager.onPurchaseFailedWithInvalidSKU(null);
				}
				return false;
			case ALREADY_ENTITLED:

				if (purchaseReceipt != null) {
					m_BillingManager.onManagedPurchaseAlreadyBought(purchaseReceipt.getSku());
				} else {
					m_BillingManager.onManagedPurchaseAlreadyBought(null);
				}

				return false;
			}
			return false;
		}

		@Override
		protected void onPostExecute(final Boolean success) {
			super.onPostExecute(success);
		}
	}

	private class PurchaseUpdatesAsyncTask extends AsyncTask<PurchaseUpdatesResponse, Void, Boolean> {

		@Override
		protected Boolean doInBackground(PurchaseUpdatesResponse... params) {
			PurchaseUpdatesResponse response = params[0];

			for (final String sku : response.getRevokedSkus()) {
				m_BillingManager.onManagedPurchaseRevoked(sku);
			}

			switch (response.getPurchaseUpdatesRequestStatus()) {
			case SUCCESSFUL:
				for (final Receipt receipt : response.getReceipts()) {
					if (receipt.getItemType() == ItemType.ENTITLED) {
						m_BillingManager.onManagedPurchaseRestored(receipt.getSku());
					}
				}
				if (response.isMore()) {
					PurchasingManager.initiatePurchaseUpdatesRequest(response.getOffset());
				}
				return true;
			case FAILED:
				return false;
			}

			return false;
		}

	}

}