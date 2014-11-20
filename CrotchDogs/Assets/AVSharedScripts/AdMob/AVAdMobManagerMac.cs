using UnityEngine;
using System.Collections;

public class AVAdMobManagerMac : AVAdMobManager {
	
    public override void ShowBanner ()
    {
    }

    public override void HideBanner ()
	{
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

	/// <summary>
	/// ////////
	/// </summary>
	/// <returns>The advert height.</returns>

	#region implemented abstract members of AVAdwhirlManager
	public override int GetAdvertHeight ()
	{
		return 0;
	}
	#endregion

	#region implemented abstract members of AVAdwhirlManager
	public override void OnStart ()
	{
	}
	public override void OnStartWithKey(string key)
	{
	}
	#endregion
}
