using System.Collections;
using UnityEngine;

public class AVAdMobManagerAndroid : AVAdMobManager
{
	//private int m_AdvertHeight = 80;
	void Start ()
	{
		//StartCoroutine (PeriodicallyGetAdvertHeight ());
	}

	public override void OnStart ()
	{
        AVAndroidInterface.Banners.SetKey (AVAppConstants.AndroidAdMobKey);
	}

	public override void OnStartWithKey(string key)
	{
		AVAndroidInterface.Banners.SetKey (key);	
	}

    public override void SetBannerConfiguration (AVAdPositionConfiguration config)
    {
        m_BannerConfig = config;
        AVAndroidInterface.Banners.SetBannerAdConfiguration((int)config);
    }

    public override void ShowBanner ()
    {
        if (!IsAdvertShowing ()) {
            AVAndroidInterface.Banners.LoadNewAdvert ();
            m_ShowingAdvert = true;
        }
    }

    public override void HideBanner ()
	{
		if (IsAdvertShowing ()) {
			AVAndroidInterface.Banners.HideAdvert ();
			//m_AdvertHeight = 0;
			m_ShowingAdvert = false;
		}
	}

	public override int GetAdvertHeight ()
	{
		return AVAndroidInterface.Banners.GetAdHeight ();
	}

	// Interstitials

	public override void CreateAdMobInterstitials ()
	{
        AVAndroidInterface.Interstitials.CreateAdMobInterstitialsWithKey (AVAppConstants.AndroidAdMobInterstitialKey);
	}

	public override bool IsAdMobInterstitialReady ()
	{
        return AVAndroidInterface.Interstitials.IsAdMobInterstitialReady ();
	}

	public override void ShowAbMobIntersitial ()
	{
        AVAndroidInterface.Interstitials.ShowAbMobIntersitial ();
	}
}