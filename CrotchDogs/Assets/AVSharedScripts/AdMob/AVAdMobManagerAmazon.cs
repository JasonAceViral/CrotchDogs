using UnityEngine;
using System.Collections;

public class AVAdMobManagerAmazon : AVAdMobManager
{
	public override void OnStart ()
	{
        AVAndroidInterface.Banners.SetKey (AVAppConstants.AmazonAdMobKey);	
	}

	public override void OnStartWithKey(string key)
	{
		AVAndroidInterface.Banners.SetKey (key);	
	}

    public override void ShowBanner ()
    {
        if (!IsAdvertShowing ()) {
            AVAndroidInterface.Banners.LoadNewAdvert ();
            AVAndroidInterface.Banners.SetBannerAdConfiguration((int)m_BannerConfig);
            m_ShowingAdvert = true;
        }
    }

    public override void HideBanner ()
	{
		if (IsAdvertShowing ()) {
			AVAndroidInterface.Banners.HideAdvert ();
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
        AVAndroidInterface.Interstitials.CreateAdMobInterstitialsWithKey (AVAppConstants.AmazonAdMobInterstitialKey);
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
