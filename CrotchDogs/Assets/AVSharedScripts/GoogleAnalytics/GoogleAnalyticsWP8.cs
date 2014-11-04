using UnityEngine;
using System.Collections;

public class GoogleAnalyticsWP8 : GoogleAnalytics {

	public override void OnStart()
	{
	}

	public override void LogEvent(string category, string action, string opt_label, long opt_value)
	{
		#if UNITY_WP8
		//AVWindowsPhonePlugin.AVGoogleAnalytics.LogEvent(category, action, opt_label, opt_value);
		#endif
	}

	public override void LogPageView(string page)
	{
		LastPageView = page;
		#if UNITY_WP8
		//AVWindowsPhonePlugin.AVGoogleAnalytics.LogPageView(page);
		#endif
	}

	public override void Dispatch()
	{
	}
}
