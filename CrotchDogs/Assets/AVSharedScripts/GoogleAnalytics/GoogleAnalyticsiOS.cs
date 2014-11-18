using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class GoogleAnalyticsiOS : GoogleAnalytics {

    #if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")] private static extern void _avSetUpGoogleAnalyticsWithKey( string key );
    [DllImport ("__Internal")] private static extern void _avTrackPageView( string page );
    [DllImport ("__Internal")] private static extern void _avTrackEvent( string category, string action, string label, int value );
    [DllImport ("__Internal")] private static extern void _avDispathAnalytics( );
	#endif

	public override void OnStart ()
	{
        #if UNITY_IPHONE && !UNITY_EDITOR
        _avSetUpGoogleAnalyticsWithKey (AVAppConstants.iOSGoogleAnalyticsId);
		#endif
	}

    public override void LogEvent(string category, string action, string opt_label, long opt_value)
    {
        #if UNITY_IPHONE && !UNITY_EDITOR
		_avTrackEvent (category, action, opt_label, (int)opt_value);
		#endif
    }

	public override void LogPageView (string page)
    {
        LastPageView = page;
        #if UNITY_IPHONE && !UNITY_EDITOR
		_avTrackPageView (page);
		#endif
    }

    public override void Dispatch ()
    {
        #if UNITY_IPHONE && !UNITY_EDITOR
        _avDispathAnalytics();
        #endif
    }
}