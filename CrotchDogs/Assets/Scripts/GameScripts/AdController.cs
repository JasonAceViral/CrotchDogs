using UnityEngine;
using System.Collections;
using AceViral;

public class AdController : MonoBehaviour {

	public static AdController Instance;
	public bool enableAds;
	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		ShowBanner ();
	}
	
	public void HideBanner()
	{
		Debug.Log ("hide banner");
				if (enableAds) {
						AVAdMobManager.Instance.HideBanner ();
				}
	}

	public void ShowBanner()
	{
				if (enableAds) {
						Debug.Log ("show banner");
						AVAdMobManager.Instance.ShowBannerWithConfiguration (AVAdPositionConfiguration.eAdConfigBottomCenter);
				}
	}
				
	public void ShowInterstitial()
	{
				if (enableAds) {
						//AVAdMobManager.Instance.s ();
				}
	}
			
}
