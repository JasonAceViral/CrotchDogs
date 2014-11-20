// #######################################################################
// This interface is inherently slow! Use with caution.	Don't just spam it.
// Ask Phil for any info you might need.
// #######################################################################

using UnityEngine;
using System.Collections;
using System;
using System.Text;


namespace AVHiddenInterface {
    public class AndroidInterface : MonoBehaviour {

    	public class Banners {
            #if UNITY_ANDROID
            private static AndroidJavaObject bannerManager;
            #endif
            private static void CheckManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (bannerManager == null)
                {
                    bannerManager = CallMainActivityFunction("getBannerManager");
                }
                #endif
            }

    		public static void SetKey (string key) {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("setupAdvertsWithKey",new object[]{key});
                #endif
    		}

            public static void SetKey (string adMobKey, string amazonKey) {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("setupAdvertsWithKey",new object[]{adMobKey, amazonKey});
                #endif
            }

            public static void ShowAdvert () {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("displayAdvert");
                #endif
    		}

            public static void LoadNewAdvert () {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("loadNewBannerAd");
                #endif
            }

            public static void SetBannerAdConfiguration (int config) {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("setBannerAdConfiguration",new object[] { config });
                #endif
            }

    		public static void HideAdvert () {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                bannerManager.Call("hideAdvert");
                #endif
    		}

    		public static int GetAdHeight () {
                CheckManagerLoaded();
                #if UNITY_ANDROID
                return bannerManager.Call<int>("getAdvertHeight");
                #else
                return 0;
                #endif
    		}
    	}

        public class Interstitials{
            #if UNITY_ANDROID
            private static AndroidJavaObject interstitialManager;
            private static AndroidJavaObject videoManager;
            #endif
            private static void CheckInterstitialManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (interstitialManager == null)
                {
                    interstitialManager = CallMainActivityFunction("getInterstitialManager");
                }
                #endif
            }

