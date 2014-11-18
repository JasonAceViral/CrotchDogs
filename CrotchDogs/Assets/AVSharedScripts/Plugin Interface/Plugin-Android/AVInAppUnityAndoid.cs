#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

public class AVInAppUnityAndoid : AVInAppUnity
{

	public void Awake()
	{
		this.gameObject.name = "AVInAppUnity";
	}

    protected override void OnStart()
    {
        AVAndroidInterface.InAppBilling.SetEncodedPublicKey(AVAppConstants.AndroidIapEncodedKey);
    }

    public override void PostProcessPurchaseInformation(AVInAppID id)
    {
        AVAndroidInterface.InAppBilling.AddSkuKey(id.AndroidID, id.isConsumable);
    }
	
	public override void RequestPurchase (AVInAppID id) {

		AVAndroidInterface.InAppBilling.RequestPurchase(id.AndroidID);
	}

	public override void RestorePurchases () {

		AVAndroidInterface.InAppBilling.RestorePurchases();
	}

	public void PurchaseSuccess(string message){

		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if(id.AndroidID == message){
				if(id.Managed){
					string scrambled = AVUtility.ScrambleString(message);
					if(PlayerPrefs.GetInt(scrambled, 0) == 0){
						AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
						if(id.OnBought != null){
							id.OnBought();
							PlayerPrefs.SetInt(scrambled, 1);
						}
					}
				} else {
					AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
					if(id.OnBought != null){
						id.OnBought();
					}
				}
				
				break;	
			}
		}
	}	
	
	public override void PurchaseFailed (string message) {

        // If the purchase failed early, it may not return an SKU. So check for 'unkown'
        if (message == "unknown")
        {
            OnPurchaseFailed(null);
            return;
        }

		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if(id.AndroidID == message){
				OnPurchaseFailed(id);
				return;
			}
		}
	}
	
	public override void OnPurchaseDataReceived (string data) {
		string SKU = data.Split('#')[0];
		string price = data.Split('#')[1];
		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if(SKU == id.AndroidID){
				id.Price = price;
				break;
			}
		}		
	}
	
	public override void RequestPurchasePrices ()
	{
		AVAndroidInterface.InAppBilling.RequestPurchasePrices();
	}

    public override void OnPurchaseFailed (AVInAppID id)
    {
        if (id == null)
        {
            AVMsgBox.Show("In-App Purchases", "Purchase failed. You may already own this item. Would you like to restore previous purchases?", AVMsgBoxType.YES_NO, (response) =>
            {
                if (response == AVMsgBoxResponse.YES)
                {
                    AVAndroidInterface.InAppBilling.RestorePurchases();
                }
            });
        }
        else
        {
            AVMsgBox.Show("In-App Purchases", "Purchase failed for: " + id.Name);
        }

        HandleFailedPurchase(id);
    }
}
#endif