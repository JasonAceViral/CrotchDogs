using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InAppUnityIOS : InAppUnity {

        public override void RequestPurchase(InAppID id) {
    #if UNITY_IPHONE
    		if(id.Managed){
    			string scrambled = Utility.ScrambleString(id.iOSID);	
    			if(PlayerPrefs.GetInt(scrambled, 0) == 1){
                    MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                        Localization.Instance.GetString(Localization.eLocalKeys.AV_Purchase_Failed_Already_Owned));
    				return;
    			}
    		}
            IPhoneInterface.IAP.RequestProduct(id.iOSID);
    #endif
        }

        public override void RestorePurchases() {
    #if UNITY_IPHONE
            IPhoneInterface.IAP.RestorePurchases();
    #endif
        }

        public void PurchaseNotification(string message) {
    #if UNITY_IPHONE
            foreach(InAppID id in InAppUnity.m_PurchaseList){
    			if(id.iOSID == message){
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
    #endif
        }

        public override void PurchaseFailed(string message) {
    #if UNITY_IPHONE
            // Some cases return a null purchase ID'
            if (message == @"unknown")
            {
                OnPurchaseFailed(null);
                return;
            }

            foreach(InAppID id in InAppUnity.m_PurchaseList){
    			if(id.iOSID == message){
    				OnPurchaseFailed(id);
    				break;
    			}
    		}
    #endif
        }

        public override void OnPurchaseDataReceived(string data) {
    #if UNITY_IPHONE
    		string SKU = data.Split('#')[0];
    		string price = data.Split('#')[1];
            foreach(InAppID id in InAppUnity.m_PurchaseList){
    			if(SKU == id.iOSID){
    				id.Price = price;
    				break;
    			}
    		}
    #endif
        }

        public override void RequestPurchasePrices() {
    #if UNITY_IPHONE
            for(int i = 0; i < m_PurchaseList.Count; i++){
                IPhoneInterface.IAP.AddIdentifierForInformationRequest(m_PurchaseList[i].iOSID, i);	
    		}
            IPhoneInterface.IAP.RequestIapInformation();
    #endif
        }

        public void PurchaseDismissed(string ident) {
            OnPurchaseFailed(null);
        }
    }
}