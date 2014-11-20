using System.Collections;
using UnityEngine;
using AceViral;

namespace AVHiddenInterface {
	public class AVAdMobManagerAndroid : AVAdMobManager
	{
		//private int m_AdvertHeight = 80;
		void Start ()
		{
			//StartCoroutine (PeriodicallyGetAdvertHeight ());
		}

		public override void OnStart ()
		{
            AndroidInterface.Banners.SetKey (AppConstants.Android.AdMobBannerKey);
		}

	    public override void SetBannerConfiguration (AVAdPositionConfiguration config)
	    {
	        m_BannerConfig = config;
	        AndroidInterface.Banners.SetBannerAdConfiguration((int)config);
	    }

	    public override void ShowBanner ()
	    {
	        if (!IsAdvertShowing ()) {
                AndroidInterface.Banners.ShowAdvert ();
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
				//m_AdvertHeight = 0;
				m_ShowingAdvert = false;
			}
		}

		public override int GetAdvertHeight ()
		{
			return AndroidInterface.Banners.GetAdHeight ();
		}
	}
}