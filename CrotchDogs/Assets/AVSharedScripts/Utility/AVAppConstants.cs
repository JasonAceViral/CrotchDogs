using UnityEngine;
using System.Collections;

public class AVAppConstants {
    public const int ADMOB = 0;
    public const int MO_PUB = 1;

    public const string facebookAppId = "602031736559944";

    public const string appReferralName = "crotchdogs";

    public const string iOSAppId = "853121342";
	public const string iOSAdMobKey = "ca-app-pub-8282354922336565/5380883931";
	public const string iOSAdMobInterstitialKey = "ca-app-pub-8282354922336565/6857617131";
    public const string iOSHouseAdLink = "http://aceviral.com/a/ad/853121342";
	public const string iOSGoogleAnalyticsId = "UA-50478536-1"; 
    public const string iOSGooglePlusClientId = "78984614272-fv4o3gss234jjq5qqv7slp514e8pfs38.apps.googleusercontent.com";
    public const string iOSmoPubBanner = "0d5d978621d7400aa26b8da2e566b667";
    public const string iOSmoPubBannerIpad = "4d09a40f94aa49b3b7174adc6ceebb1c";
    public const string iOSmoPubInterstitial = "f204f3111eef4c1696ddc822862ebadc";
    public const string iOSmoPubInterstitialIpad = "1800e7a7f4d446129c9a6f6129b7971a";
    public const int iOSAddNetwork = MO_PUB;
    // Google Play App ID on iOS is set in info.plist with key GPGApplicationID

    // Double check the keys in Android Manifest (Activity and Package Name)
    public const string AndroidPackageName = "com.aceviral.crotchdogs";
    public static string AndroidGooglePlayId = "78984614272";
    public const string AndroidPackageId = "com/aceviral/crotchdogs/";
    public const string AndroidAdMobKey = "ca-app-pub-8282354922336565/8473951131";
	public const string AndroidAdMobInterstitialKey = "ca-app-pub-8282354922336565/9950684332";
    public const string AndroidHouseAdLink = "http://aceviral.com/a/ad/18";
    public const string AndroidGoogleAnalyticsId = "UA-50477942-2"; // This is actually read from Plugins/Android/res/values/analytics.xml
	
    public const string AndroidAdColonyAppID = "UA-50477942-2"; 
    public const string AndroidAdColonyZoneID = "UA-50477942-2"; 

	public const string AmazonAdMobKey = "ca-app-pub-8282354922336565/3764549933";
	public const string AmazonAdMobInterstitialKey = "ca-app-pub-8282354922336565/5241283133";
    public const string AmazonHouseAdLink = "http://aceviral.com/a/ad/22";
	public const string AmazonGoogleAnalyticsId = "UA-50468568-2"; // This is actually read from Plugins/Android/res/values/analytics.xml

	public const string WindowsHouseAdLink = "http://aceviral.com/a/ad/23";
	public const string WP8HouseAdLink = "http://aceviral.com/a/ad/24";

    public const bool CompileForAmazonAppStore = false;


    // IAP's
    public const string AndroidIapEncodedKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsxaIZZtPuyi1RVW5XCCvMsK3AP8Yf7mj5kvfw/abBtSH4mnOt3rjuuspf8YQfiCme/J39XqZLviJ+OMjVf/xXNfC1xFHS50JQvZK+XvBCrPNJ4/x50Wm3Fr0LuJ44q4FcQNwojJP4nm+TOtZvf7lY5Bwtb9eeuN9606JeFMzHz9V7Jja3sW0e0MQWydyC+lUoLOe5SDCC1hH78lz/dlK4AWGuv987AV/G47mrsBjOQNY5+enMVNjB5dYIZPdu2QQnXbloAJELdWEKGADZkiUFMVMSt0m0H4wgF4smUkAdqNeqMY3+yOdKpfwgOkEzEbou42W4+DNwaq5yob5y6P1RQIDAQAB";

    public static AVInAppID[] InAppPurchases = new AVInAppID[] {
		//new AVInAppID(){
		//    Name = GameDataCache.BikeReferenceNames[1], AndroidID = "armyriderbike1", iOSID = "com.aceviral.armyrider.armyriderbike1",
		//    AmazonID = "armyriderbike1", Display = GameDataCache.BikeReferenceNames[1] + " Bike", isConsumable = false,
		//    OnBought = new InAppBoughtFunction(delegate { GameDataCache.Instance.UnlockBike(1); })
		//},
		//new AVInAppID(){
		//    Name = GameDataCache.BikeReferenceNames[2], AndroidID = "armyriderbike2", iOSID = "com.aceviral.armyrider.armyriderbike2",
		//    AmazonID = "armyriderbike2", Display = GameDataCache.BikeReferenceNames[2] + " Bike", isConsumable = false,
		//    OnBought = new InAppBoughtFunction(delegate { GameDataCache.Instance.UnlockBike(2); })
		//},
		//new AVInAppID(){
		//    Name = GameDataCache.BikeReferenceNames[3], AndroidID = "armyriderbike3", iOSID = "com.aceviral.armyrider.armyriderbike3",
		//    AmazonID = "armyriderbike3", Display = GameDataCache.BikeReferenceNames[3] + " Bike", isConsumable = false,
		//    OnBought = new InAppBoughtFunction(delegate { GameDataCache.Instance.UnlockBike(3); })
		//},
		//new AVInAppID(){
		//    Name = GameDataCache.BikeReferenceNames[4], AndroidID = "armyriderbike4", iOSID = "com.aceviral.armyrider.armyriderbike4",
		//    AmazonID = "armyriderbike4", Display = GameDataCache.BikeReferenceNames[4] + " Bike", isConsumable = false,
		//    OnBought = new InAppBoughtFunction(delegate { GameDataCache.Instance.UnlockBike(4); })
		//}
    };
}
