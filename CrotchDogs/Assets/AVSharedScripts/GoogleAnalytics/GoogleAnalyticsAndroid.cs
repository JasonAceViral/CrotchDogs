using UnityEngine;
using System.Collections;

public class GoogleAnalyticsAndroid : GoogleAnalytics
{
    public override void OnStart()
    {
    }

    public override void LogEvent(string category, string action, string opt_label, long opt_value)
    {
        AVAndroidInterface.Analytics.SendEvent(category, action, opt_label, opt_value);
    }

    public override void LogPageView(string page)
    {
        LastPageView = page;
		AVAndroidInterface.Analytics.SendCurrentScreenName (page);
    }

    public override void Dispatch()
    {
        AVAndroidInterface.Analytics.DispatchTracker ();
    }
}