            private static void CheckVideoManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (videoManager == null)
                {
                    videoManager = CallMainActivityFunction("getVideoManager");
                }
                #endif
            }

            // Interstitials
            public static void CreateInterstitialWithKey (string key) {
                #if UNITY_ANDROID
                CheckInterstitialManagerLoaded();
                interstitialManager.Call("createInterstitialWithKey",new object[]{key});
                #endif
            }

            public static bool IsInterstitialReady() {
                #if UNITY_ANDROID
                CheckInterstitialManagerLoaded();
                return interstitialManager.Call<bool>("isInterstitialReady");
                #else
                return false;
                #endif
            }

            public static void ShowInterstitial () {
                #if UNITY_ANDROID
                CheckInterstitialManagerLoaded();
                interstitialManager.Call("showInterstitial");
                #endif
            }

            public static void LoadInterstitialIfNotAlready() {
                #if UNITY_ANDROID
                CheckInterstitialManagerLoaded();
                interstitialManager.Call("loadInterstitialIfNotAlready");
                #endif
            }

            public static void CancelAutoShowInterstitial() {
                #if UNITY_ANDROID
                CheckInterstitialManagerLoaded();
                interstitialManager.Call("cancelAutoShowInterstitial");
                #endif
            }

            // Video Interstitials
            public static void CreateVideoInterstitialWithKey (string key) {
                #if UNITY_ANDROID
                CheckVideoManagerLoaded();
                videoManager.Call("createInterstitialWithKey",new object[]{key});
                #endif
            }

            public static bool IsVideoInterstitialReady() {
                #if UNITY_ANDROID
                CheckVideoManagerLoaded();
                return videoManager.Call<bool>("isInterstitialReady");
                #else
                return false;
                #endif
            }

            public static void ShowVideoInterstitial () {
                #if UNITY_ANDROID
                CheckVideoManagerLoaded();
                videoManager.Call("showInterstitial");
                #endif
            }

            public static void LoadVideoInterstitialIfNotAlready() {
                #if UNITY_ANDROID
                CheckVideoManagerLoaded();
                videoManager.Call("loadInterstitialIfNotAlready");
                #endif
            }

            public static void CancelAutoShowVideoInterstitial() {
                #if UNITY_ANDROID
                CheckVideoManagerLoaded();
                videoManager.Call("cancelAutoShowInterstitial");
                #endif
            }
        }

        public class HouseAds {
            #if UNITY_ANDROID
            private static AndroidJavaObject houseAdManager;
            #endif
            private static void CheckManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (houseAdManager == null)
                {
                    houseAdManager = CallMainActivityFunction("getHouseAdManager",new object[]{AceViral.AppConstants.CompileForAmazonAppStore});
                }
                #endif
            }

            public static void ShowMoreGamesInMarketPlace() {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                houseAdManager.Call("showMoreGamesInMarketPlace");
                #endif
            }

            public static void ShowAppLinkInMarketPlaceWithReferral(string packageName, string referralAppName, string adSlotName) {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                houseAdManager.Call("showAppLinkInMarketPlaceWithReferral",new object[]{packageName,referralAppName,adSlotName});
                #endif
            }

            public static void ShowAppLinkInMarketPlace(string packageName) {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                houseAdManager.Call("showAppLinkInMarketPlace",new object[]{packageName});
                #endif
            }

        }


        public class Analytics
        {
            #if UNITY_ANDROID
            private static AndroidJavaObject analyticsManager;
            #endif
            private static void CheckManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (analyticsManager == null)
                {
                    analyticsManager = CallMainActivityFunction("getAnalyticsManager");
                }
                #endif
            }
            public static void SendCurrentScreenName (string screenName)
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                analyticsManager.Call("sendScreenView", new object[]{ screenName });
                #endif
            }

            public static void SendEvent (string category, string action, string label, long value)
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                analyticsManager.Call("trackEvent", new object[]{ category,action,label,value });
                #endif
            }

            public static void DispatchTracker() {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                analyticsManager.Call("dispatchEvents");
                #endif
            }
        }

        public class GameServices
        {
            #if UNITY_ANDROID
            private static AndroidJavaObject gamesServicesManager;
            #endif
            private static void CheckManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (gamesServicesManager == null)
                {
                gamesServicesManager = CallMainActivityFunction("getGameServicesManager");
                }
                #endif
            }

            public static bool IsAvailable()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                return gamesServicesManager.Call<bool>("isAvailable");
                #else 
                return false;
                #endif
            }

            public static void SignIn()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("signIn");
                #endif
            }

            public static void SignOut()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("signOut");
                #endif
            }

            public static void ShowAchievements()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("showAchievements");
                #endif
            }

            public static void ShowLeaderboards()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("showLeaderboards");
                #endif
            }

            public static void UnlockAchievement(string achId, float progress, int steps)
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("updateAchievement",new object[]{achId, progress, steps});
                #endif
            }

            public static void UpdateLeaderboard(string achId, float score)
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("updateLeaderboard",new object[]{achId,score});
                #endif
            }

            public static void ShowLeaderboard(string id)
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                gamesServicesManager.Call("showLeaderboard",new object[]{id});
                #endif
            }

            public static bool IsSignedIn()
            {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                return gamesServicesManager.Call<bool>("isSignedIn");
                #else 
                return false;
                #endif
            }

            public class Cloud
            {
                public static void FetchData()
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    gamesServicesManager.Call("cloudFetchData");
                    #endif
                }

                public static bool IsAvailable()
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    return gamesServicesManager.Call<bool>("cloudIsAvailable");
                    #else 
                    return false;
                    #endif
                }

                public static string LoadAllData()
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    return gamesServicesManager.Call<string>("cloudLoadAllData");
                    #else 
                    return "";
                    #endif
                }

                public static string LoadKey(string key)
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    return gamesServicesManager.Call<string>("cloudLoadKey");
                    #else 
                    return "";
                    #endif
                }

                public static void SaveDictionaryData(string dictData)
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    gamesServicesManager.Call("cloudSaveDictionaryData",new object[]{dictData});
                    #endif
                }

                public static void SaveKey(string key, string data)
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    gamesServicesManager.Call("cloudSaveKey",new object[]{key, data});
                    #endif
                }

                public static void Synchronize()
                {
                    #if UNITY_ANDROID
                    CheckManagerLoaded();
                    gamesServicesManager.Call("cloudSynchronize");
                    #endif
                }
            }
        } 

    	public class InAppBilling 
        {
            #if UNITY_ANDROID
            private static AndroidJavaObject billingManager;
            #endif
            private static void CheckManagerLoaded()
            {
                #if UNITY_ANDROID
                //if (billingManager == null)
                {
                    billingManager = CallMainActivityFunction("getBillingManager",new object[]{AceViral.AppConstants.CompileForAmazonAppStore});
                }
                #endif
            }

            public static void SetEncodedPublicKey(string key) {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                billingManager.Call("setEncodedPublicKey", new object[]{ key });
                #endif
            }

            public static void AddSkuKey (string SKU, bool isConsumable) {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                billingManager.Call("addSkuKey", new object[]{ SKU, isConsumable });
                #endif
            }

            public static void RequestPurchase (string SKU) {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                billingManager.Call("requestPurchase", new object[]{ SKU });
                #endif
    		}

            public static void RestorePurchases () {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                billingManager.Call("restorePurchases");
                #endif
    		}

            public static void RequestPurchasePrices () {
                #if UNITY_ANDROID
                CheckManagerLoaded();
                billingManager.Call("requestPurchasePrices");
                #endif
    		}
    	}

    	public static bool isOnline () {
            #if UNITY_EDITOR || !UNITY_ANDROID
            return true;
            #else
            return GetMainActivity().Call<bool>("isOnline");
            #endif
    	}
    	// Standard func
    	public static void RateGame (string name, string package) {

    	}

        #if UNITY_ANDROID

    	// Checks for Android 4.3 (some versions of millenial media ads crash entirely on this version of android)
    	public static bool IsAndroid4p3 () {
            return GetMainActivity().Call<bool>("isAndroidFourPointThree");
    	}


    	#region Java Functionality


        private static string activityID = ".AVUnityActivity";
        private static string AndroidPackageName = "com.aceviral.activities";

        public static AndroidJavaObject GetMainActivity()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
            return jc.GetStatic<AndroidJavaObject>("CurrentInstance");  
    		#else 
    		return null;
    		#endif
    	}

        public static AndroidJavaObject CallMainActivityFunction(string functionName)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("CurrentInstance");

            return jo.Call<AndroidJavaObject>(functionName);
    		#else 
    		return null;
    		#endif
        }

        public static AndroidJavaObject CallMainActivityFunction(string functionName,object[] parameters)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("CurrentInstance");

            return jo.Call<AndroidJavaObject>(functionName,parameters);
    		#else 
    		return null;
    		#endif
        }
    	#endregion

        #endif
    }
}

