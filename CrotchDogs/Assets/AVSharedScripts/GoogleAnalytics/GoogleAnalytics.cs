using UnityEngine;
using System.Collections;

public abstract class GoogleAnalytics {

    public static GoogleAnalytics m_Instance;
    public static string LastPageView { get; protected set; }

    public static GoogleAnalytics Instance {
        get {
            if (m_Instance == null) {
                #if UNITY_EDITOR
                    m_Instance = new GoogleAnalyticsEditor ();
                #elif UNITY_ANDROID
					m_Instance = new GoogleAnalyticsAndroid ();
                #elif UNITY_IPHONE
                    m_Instance = new GoogleAnalyticsiOS ();
				#elif UNITY_METRO
				m_Instance = new GoogleAnalyticsWindows ();
				#elif UNITY_WP8
				m_Instance = new GoogleAnalyticsWP8 ();
                #endif
            }

			m_Instance.OnStart ();
            return m_Instance;
        }
    }

	public abstract void OnStart ();

    public abstract void LogEvent(string category, string action, string opt_label, long opt_value);

	public abstract void LogPageView (string page);

    public virtual void Dispatch() {}
}
