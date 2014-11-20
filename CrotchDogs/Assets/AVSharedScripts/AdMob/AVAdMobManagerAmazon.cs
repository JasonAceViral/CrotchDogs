using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
	public class AVAdMobManagerAmazon : AVAdMobManager
	{
		public override void OnStart ()
		{
            AndroidInterface.Banners.SetKey (AppConstants.Amazon.AdMobBannerKey, AppConstants.Amazon.AmazonAdsKey);	
		}

	    public override void ShowBanner ()
	    {
	        if (!IsAdvertShowing ()) {
                AndroidInterface.Banners.ShowAdvert ();
	            AndroidInterface.Banners.SetBannerAdConfiguration((int)m_BannerConfig);
	            m_ShowingAdvert = true;
	        }

            if (RefreshAdOnShow)
                LoadNewBanner();
	    }

        public override void LoadNewBanner()
        {
            AndroidInterface.Banners.LoadNewAdvert();
        }

	    public override void HideBanner ()
		{
			if (IsAdvertShowing ()) {
				AndroidInterface.Banners.HideAdvert ();
				m_ShowingAdvert = false;
			}
		}

		public override int GetAdvertHeight ()
		{
			return AndroidInterface.Banners.GetAdHeight ();
		}
	}
}
