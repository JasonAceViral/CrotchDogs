#if UNITY_WP8
using AVWindowsPhonePlugin;
using System;
using UnityEngine;
using AVSharedScripts;

namespace AVHiddenInterface {
	public class AVAdMobManagerWP : AVAdMobManager {

		private INetworkAdapter m_AdNetworkAdapter;

	    public override void OnStart() 
		{
			try {
				m_AdNetworkAdapter = AVAdvertFactory.CreateAdvertAdapter(AVAdvertFactory.AdVendor.PubCenter);
				m_AdNetworkAdapter.Initialize();
			} 
			catch(Exception e)
			{
			}
		}

		public override void OnStartWithKey(string key) {
		}

	    public override void ShowBanner ()
	    {
			m_AdNetworkAdapter.ShowAdvert();	
	    }

        public override void LoadNewBanner()
        {
        }

	    public override void HideBanner() {
			m_AdNetworkAdapter.HideAdvert();
	    }

	    public override int GetAdvertHeight() {
			return Mathf.Max(80, m_AdNetworkAdapter.GetAdvertHeight());
	    }

		// Interstitials

		public override void CreateAdMobInterstitials ()
		{
		}

		public override bool IsAdMobInterstitialReady ()
		{
			return false;
		}

		public override void ShowAbMobIntersitial ()
		{
		}
	}
}
#endif