using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

class AVHouseAdInterface {

    #if UNITY_IPHONE && !UNITY_EDITOR
    [DllImport ("__Internal")] private static extern void _avOpenHouseAdLink( string url );
    [DllImport ("__Internal")] private static extern void _avOpenMoreGamesPage( );
    #endif

	public static void OpenHouseAdUrl(string urlLink, string fromSlotId)
    {
        #if UNITY_IPHONE && !UNITY_EDITOR
        _avOpenHouseAdLink(urlLink);
        #elif UNITY_ANDROID && !UNITY_EDITOR

		AVAndroidInterface.HouseAds.ShowAppLinkInMarketPlaceWithReferral(urlLink, AVAppConstants.appReferralName, fromSlotId);
		#elif (UNITY_WP8 || UNITY_METRO) && !UNITY_EDITOR
		AVWindowsPhonePlugin.AVWindowsPhoneUtility.OpenMarketplaceWithStoreCode(urlLink);
        #endif
    }

    public static void OpenMoreGamesPage()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
       
            AVAndroidInterface.HouseAds.ShowMoreGamesInMarketPlace();

        #elif !UNITY_EDITOR && UNITY_IPHONE
        _avOpenMoreGamesPage();

		#elif (UNITY_WP8 || UNITY_METRO) && !UNITY_EDITOR
		AVWindowsPhonePlugin.AVWindowsPhoneUtility.ShowMoreGamesInMarketplace();
        #endif
    }
}
