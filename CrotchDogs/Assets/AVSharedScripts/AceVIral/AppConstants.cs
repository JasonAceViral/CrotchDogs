using UnityEngine;
using System.Collections;

namespace AceViral {
    public class AppConstants {

        public const string AppReferralName = "APP NAME";
        public const bool CompileForAmazonAppStore = false;

        /////////////
        /// 
        /// iOS Variables
        /// 
        /// /////////

        public class IOS 
        {
            public const string AppId = "000000000";
            public const string AdMobBannerKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AdMobInterstitialKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AdMobVideoKey = "ca-app-pub-0000000000000000/0000000000";
            public const string HouseAdLink = "http://aceviral.com/a/ad/18";
            public const string GoogleAnalyticsId = "UA-00000000-0"; 
        }


        /////////////
        /// 
        /// Android Variables
        /// 
        /// /////////

        public class Android 
        {
            public const string PackageName = "com.aceviral.crotchdogs";
            public const string PackageId = "com/aceviral/crotchdogs/";
            public const string AdMobBannerKey = "ca-app-pub-0000000000000000/0000000000"; 
            public const string AdMobInterstitialKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AdMobVideoKey = "ca-app-pub-0000000000000000/0000000000";
            public const string HouseAdLink = "http://aceviral.com/a/ad/18";
            public const string GoogleAnalyticsId = "UA-00000000-0";
            public const string GooglePlayId = "00000000000";
        }


        /////////////
        /// 
        /// Amazon Variables
        /// 
        /// /////////

        public class Amazon 
        {
            public const string AdMobBannerKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AdMobInterstitialKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AdMobVideoKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AmazonAdsKey = "00000000";
            public const string HouseAdLink = "http://aceviral.com/a/ad/22";
            public const string GoogleAnalyticsId = "UA-00000000-0"; 
        }


        /////////////
        /// 
        /// Windows Variables
        /// 
        /// /////////

        public class WP8
        {
           
            public const string HouseAdLink = "http://aceviral.com/a/ad/23";
            public const string AdUnitKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AppId = "00000000-0000-0000-0000-000000000000";
            public const bool UseAdMobAds = false;
        }

        public class Metro
        {
            public const string HouseAdLink = "http://aceviral.com/a/ad/24";
            public const string AdUnitKey = "ca-app-pub-0000000000000000/0000000000";
            public const string AppId = "00000000-0000-0000-0000-000000000000";
            public const bool UseAdMobAds = false;
        }


        /////////////
        /// 
        /// Facebook Variables
        /// 
        /// /////////

        public class Facebook
        {
            public const string AppId = "000000000000000";
            public const string PageId = "000000000000000";
            public const int PreferredPicSize = 100;
        }

        /////////////
        /// 
        /// Leaderboards & Achievements
        /// 
        /// /////////

        public static GSAchievement[] Achievements = new GSAchievement[] {
            new GSAchievement(){
                iOSId = "not set", AndroidId = "not set", AmazonId = "not set"
            }
        };

        public static GSLeaderboard[] Leaderboards = new GSLeaderboard[] {
            new GSLeaderboard(){
                iOSId = "not set", AndroidId = "not set", AmazonId = "not set"
            }
        };

        /////////////
        /// 
        /// IAP Variables
        /// 
        /// /////////

        public class IAP
        {
            public const string AndroidIapEncodedKey = "NOT SET";

            public static InAppID[] Purchases = new InAppID[] {
    //            new InAppID(){
    //                Name = "In App Name", AndroidID = "inAppId", iOSID = "com.aceviral.game.inappid",
    //                AmazonID = "inAppId", WindowsPhoneID = "inApPId", Display = "Display Name", isConsumable = false,
    //                OnBought = new InAppBoughtFunction(delegate { })
    //            },
            };
        }
    }
}
