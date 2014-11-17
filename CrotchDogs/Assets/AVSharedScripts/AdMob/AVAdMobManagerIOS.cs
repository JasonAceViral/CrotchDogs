using System.Collections;
using UnityEngine;
using AceViral;

namespace AVHiddenInterface {
	public class AVAdMobManagerIOS : AVAdMobManager {

	    private bool m_Initialized = false;
	    private int m_AdvertHeight = 0;

	    public override void OnStart()
	    {
	        InitializeAds();
	    }

	    void InitializeAds ()
	    {
            IPhoneInterface.Banners.CreateBannerWithKey(AppConstants.IOS.AdMobBannerKey);
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
                IPhoneInterface.Banners.ShowBannerWithConfig((int)m_BannerConfig);
	            m_ShowingAdvert = true;
	        }

            #if UNITY_IPHONE
            if (RefreshAdOnShow)
                LoadNewBanner();
            #endif

	    }

        public override void LoadNewBanner() {
            #if UNITY_IPHONE
            IPhoneInterface.Banners.RefreshBanner();
            #endif
        }

	    public override void HideBanner() {
	        if (IsAdvertShowing()) {
	            if (!m_Initialized) {
	                InitializeAds();
	            }
			#if UNITY_IPHONE
                IPhoneInterface.Banners.HideBanner();
				#endif
	            m_ShowingAdvert = false;
	        }
	    }

        public override void SetBannerConfiguration (AVAdPositionConfiguration config)
        {
            m_BannerConfig = config;
            #if UNITY_IPHONE
            IPhoneInterface.Banners.SetBannerConfig((int)m_BannerConfig);
            #endif
        }

	    public override int GetAdvertHeight() {
			#if UNITY_IPHONE
	        return 100;//m_AdvertHeight;
			#else
			return 50;
			#endif
	    }
	}
}


