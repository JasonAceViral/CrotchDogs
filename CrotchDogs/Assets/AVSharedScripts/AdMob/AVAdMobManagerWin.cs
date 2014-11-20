
public class AVAdMobManagerWin : AVAdMobManager {

	public override void OnStart() {

	}

	public override void OnStartWithKey(string key) {
	}

	public override void ShowBanner ()
	{
	}

	public override void HideBanner() {

	}

	public override int GetAdvertHeight() {
		return 0;
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
