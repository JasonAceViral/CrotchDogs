using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AVInAppUnityIOS : AVInAppUnity {
    #if UNITY_IPHONE
	// DLL IMPORTS
	[DllImport ("__Internal")] private static extern void _avInAppPurchaseManagerRequestProduct( string appID );
	[DllImport ("__Internal")] private static extern void _avInAppPurchaseManagerRestorePurchases( );
	[DllImport ("__Internal")] private static extern int _avGetSuccessStatus( );
	[DllImport ("__Internal")] private static extern int _avGetRestoreSuccessStatus( );
	[DllImport ("__Internal")] private static extern string[] _avGetRestoredPurchases( );	
	[DllImport ("__Internal")] private static extern string[] _avRequestAppInformation( );
	[DllImport ("__Internal")] private static extern string[] _avAddIdentifierForInformationRequest( string productID, int index );

    #endif
    public void Awake() {
        this.gameObject.name = "AVInAppUnity";
    }

    public override void RequestPurchase(AVInAppID id) {
#if UNITY_IPHONE
		if(id.Managed){
			string scrambled = AVUtility.ScrambleString(id.iOSID);	
			if(PlayerPrefs.GetInt(scrambled, 0) == 1){
				AVMsgBox.Show("In App Purchases", "Sorry! You've already purchased this ONE-TIME In App Purchase. You can only receive this product once per installation.");
				return;
			}
		}
		_avInAppPurchaseManagerRequestProduct(id.iOSID);
#endif
    }

    public override void RestorePurchases() {
#if UNITY_IPHONE
		_avInAppPurchaseManagerRestorePurchases();
#endif
    }

    public void PurchaseNotification(string message) {
#if UNITY_IPHONE
        foreach(AVInAppID id in AVInAppUnity.m_PurchaseList){
			if(id.iOSID == message){
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

        foreach(AVInAppID id in AVInAppUnity.m_PurchaseList){
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
        foreach(AVInAppID id in AVInAppUnity.m_PurchaseList){
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
            _avAddIdentifierForInformationRequest(m_PurchaseList[i].iOSID, i);	
		}
		_avRequestAppInformation();
#endif
    }

    public void PurchaseDismissed(string ident) {
        OnPurchaseFailed(null);
    }
}