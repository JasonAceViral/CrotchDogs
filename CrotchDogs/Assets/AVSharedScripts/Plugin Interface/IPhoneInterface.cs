using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace AVHiddenInterface {
    class IPhoneInterface {

        #if UNITY_IPHONE
        [DllImport("__Internal")] private static extern bool _avDeviceIsIPad( );

        [DllImport("__Internal")] private static extern void _avCreateAdMobBannerWithKey(string key);
        [DllImport("__Internal")] private static extern void _avShowAdMobBanner(int config);
        [DllImport("__Internal")] private static extern void _avRefreshAdMobBanner();
        [DllImport("__Internal")] private static extern void _avSetBannerConfig(int config);
        [DllImport("__Internal")] private static extern void _avRemoveAdMobBanner();

        [DllImport ("__Internal")] private static extern void _avCreateInterstitialWithKey (string adKey);
        [DllImport ("__Internal")] private static extern void _avShowInterstitial ();
        [DllImport ("__Internal")] private static extern void _avLoadInterstitial ();
        [DllImport ("__Internal")] private static extern bool _avIsInterstitialReady ();
        [DllImport ("__Internal")] private static extern void _avCancelAutoShowInterstitial ();

        [DllImport ("__Internal")] private static extern void _avCreateVideoInterstitialWithKey (string adKey);
        [DllImport ("__Internal")] private static extern void _avShowVideoInterstitial ();
        [DllImport ("__Internal")] private static extern void _avLoadVideoInterstitial ();
        [DllImport ("__Internal")] private static extern bool _avIsVideoInterstitialReady ();
        [DllImport ("__Internal")] private static extern void _avCancelAutoShowVideoInterstitial ();

        [DllImport ("__Internal")] private static extern bool _avIsCloudAvailable ();
        [DllImport ("__Internal")] private static extern string _avLoadAllDataFromCloud ();
        [DllImport ("__Internal")] private static extern string _avLoadDataFromCloud (string key);
        [DllImport ("__Internal")] private static extern void _avSaveDictionaryDataToCloud (string data);
        [DllImport ("__Internal")] private static extern void _avSaveDataToCloud (string key, string data);
        [DllImport ("__Internal")] private static extern void _avSynchronizeCloud ();

        [DllImport ("__Internal")] private static extern bool _avGameCenterIsAvailable ( );
        [DllImport ("__Internal")] private static extern void _avGameCenterAuthenticate ( );
        [DllImport ("__Internal")] private static extern bool _avGameCenterIsSignedIn ( );
        [DllImport ("__Internal")] private static extern void _avGameCenterSignOut ( );
        [DllImport ("__Internal")] private static extern void _avGameCenterShowAchievements ( );
        [DllImport ("__Internal")] private static extern void _avGameCenterShowLeaderboards ( );
        [DllImport ("__Internal")] private static extern void _avGameCenterShowLeaderboard ( string leaderboardId );
        [DllImport ("__Internal")] private static extern void _avGameCenterPostScore ( float score, string leaderboardId );
        [DllImport ("__Internal")] private static extern void _avGameCenterPostAchievement ( float progress, string achievement );

        [DllImport ("__Internal")] private static extern void _avSetUpGoogleAnalyticsWithKey( string key );
        [DllImport ("__Internal")] private static extern void _avTrackPageView( string pageName );
        [DllImport ("__Internal")] private static extern void _avTrackEvent( string category, string action, string label, int value );
        [DllImport ("__Internal")] private static extern void _avDispathAnalytics( );

        [DllImport ("__Internal")] private static extern void _avInAppPurchaseManagerRequestProduct( string iapID );
        [DllImport ("__Internal")] private static extern void _avInAppPurchaseManagerRestorePurchases( );
        [DllImport ("__Internal")] private static extern int _avGetSuccessStatus( );
        [DllImport ("__Internal")] private static extern int _avGetRestoreSuccessStatus( );
        [DllImport ("__Internal")] private static extern string[] _avGetRestoredPurchases( );   
        [DllImport ("__Internal")] private static extern string[] _avRequestAppInformation( );
        [DllImport ("__Internal")] private static extern string[] _avAddIdentifierForInformationRequest( string productID, int index );

        [DllImport ("__Internal")] private static extern void _avOpenHouseAdLink( string url );
        [DllImport ("__Internal")] private static extern void _avOpenMoreGamesPage( );
        #endif

        public class Utility 
        {
            public static bool iOSDeviceIsIpad()
            {
                #if !UNITY_EDITOR && UNITY_IPHONE
                return _avDeviceIsIPad();
                #endif
                return false;
            }
        }

        public class Banners
        {
            public static void CreateBannerWithKey(string key) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avCreateAdMobBannerWithKey(key);
                #endif
            }

            public static void ShowBannerWithConfig(int config) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avShowAdMobBanner(config);
                #endif
            }

            public static void HideBanner() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avRemoveAdMobBanner();
                #endif
            }

            public static void RefreshBanner() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avRefreshAdMobBanner();
                #endif
            }

            public static void SetBannerConfig(int config) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avSetBannerConfig(config);
                #endif
            }
        }

        public class Interstitials
        {
            public static void CreateInterstitialWithKey(string key) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avCreateInterstitialWithKey(key);
                #endif
            }

            public static void ShowInterstitial() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avShowInterstitial();
                #endif
            }

            public static void LoadInterstitialIfNotAlready() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avLoadInterstitial();
                #endif
            }

            public static bool IsInterstitialReady() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avIsInterstitialReady();
                #else
                return false;
                #endif
            }

            public static void CancelAutoShowInterstitial() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avCancelAutoShowInterstitial();
                #endif
            }

            public static void CreateVideoWithKey(string key) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avCreateVideoInterstitialWithKey(key);
                #endif
            }

            public static void ShowVideo() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avShowVideoInterstitial();
                #endif
            }

            public static void LoadVideoIfNotAlready() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avLoadVideoInterstitial();
                #endif
            }

            public static bool IsVideoReady() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avIsVideoInterstitialReady();
                #else
                return false;
                #endif
            }

            public static void CancelAutoShowVideo() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avCancelAutoShowVideoInterstitial();
                #endif
            }
        }

        public class Cloud
        {
            public static bool IsAvailable() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avIsCloudAvailable ();
                #else
                return false;
                #endif
            }

            public static string LoadAllData() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avLoadAllDataFromCloud ();
                #else
                return "";
                #endif
            }

            public static string LoadDataForKey(string key) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avLoadDataFromCloud(key);
                #else
                return "";
                #endif
            }

            public static void SaveDictionaryData(string data) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avSaveDictionaryDataToCloud(data);
                #endif
            }

            public static void SaveDataForKey(string key, string data) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avSaveDataToCloud(key, data);
                #endif
            }

            public static void Synchronize() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avSynchronizeCloud();
                #endif
            }
        }

        public class GameCenter
        {
            public static bool IsAvailable() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avGameCenterIsAvailable ( );
                #else 
                return false;
                #endif
            }

            public static void Authenticate() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterAuthenticate ( );
                #endif
            }

            public static bool IsSignedIn() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avGameCenterIsSignedIn ( );
                #else 
                return false;
                #endif
            }

            public static void SignOut() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterSignOut ( );
                #endif
            }

            public static void ShowAchievements() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterShowAchievements ( );
                #endif
            }

            public static void ShowLeaderboards() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterShowLeaderboards ( );
                #endif
            }

            public static void ShowLeaderboard(string leaderboardId) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterShowLeaderboard ( leaderboardId );
                #endif
            }

            public static void PostScore(float score, string leaderboardId) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterPostScore ( score, leaderboardId );
                #endif
            }

            public static void PostAchievement(float progress, string achievement) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGameCenterPostAchievement ( progress, achievement );
                #endif
            }
        }

        public class GoogleAnalytics
        {
            public static void SetupWithKey(string key) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avSetUpGoogleAnalyticsWithKey( key );
                #endif
            }

            public static void TrackPageView(string pageName) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avTrackPageView( pageName );
                #endif
            }

            public static void TrackEvent(string category, string action, string label = "", int value = 0) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avTrackEvent( category, action, label, value );
                #endif
            }

            public static void Dispatch() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avDispathAnalytics( );
                #endif
            }
        }

        public class IAP
        {
            public static void RequestProduct(string iapID) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avInAppPurchaseManagerRequestProduct( iapID );
                #endif
            }

            public static void RestorePurchases() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avInAppPurchaseManagerRestorePurchases( );
                #endif
            }

            public static int GetSuccessSatus() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avGetSuccessStatus( );
                #else
                return 0;
                #endif
            }

            public static int GetRestoreSuccessStatus() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                return _avGetRestoreSuccessStatus( );
                #else
                return 0;
                #endif
            }

            public static void GetRestoredPurchases() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avGetRestoredPurchases( );  
                #endif
            }

            public static void RequestIapInformation() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avRequestAppInformation( );
                #endif
            }

            public static void AddIdentifierForInformationRequest( string productID, int index ) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avAddIdentifierForInformationRequest( productID, index );
                #endif
            }
        }

        public class HouseAds
        {
            public static void OpenHouseAdUrl(string url) {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avOpenHouseAdLink(url);
                #endif
            }

            public static void OpenMoreGamesPage() {
                #if UNITY_IPHONE && !UNITY_EDITOR
                _avOpenMoreGamesPage();
                #endif
            }
        }
    }
}


