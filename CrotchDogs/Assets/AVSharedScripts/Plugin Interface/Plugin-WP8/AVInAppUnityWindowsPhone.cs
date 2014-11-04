#if UNITY_WP8
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AVWindowsPhonePlugin;

    public class AVInAppUnityWindowsPhone : AVInAppUnity{

        void Awake() 
		{
			this.gameObject.name = "AVInAppUnity";
            AVWP8InAppPurchase.Instance.OnInAppPurchaseSuccess += OnInAppPurchaseSuccess;
            AVWP8InAppPurchase.Instance.OnInAppPurchaseFailed += PurchaseFailed;
            AVWP8InAppPurchase.Instance.OnInAppPurchaseDataReceived += OnPurchaseDataReceived;
        }

        void OnInAppPurchaseSuccess(string data) {
            foreach (AVInAppID id in AVInAppUnity.m_PurchaseList) {
                if (id.WindowsPhoneID == data) {
                    if (id.Managed) {
                        string scrambled = AVUtility.ScrambleString(data);
                        if (PlayerPrefs.GetInt(scrambled, 0) == 0) {
                            AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
                            if (id.OnBought != null) {
                                id.OnBought();
                                PlayerPrefs.SetInt(scrambled, 1);
                            }
                        }
                    } else {
                        AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
                        if (id.OnBought != null) {
                            id.OnBought();
                        }
                    }

                    break;
                }
            }
        }
        public override void RequestPurchase(AVInAppID id) {
            AVWP8InAppPurchase.Instance.RequestPurchase(id.WindowsPhoneID);
        }

        public override void RestorePurchases() {
            AVWP8InAppPurchase.Instance.RestorePurchases();
        }

        public override void PurchaseFailed(string message) {
			foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
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
			foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
			{
                if (SKU == id.WindowsPhoneID) {
                    id.Price = price;
                    break;
                }
            }	
        }

        public override void RequestPurchasePrices() {
            AVWP8InAppPurchase.Instance.RequestPurchasePrices();
        }
    }

#endif