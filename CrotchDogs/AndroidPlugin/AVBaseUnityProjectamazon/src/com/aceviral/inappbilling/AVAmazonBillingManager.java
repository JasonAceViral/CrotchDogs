package com.aceviral.inappbilling;

import java.util.HashSet;
import java.util.Set;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;

import com.aceviral.BillingInterface;
import com.aceviral.utility.AVUtility;
import com.amazon.inapp.purchasing.Item;
import com.amazon.inapp.purchasing.PurchasingManager;
import com.unity3d.player.UnityPlayer;

public class AVAmazonBillingManager implements BillingInterface{

	private AVAmazonBillingObserver m_BillingObserver;
	private final Activity m_MainActivity;

	public Set<String> SKUSet = new HashSet<String>();

	public AVAmazonBillingManager(Activity mainActivity){
		m_MainActivity = mainActivity;
		onStart();
	}

	public void onStart(){
		m_BillingObserver = new AVAmazonBillingObserver(this);
		PurchasingManager.registerObserver(m_BillingObserver);
	}

	public void onResume(){
		PurchasingManager.initiateGetUserIdRequest();
	}

	public Activity getActivity(){
		return m_MainActivity;
	}

	public void addSkuKey(String sku, boolean isConsumable) {
		SKUSet.add(sku);
	}

	public void requestPurchase(String sku){
		Log.v("AVAmazonBillingManager", "Requesting product: " + sku);
		PurchasingManager.initiatePurchaseRequest(sku);
	}

	public void requestPurchasePrices(){
		PurchasingManager.initiateItemDataRequest(SKUSet);
	}

	public void setCurrentUser(String userId){

	}

	// ### Purchase Events ###

	// SKU CAN be null so watch out

	public void onPurchaseSKUInformationGained(Item item){
		if(item != null){
			AVUtility.DebugOut("AVAmazonBillingManager", "OnPurchaseDataReceived: " + item.getSku() + " / " + item.getPrice());
			UnityPlayer.UnitySendMessage("AVInAppUnity", "OnPurchaseDataReceived", item.getSku() + "#" + item.getPrice());
		} else {
			Log.w("AVAmazonBillingManager", "onPurchaseSKUInformationGained contained a null item");
		}
	}

	public void onConsumablePurchaseSuccess(String sku){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseNotification", sku);
		AVUtility.DebugOut("AVAmazonBillingManager", "onConsumablePurchaseSuccess: " + sku);
		//showConfirmBox("onConsumablePurchaseSuccess",sku);
	}

	public void onManagedPurchaseSuccess(String sku){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseNotification", sku);
		AVUtility.DebugOut("AVAmazonBillingManager", "onManagedPurchaseSuccess: " + sku);
		//showConfirmBox("onManagedPurchaseSuccess",sku);
	}

	public void onManagedPurchaseAlreadyBought(String sku){
		AVUtility.DebugOut("AVAmazonBillingManager", "onManagedPurchaseAlreadyBought: " + sku);
		//showConfirmBox("onManagedPurchaseAlreadyBought",sku);
	}

	public void onManagedPurchaseRevoked(String sku){
		AVUtility.DebugOut("AVAmazonBillingManager", "onManagedPurchaseAlreadyBought: " + sku);
		//showConfirmBox("onManagedPurchaseRevoked",sku);
	}

	public void onManagedPurchaseRestored(String sku){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseNotification", sku);
		AVUtility.DebugOut("AVAmazonBillingManager", "onManagedPurchaseRestored: " + sku);
		//showConfirmBox("onManagedPurchaseRestored",sku);
	}

	public void onSubscriptionPurchaseSuccess(String sku){
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseNotification", sku);
		AVUtility.DebugOut("AVAmazonBillingManager", "onPurchaseFailed: " + sku);
		//showConfirmBox("onSubscriptionPurchaseSuccess",sku);
	}

	public void onPurchaseFailed(String sku){
		// This shit appears even if you press a purchase and then just cancel it.
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseFailed", "Purchase failed.");
		AVUtility.DebugOut("AVAmazonBillingManager", "onPurchaseFailed: " + sku);
		//showConfirmBox("In-App Billing", "Purchase failed.");
	}

	public void onPurchaseFailedWithInvalidSKU(String sku){
		//showConfirmBox("In-App Billing", "Developer error! The product SKU was invalid. Please contact AceViral and state which item you wanted to buy along with this error message.");
		UnityPlayer.UnitySendMessage("AVInAppUnity", "PurchaseFailed", "Developer error! The product SKU was invalid. Please contact AceViral and state which item you wanted to buy along with this error message.");
		AVUtility.DebugOut("AVAmazonBillingManager", "onPurchaseFailedWithInvalidSKU: " + sku);
		//showConfirmBox("onPurchaseFailedWithInvalidSKU",sku);
	}

	@Override
	public void onDestroy() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public boolean onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public void setEncodedPublicKey(String key) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void restorePurchases() {
		// TODO Auto-generated method stub
		
	}
}