using UnityEngine;
using System.Collections;

public class GoogleAnalyticsWindows : GoogleAnalytics {

	public override void OnStart()
	{
	}

	public override void LogEvent(string category, string action, string opt_label, long opt_value)
	{
	}

	public override void LogPageView(string page)
	{
		LastPageView = page;
	}

	public override void Dispatch()
	{
	}
}
