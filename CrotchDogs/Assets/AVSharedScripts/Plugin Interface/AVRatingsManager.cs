using UnityEngine;

public class AVRatingsManager
{
	private static AVRatingsManager m_CurrentManager;

	public static AVRatingsManager Manager {
		get {
			if (m_CurrentManager == null) {
#if UNITY_ANDROID
				//if (GameConstants.CompileForAmazonAppStore) {
				//m_CurrentManager = new AVRatingsManagerAmazon ();
				//} else {
				m_CurrentManager = new AVRatingsManagerAndroid ();
				//}
#elif UNITY_IPHONE
				m_CurrentManager = new AVRatingsManagerIOS ();
#elif UNITY_STANDALONE_OSX
				m_CurrentManager = new AVRatingsManagerMac();
#elif UNITY_WEBPLAYER
				m_CurrentManager = new AVRatingsManagerWeb();
#elif UNITY_METRO || UNITY_WP8
                m_CurrentManager = new AVRatingsManagerWeb();
#endif

				m_CurrentManager.OnStart ();
			}
			return m_CurrentManager;
		}
	}

	private int m_TimesInitialized = 0;

	public int TimesInitialized { get { return m_TimesInitialized; } }

	protected virtual void OnStart ()
	{
		m_TimesInitialized = PlayerPrefs.GetInt ("TimesOpened", 0);
		m_TimesInitialized++;
		PlayerPrefs.SetInt ("TimesOpened", m_TimesInitialized);
	}

	public bool HasRated ()
	{
		return (PlayerPrefs.GetInt ("HasRated") == 1);
	}

	public virtual void AskToRate ()
	{
		PlayerPrefs.SetInt ("HasRated", 1);
		PlayerPrefs.Save ();
	}
}
