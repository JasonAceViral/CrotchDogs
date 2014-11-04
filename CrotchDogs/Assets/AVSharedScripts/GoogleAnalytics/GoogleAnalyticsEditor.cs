using UnityEngine;
using System.Collections;

public class GoogleAnalyticsEditor : GoogleAnalytics {

    public override void OnStart()
    {
    }

    public override void LogEvent(string category, string action, string opt_label, long opt_value)
    {
        Debug.Log("Google Analytics Log Event: " + category + " | " + action + " | " + opt_label + " | " + opt_value);
    }

    public override void LogPageView(string page)
    {
        LastPageView = page;
        Debug.Log("Google Analytics Log Page View: " + page);
    }

    public override void Dispatch()
    {
        Debug.Log("Google Analytics - Dispatch");
    }
}
