#if UNITY_WP8 || UNITY_METRO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AVWindowsPhonePlugin;
using AVSharedScripts;

namespace AVHiddenInterface {
    public class InAppUnityWin : InAppUnity{

        void Awake() 
		{
            AVWP8InAppPurchase.Instance.OnInAppPurchaseSuccess += OnInAppPurchaseSuccess;
            AVWP8InAppPurchase.Instance.OnInAppPurchaseFailed += PurchaseFailed;
            AVWP8InAppPurchase.Instance.OnInAppPurchaseDataReceived += OnPurchaseDataReceived;
        }

        void OnInAppPurchaseSuccess(string data) {
			foreach (InAppID id in InAppUnity.m_PurchaseList)
			{
                if (id.WindowsPhoneID == data) {
                    if (id.Managed) {
                        string scrambled = Utility.ScrambleString(data);
                        if (PlayerPrefs.GetInt(scrambled, 0) == 0) {
                            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                                Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
                            if (id.OnBought != null) {
                                id.OnBought();
                                PlayerPrefs.SetInt(scrambled, 1);
                            }
                        }
                    } else {
                        MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                            Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
                        if (id.OnBought != null) {
                            id.OnBought();
                        }
                    }

                    break;
                }
            }
        }
        public override void RequestPurchase(InAppID id) {
            WP8InAppPurchase.Instance.RequestPurchase(id.WindowsPhoneID);
        }

        public override void RestorePurchases() {
            WP8InAppPurchase.Instance.RestorePurchases();
        }

        public override void PurchaseFailed(string message) {
			foreach (InAppID id in InAppUnity.m_PurchaseList)
			{
                if (id.WindowsPhoneID == message) {
                    OnPurchaseFailed(id);
                    return;
                }
            }
        }

        public override void OnPurchaseDataReceived(string data) {
            string SKU = data.Split('#')[0];
            string price = data.Split('#')[1];
			foreach (InAppID id in InAppUnity.m_PurchaseList)
			{
                if (SKU == id.WindowsPhoneID) {
                    id.Price = price;
                    break;
                }
            }	
        }

        public override void RequestPurchasePrices() {
            WP8InAppPurchase.Instance.RequestPurchasePrices();
        }
    }
}
#endif