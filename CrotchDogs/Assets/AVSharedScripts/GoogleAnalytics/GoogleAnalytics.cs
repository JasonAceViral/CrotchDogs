using UnityEngine;
using System.Collections;
using AVHiddenInterface;

namespace AceViral {
    public class GoogleAnalytics {

        public static GoogleAnalytics m_Instance;
        public static string LastPageView { get; protected set; }

        public static GoogleAnalytics Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new GoogleAnalytics ();
                }

    			m_Instance.OnStart ();
                return m_Instance;
            }
        }

    	public void OnStart ()
        {
            #if UNITY_IPHONE
            IPhoneInterface.GoogleAnalytics.SetupWithKey(AppConstants.IOS.GoogleAnalyticsId);
            #endif
        }

        public void LogEvent(string category, string action, string label = "", int value = 0)
        {
            #if UNITY_EDITOR
            Debug.Log("Google Analytics Log Event: " + category + " | " + action + " | " + label + " | " + value);
            #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.Analytics.SendEvent(category, action, label, value);
            #elif UNITY_IPHONE
            IPhoneInterface.GoogleAnalytics.TrackEvent(category, action, label, (int)value);
            #elif UNITY_WP8
            //AVWindowsPhonePlugin.AVGoogleAnalytics.LogEvent(category, action, label, value);
            #endif
        }

    	public void LogPageView (string page)
        {
            LastPageView = page;

            #if UNITY_EDITOR
            Debug.Log("Google Analytics Log Page View: " + page);
            #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.Analytics.SendCurrentScreenName (page);
            #elif UNITY_IPHONE
            IPhoneInterface.GoogleAnalytics.TrackPageView(page);
            #elif UNITY_WP8
            //AVWindowsPhonePlugin.AVGoogleAnalytics.LogPageView(page);
            #endif
        }

        public void Dispatch()
        {
            #if UNITY_EDITOR
            Debug.Log("Google Analytics - Dispatch");
            #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.Analytics.DispatchTracker ();
            #elif UNITY_IPHONE
            IPhoneInterface.GoogleAnalytics.Dispatch();
            #elif UNITY_WP8

            #endif
        }
    }
}