// #######################################################################
// This interface is inherently slow! Use with caution. Don't just spam it.
// Ask Phil for any info you might need.
// #######################################################################
using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class AVAndroidInterface : MonoBehaviour {


    public class Banners {
        private static AndroidJavaObject bannerManager;
        private static void CheckManagerLoaded()
        {
            //if (bannerManager == null)
            {
                bannerManager = CallMainActivityFunction("getBannerManager");
            }
        }

        public static void SetKey (string key) {
            CheckManagerLoaded();
            bannerManager.Call("setupAdvertsWithKey",new object[]{key});
        }

        public static void LoadNewAdvert () {
            CheckManagerLoaded();
            bannerManager.Call("displayAdvert");
        }

        public static void SetBannerAdConfiguration (int config) {
            CheckManagerLoaded();
            bannerManager.Call("setBannerAdConfiguration",new object[] { config });
        }

        public static void HideAdvert () {
            CheckManagerLoaded();
            bannerManager.Call("hideAdvert");
        }

        public static int GetAdHeight () {
            CheckManagerLoaded();
            return bannerManager.Call<int>("getAdvertHeight");
        }


    }

    public class Interstitials{
        private static AndroidJavaObject interstitialManager;
        private static void CheckManagerLoaded()
        {
            //if (interstitialManager == null)
            {
                interstitialManager = CallMainActivityFunction("getInterstitialManager");
            }
        }

        // Intersititials
        public static void CreateAdMobInterstitialsWithKey (string key) {
            CheckManagerLoaded();
            interstitialManager.Call("createInterstitialWithKey",new object[]{key});
        }

        public static bool IsAdMobInterstitialReady() {
            CheckManagerLoaded();
            return interstitialManager.Call<bool>("isInterstitialReady");
        }

        public static void ShowAbMobIntersitial () {
            CheckManagerLoaded();
            interstitialManager.Call("showIntersitial");
        }
    }

    public class HouseAds {
        private static AndroidJavaObject houseAdManager;
        private static void CheckManagerLoaded()
        {
            //if (houseAdManager == null)
            {
                houseAdManager = CallMainActivityFunction("getHouseAdManager",new object[]{AVAppConstants.CompileForAmazonAppStore});
            }
        }

        public static void ShowMoreGamesInMarketPlace() {
            CheckManagerLoaded();
            houseAdManager.Call("showMoreGamesInMarketPlace");
        }

        public static void ShowAppLinkInMarketPlaceWithReferral(string packageName, string referralAppName, string adSlotName) {
            CheckManagerLoaded();
            houseAdManager.Call("showAppLinkInMarketPlaceWithReferral",new object[]{packageName,referralAppName,adSlotName});
        }

        public static void ShowAppLinkInMarketPlace(string packageName) {
            CheckManagerLoaded();
            houseAdManager.Call("showAppLinkInMarketPlace",new object[]{packageName});
        }

    }


    public class Analytics
    {
        private static AndroidJavaObject analyticsManager;
        private static void CheckManagerLoaded()
        {
            //if (analyticsManager == null)
            {
                analyticsManager = CallMainActivityFunction("getAnalyticsManager");
            }
        }
        public static void SendCurrentScreenName (string screenName)
        {
            CheckManagerLoaded();
            analyticsManager.Call("sendScreenView", new object[]{ screenName });
        }

        public static void SendEvent (string category, string action, string label, long value)
        {
            CheckManagerLoaded();
            analyticsManager.Call("trackEvent", new object[]{ category,action,label,value });
        }

        public static void DispatchTracker() {
            CheckManagerLoaded();
            analyticsManager.Call("dispatchEvents");
        }
    }

    public class GameServices
    {
        private static AndroidJavaObject gamesServicesManager;
        private static void CheckManagerLoaded()
        {
            //if (gamesServicesManager == null)
            {
                gamesServicesManager = CallMainActivityFunction("getGameServicesManager");
            }
        }

        public static void SignIn()
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("signIn");
        }

        public static void SignOut()
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("signOut");
        }

        public static void ShowAchievements()
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("showAchievements");
        }

        public static void ShowLeaderboards()
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("showLeaderboards");
        }

        public static void UnlockAchievement(string achId)
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("unlockAchievement",new object[]{achId});
        }

        public static void UpdateLeaderboard(string achId, float score)
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("updateLeaderboard",new object[]{achId,score});
        }

        public static void ShowLeaderboard(string id)
        {
            CheckManagerLoaded();
            gamesServicesManager.Call("showLeaderboard",new object[]{id});
        }

        public static bool IsSignedIn()
        {
            CheckManagerLoaded();
            return gamesServicesManager.Call<bool>("IsSignedIn");
        }
    }



    public class Facebook
    {
        private static AndroidJavaObject socialManager;
        private static void CheckManagerLoaded()
        {
            //if (socialManager == null)
            {
                socialManager = CallMainActivityFunction("getSocialManager");
            }
        }

        public class User
        {
            public static void Login ()
            {
                CheckManagerLoaded();
                socialManager.Call("login");
            }

            public static bool IsLoggedIn ()
            {
                CheckManagerLoaded();
                return socialManager.Call<bool>("isLoggedIn");  
            }

            public static string GetUserID ()
            {
                CheckManagerLoaded();
                return socialManager.Call<string>("getUserId");
            }

            public static string GetUserName ()
            {
                CheckManagerLoaded();
                return socialManager.Call<string>("getUserName");
            }

            internal static void SendChallenge(string title,string message)
            {
                CheckManagerLoaded();
                socialManager.Call("sendChallenge", new object[]{ title, message });
            }
        }

        public class Feed
        {
            public static void Post (string msg)
            {
                CheckManagerLoaded();
                socialManager.Call("postMessage", new object[]{ msg });
            }
        }

        public class Scores
        {
            public static void Send (int score)
            {
                CheckManagerLoaded();
                socialManager.Call("postScore", new object[]{ score });
            }

            public static void Get ()
            {
                CheckManagerLoaded();
                socialManager.Call("getScores");
            }
        }

        public class Achievements
        {
            public static void Add (string achievementFileName)
            {       
                CheckManagerLoaded();        
                socialManager.Call("addAchievement",new object[]{achievementFileName});
            }

            public static void SendBatch ()
            {        
                CheckManagerLoaded();       
                socialManager.Call("senAchievementBatch");
            }
        }

        public class Permissions
        {
            public static bool HasPublishPermissions ()
            {
                CheckManagerLoaded();
                return socialManager.Call<bool>("hasPublishPermission");
            }

            public static void RequestPublishPermissions ()
            {
                CheckManagerLoaded();
                socialManager.Call("requestPublishPermissions"); 
            }
        }
    }

    public class VideoReward
    {
        private static AndroidJavaObject videoManager;
        private static void CheckManagerLoaded()
        {
            //if (billingManager == null)
            {
                videoManager = CallMainActivityFunction("getVideoRewardManager");
            }
        }

        public static void ShowVideo()
        {
            CheckManagerLoaded();
            videoManager.Call("showVideo");
        }
    }

    public class InAppBilling 
    {
        private static AndroidJavaObject billingManager;
        private static void CheckManagerLoaded()
        {
            //if (billingManager == null)
            {
                billingManager = CallMainActivityFunction("getBillingManager");
            }
        }

        public static void SetEncodedPublicKey(string key) {
            CheckManagerLoaded();
            billingManager.Call("setEncodedPublicKey", new object[]{ key });
        }

        public static void AddSkuKey (string SKU, bool isConsumable) {
            CheckManagerLoaded();
            billingManager.Call("addSkuKey", new object[]{ SKU, isConsumable });
        }

        public static void RequestPurchase (string SKU) {
            CheckManagerLoaded();
            billingManager.Call("requestPurchase", new object[]{ SKU });
        }

        public static void RestorePurchases () {
            CheckManagerLoaded();
            billingManager.Call("restorePurchases");
        }

        public static void RequestPurchasePrices () {
            CheckManagerLoaded();
            billingManager.Call("requestPurchasePrices");
        }
    }

    public static bool isOnline () {
        #if UNITY_EDITOR
        return true;
        #else
        return GetMainActivity().Call<bool>("isOnline");
        #endif
    }
    // Standard func
    public static void RateGame (string name, string package) {

    }
    // Checks for Android 4.3 (some versions of millenial media ads crash entirely on this version of android)
    public static bool IsAndroid4p3 () {
        return GetMainActivity().Call<bool>("isAndroidFourPointThree");
    }





    #region Java Functionality
    #if UNITY_ANDROID

    private static string activityID = ".AVUnityActivity";
    private static string AndroidPackageName = "com.aceviral.activities";

    public static AndroidJavaObject GetMainActivity()
    {
    AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
    return jc.GetStatic<AndroidJavaObject>("CurrentInstance");
    }

    public static AndroidJavaObject CallMainActivityFunction(string functionName)
    {
    AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
    AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("CurrentInstance");

    return jo.Call<AndroidJavaObject>(functionName);
    }

    public static AndroidJavaObject CallMainActivityFunction(string functionName,object[] parameters)
    {
    AndroidJavaClass jc = new AndroidJavaClass(AndroidPackageName + activityID);
    AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("CurrentInstance");

    return jo.Call<AndroidJavaObject>(functionName,parameters);
    }

    #endif
    #endregion
}
