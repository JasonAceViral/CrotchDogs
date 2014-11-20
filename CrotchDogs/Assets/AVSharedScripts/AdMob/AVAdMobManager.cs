using UnityEngine;

public enum AVAdPositionConfiguration
{
    eAdConfigTop    =   1 << 0,   ///The advert should be at the top of the screen
    eAdConfigBottom =   1 << 1,   ///The advert should be at the bottom of the screen

    eAdConfigLeft   =   1 << 2,   ///The advert should be on the left of the screen
    eAdConfigRight  =   1 << 3,   ///The advert should be on the right of the screen
    eAdConfigCenter  =   1 << 4,  ///The advert should be in the center of the screen (horizontally, vertically is not permitted)

    eAdConfigBottomLeft = eAdConfigBottom | eAdConfigLeft,      ///The advert should be in the bottom left of the screen
    eAdConfigBottomRight = eAdConfigBottom | eAdConfigRight,    ///The advert should be in the bottom right of the screen
    eAdConfigBottomCenter = eAdConfigBottom | eAdConfigCenter,  ///The advert should be in the bottom center of the screen

    eAdConfigTopLeft = eAdConfigTop | eAdConfigLeft,            ///The advert should be in the top left of the screen
    eAdConfigTopRight = eAdConfigTop | eAdConfigRight,          ///The advert should be in the top right of the screen
    eAdConfigTopCenter = eAdConfigTop | eAdConfigCenter,        ///The advert should be in the top center of the screen
};

public abstract class AVAdMobManager : MonoBehaviour
{
    public static AVAdMobManager m_Instance;

    public static AVAdMobManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
#if UNITY_EDITOR
                m_Instance = new GameObject().AddComponent<AVAdMobManagerEditor>();
#elif UNITY_ANDROID
                m_Instance = new GameObject ().AddComponent<AVAdMobManagerAndroid> ();
#elif UNITY_IPHONE
					m_Instance = new GameObject().AddComponent<AVAdMobManagerIOS>();
#elif UNITY_STANDALONE_OSX
					m_Instance = new GameObject().AddComponent<AVAdMobManagerMac>();
#elif UNITY_WEBPLAYER
					m_Instance = new GameObject().AddComponent<AVAdMobManagerWeb>();
#elif UNITY_WP8
					m_Instance = new GameObject().AddComponent<AVAdMobManagerWP>();
#elif UNITY_METRO
				m_Instance = new GameObject().AddComponent<AVAdMobManagerWin>();
#endif
                m_Instance.OnStart();
                m_Instance.gameObject.name = "AVAdMobManager";
            }
            return m_Instance;
        }
    }

    public abstract void OnStart();

    public abstract void OnStartWithKey(string key);

    public virtual void ShowBannerWithConfiguration (AVAdPositionConfiguration config)
    {
        SetBannerConfiguration(config);
        ShowBanner();
    }

    protected AVAdPositionConfiguration m_BannerConfig = AVAdPositionConfiguration.eAdConfigBottomCenter;

    public virtual void SetBannerConfiguration (AVAdPositionConfiguration config)
    {
        m_BannerConfig = config;
    }

    public abstract void ShowBanner ();

    public abstract void HideBanner ();

    public abstract int GetAdvertHeight();

    protected bool m_ShowingAdvert = false;

    public bool IsAdvertShowing()
    {
        return m_ShowingAdvert;
    }

    public abstract void CreateAdMobInterstitials();

    public abstract bool IsAdMobInterstitialReady();

    public abstract void ShowAbMobIntersitial();


    // Callbacks from AdMob SDK
//
//    protected virtual void InterstitialAdIsAvailable(string key)
//    {
//        Debug.Log("InterstitialAdIsAvailable");
//    }
//
//    protected virtual void InterstitialAdDismissed(string key)
//    {
//        Debug.Log("InterstitialAdDismissed");
//    }
//
//    protected virtual void FailedToReceiveInterstitialAd(string error)
//    {
//
//        Debug.Log("FailedToReceiveInterstitialAd");
//    }
}

