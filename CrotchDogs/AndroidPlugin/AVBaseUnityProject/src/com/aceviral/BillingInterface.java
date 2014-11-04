package com.aceviral;

import android.content.Intent;

public interface BillingInterface 
{

	public void onDestroy();

	public boolean onActivityResult(int requestCode, int resultCode, Intent data);

	public void setEncodedPublicKey(String key);

	public void addSkuKey(String sku, boolean isConsumable);

	public void requestPurchase(String id);

	public void requestPurchasePrices();

	public void restorePurchases();

}
