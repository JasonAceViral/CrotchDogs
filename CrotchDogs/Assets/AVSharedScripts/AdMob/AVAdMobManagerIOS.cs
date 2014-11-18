using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class AVAdMobManagerIOS : AVAdMobManager {

	#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _avSetAdMobKey(string key,int network);
    [DllImport("__Internal")]
    private static extern void _avShowAdmobAd(int config);
    [DllImport("__Internal")]
    private static extern void _avRemoveAdmobAd();

    // Interstitial dll imports
    [DllImport("__Internal")]
    private static extern void _avSetAdMobInterstitialKey(string key);
    [DllImport("__Internal")]
    private static extern bool _avIsAdmobInterstitialAdReady();
    [DllImport("__Internal")]
    private static extern void _avShowAdmobInterstitialAd();
	#endif

    private bool m_Initialized = false;
    private int m_AdvertHeight = 0;

    public override void OnStart()
    {
        InitializeAds();
    }

	public override void OnStartWithKey(string key)
	{
		#if UNITY_IPHONE
        _avSetAdMobKey(key,AVAppConstants.iOSAddNetwork);
		#endif
		m_Initialized = true;
	}

    void InitializeAds ()
    {
		#if UNITY_IPHONE
        _avSetAdMobKey(AVAppConstants.iOSAdMobKey,AVAppConstants.iOSAddNetwork);
		#endif
        m_Initialized = true;
    }

    public override void ShowBannerWithConfiguration (AVAdPositionConfiguration config)
    {
        SetBannerConfiguration(config);
        ShowBanner();
    }

    public override void ShowBanner ()
    {
        if (!IsAdvertShowing()) {
            if (!m_Initialized) {
                InitializeAds();
            }
			#if UNITY_IPHONE
            _avShowAdmobAd((int)m_BannerConfig);
			#endif
            m_ShowingAdvert = true;
        }
    }

    public override void HideBanner() {
        if (IsAdvertShowing()) {
            if (!m_Initialized) {
                InitializeAds();
            }
		#if UNITY_IPHONE
            _avRemoveAdmobAd();
			#endif
            m_ShowingAdvert = false;
        }
    }

    public override int GetAdvertHeight() {
		#if UNITY_IPHONE
        return 100;//m_AdvertHeight;
		#else
		return 50;
		#endif
    }

	// Interstitials

	public override void CreateAdMobInterstitials ()
	{
		#if UNITY_IPHONE
        _avSetAdMobInterstitialKey(AVAppConstants.iOSAdMobInterstitialKey);
		#endif
	}

	public override bool IsAdMobInterstitialReady ()
	{
		#if UNITY_IPHONE
        return _avIsAdmobInterstitialAdReady();
		#else
		return false;
		#endif
	}

	public override void ShowAbMobIntersitial ()
	{
		#if UNITY_IPHONE
        _avShowAdmobInterstitialAd();
		#endif
	}
}


