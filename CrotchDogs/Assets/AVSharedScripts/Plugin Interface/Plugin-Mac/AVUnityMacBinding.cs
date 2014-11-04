using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AVUnityMacBinding {
#if UNITY_STANDALONE_OSX
	[DllImport ("AVUnityMacPlugin")] private static extern void _avUnityMacShowAlert(string title, string message);
	[DllImport ("AVUnityMacPlugin")] private static extern void _avInAppPurchaseManagerRequestProduct( string appID );
	[DllImport ("AVUnityMacPlugin")] private static extern void _avInAppPurchaseManagerRestorePurchases( );
	[DllImport ("AVUnityMacPlugin")] private static extern void _avInAppPurchaseManagerTestPurchaseCallback( string appID );
	[DllImport ("AVUnityMacPlugin")] private static extern void _avUnityMacFlushMessages();
#endif
	
	public static void ShowAlert(string title, string message){
#if UNITY_STANDALONE_OSX
		if(Application.platform != RuntimePlatform.OSXEditor){
			_avUnityMacShowAlert(title, message);
		}
#endif
	}
	
	public static void RequestInAppPurchase(string purchaseID){
#if UNITY_STANDALONE_OSX
		if(Application.platform != RuntimePlatform.OSXEditor){
			_avInAppPurchaseManagerRequestProduct(purchaseID);
		}
#endif
	}
	
	public static void RequestRestorePurchases(){
#if UNITY_STANDALONE_OSX
		if(Application.platform != RuntimePlatform.OSXEditor){
			_avInAppPurchaseManagerRestorePurchases();	
		}
#endif
	}
	
	public static void RequestTestPurchase(string id){
#if UNITY_STANDALONE_OSX
		_avInAppPurchaseManagerTestPurchaseCallback(id);	
#endif
	}
	
	public static void FlushPurchases(){
#if UNITY_STANDALONE_OSX
		_avUnityMacFlushMessages();
#endif
	}
}
