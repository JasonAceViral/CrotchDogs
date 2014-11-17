#if UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InAppUnityAndoid : InAppUnity
    {
        protected override void OnStart()
        {
            AndroidInterface.InAppBilling.SetEncodedPublicKey(AppConstants.IAP.AndroidIapEncodedKey);
        }

        public override void PostProcessPurchaseInformation(InAppID id)
        {
            AndroidInterface.InAppBilling.AddSkuKey(id.AndroidID, id.isConsumable);
        }
    	
    	public override void RequestPurchase (InAppID id) {

    		AndroidInterface.InAppBilling.RequestPurchase(id.AndroidID);
    	}

    	public override void RestorePurchases () {

    		AndroidInterface.InAppBilling.RestorePurchases();
    	}

    	public void PurchaseSuccess(string message){

    		foreach (InAppID id in InAppUnity.m_PurchaseList)
    		{
    			if(id.AndroidID == message){
    				if(id.Managed){
    					string scrambled = Utility.ScrambleString(message);
    					if(PlayerPrefs.GetInt(scrambled, 0) == 0){
                            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                                Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
    						if(id.OnBought != null){
    							id.OnBought();
    							PlayerPrefs.SetInt(scrambled, 1);
    						}
    					}
    				} else {
                        MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                            Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
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

    		foreach (InAppID id in InAppUnity.m_PurchaseList)
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
    		foreach (InAppID id in InAppUnity.m_PurchaseList)
    		{
    			if(SKU == id.AndroidID){
    				id.Price = price;
    				break;
    			}
    		}		
    	}
    	
    	public override void RequestPurchasePrices ()
    	{
    		AndroidInterface.InAppBilling.RequestPurchasePrices();
    	}

        protected override void OnPurchaseFailed (InAppID id)
        {
            if (id == null)
            {
                MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                    Localization.Instance.GetString(Localization.eLocalKeys.AV_Purchase_Failed_Already_Owned) + " " +
                    Localization.Instance.GetString(Localization.eLocalKeys.AV_Purchase_Would_You_Like_Restore_Purchases), MsgBoxType.YES_NO, (response) =>
                {
                    if (response == MsgBoxResponse.YES)
                    {
                        AndroidInterface.InAppBilling.RestorePurchases();
                    }
                });
            }
            else
            {
                MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                    Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Purchase_Failed_For_x, id.Name));
            }

            if(PurchaseFailedHandle != null){
                PurchaseFailedHandle(id);
            }
        }
    }
}
#endif