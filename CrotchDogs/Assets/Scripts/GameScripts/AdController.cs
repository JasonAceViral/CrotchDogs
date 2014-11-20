using UnityEngine;
using System.Collections;
using AceViral;

public class AdController : MonoBehaviour {

	public static AdController Instance;
	public bool enableAds;
	void Awake()
	{
		Instance = this;
		Debug.Log ("Init FB");
		AVFacebook.Instance.Initialize ();
	}
	// Use this for initialization
	void Start () 
	{
		ShowBanner ();
	}
	
	public void HideBanner()
	{

				if (enableAds) {
						Debug.Log ("hide banner");
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
