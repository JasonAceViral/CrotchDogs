using UnityEngine;
using System.Collections;

namespace AceViral {

    class HouseAdInterface {

    	public static void OpenHouseAdUrl(string urlLink, string fromSlotId)
        {
            #if UNITY_EDITOR
            Debug.Log("HouseAdInterface :: OpenHouseAdUrl. <Link : " + urlLink + " > <Slot : " + fromSlotId + " >");
            #elif UNITY_IPHONE
            AVHiddenInterface.IPhoneInterface.HouseAds.OpenHouseAdUrl(urlLink);
            #elif UNITY_ANDROID

            AVHiddenInterface.AndroidInterface.HouseAds.ShowAppLinkInMarketPlaceWithReferral(urlLink, AppConstants.AppReferralName, fromSlotId);
    		#elif (UNITY_WP8 || UNITY_METRO)
    		AVWindowsPhonePlugin.AVWindowsPhoneUtility.OpenMarketplaceWithStoreCode(urlLink);
            #endif
        }

        public static void OpenMoreGamesPage()
        {
            #if UNITY_EDITOR
            Debug.Log("HouseAdInterface :: OpenMoreGamesPage");

            #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.HouseAds.ShowMoreGamesInMarketPlace();

            #elif UNITY_IPHONE
            AVHiddenInterface.IPhoneInterface.HouseAds.OpenMoreGamesPage();

    		#elif (UNITY_WP8 || UNITY_METRO)
    		AVWindowsPhonePlugin.AVWindowsPhoneUtility.ShowMoreGamesInMarketplace();
            #endif
        }
    }
}
