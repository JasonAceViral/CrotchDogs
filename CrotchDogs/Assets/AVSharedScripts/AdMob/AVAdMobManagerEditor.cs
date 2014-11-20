using System;
using UnityEngine;

public class AVAdMobManagerEditor : AVAdMobManager
{
	//private bool m_ShowingAdvert = false;
	private Texture2D m_RedTexture;
	private GUIStyle m_TextStyle;
	private Rect m_DrawRect;

	private Vector2 adSize = new Vector2(320f, 50f);
	
    public override void ShowBanner ()
    {
        m_ShowingAdvert = true;

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 adPosition = new Vector2(0, 0);

        if ((int)(m_BannerConfig & AVAdPositionConfiguration.eAdConfigTop) != 0)     adPosition.y = 0.0f;
        if ((int)(m_BannerConfig & AVAdPositionConfiguration.eAdConfigBottom) != 0)  adPosition.y = screenSize.y - adSize.y;

        if ((int)(m_BannerConfig & AVAdPositionConfiguration.eAdConfigLeft) != 0)    adPosition.x = 0.0f;
        if ((int)(m_BannerConfig & AVAdPositionConfiguration.eAdConfigRight) != 0)   adPosition.x = screenSize.x - adSize.x;
        if ((int)(m_BannerConfig & AVAdPositionConfiguration.eAdConfigCenter) != 0)  adPosition.x = (screenSize.x * 0.5f) - (adSize.x * 0.5f);

        m_DrawRect = new Rect(adPosition.x, adPosition.y, adSize.x, adSize.y);
        if(m_RedTexture == null){
            Construct();
        }
    }

    public override void HideBanner ()
	{
		m_ShowingAdvert = false;
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
	/// ///////////////
	/// </summary>
	
	void Construct()
	{
		m_RedTexture = new Texture2D(1,1);
		m_RedTexture.SetPixel(1,1,Color.red);
		m_RedTexture.Apply();
		
		//m_TextStyle = new GUIStyle();
		//m_TextStyle.font = AVFontManager.Manager.GetFont("", 64);
		//m_TextStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnGUI()
	{
		if(m_ShowingAdvert){
			GUI.DrawTexture(m_DrawRect, m_RedTexture);
			//GUI.Label(m_DrawRect, "ADWHIRL ADVERT", m_TextStyle);
		}
	}	

	#region implemented abstract members of AVAdwhirlManager
	public override int GetAdvertHeight ()
	{
        return (int)adSize.y;
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

