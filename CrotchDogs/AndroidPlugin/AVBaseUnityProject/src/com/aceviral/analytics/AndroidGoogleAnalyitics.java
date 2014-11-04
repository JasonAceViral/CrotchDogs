package com.aceviral.analytics;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import com.aceviral.AnalyticsInterface;
import com.google.analytics.tracking.android.EasyTracker;
import com.google.analytics.tracking.android.Fields;
import com.google.analytics.tracking.android.GAServiceManager;
import com.google.analytics.tracking.android.GoogleAnalytics;
import com.google.analytics.tracking.android.Logger.LogLevel;
import com.google.analytics.tracking.android.MapBuilder;
import com.google.analytics.tracking.android.Tracker;

public class AndroidGoogleAnalyitics implements AnalyticsInterface
{
	private final GoogleAnalytics mGa;
	private final Tracker mTracker;

	private final String GA_Property_ID;
	private final int GA_DISPATCH_PERIOD = 30;
	private final boolean GA_IS_DRY_RUN = false;
	private final LogLevel GA_LOG_VERBOSITY = LogLevel.INFO;
	private final String TRACKING_PREF_KEY = "trackingPreference";
	private Activity m_Activity;

	// call in onCreate()
	public AndroidGoogleAnalyitics(Activity activity, final Context context,String GA_Property_Id)
	{
		this.m_Activity = activity;
		mGa = GoogleAnalytics.getInstance(activity);
		GA_Property_ID = GA_Property_Id;

		mTracker = mGa.getTracker(GA_Property_ID);

		GAServiceManager.getInstance().setLocalDispatchPeriod(
				(GA_DISPATCH_PERIOD));

		mGa.setDryRun(GA_IS_DRY_RUN);

		mGa.getLogger().setLogLevel(GA_LOG_VERBOSITY);

		SharedPreferences userPrefs = PreferenceManager
				.getDefaultSharedPreferences(activity);
		userPrefs
		.registerOnSharedPreferenceChangeListener(new SharedPreferences.OnSharedPreferenceChangeListener()
		{
			@Override
			public void onSharedPreferenceChanged(
					SharedPreferences sharedPreferences, String key)
			{
				if (key.equals(TRACKING_PREF_KEY))
				{
					GoogleAnalytics.getInstance(context).setAppOptOut(
							sharedPreferences.getBoolean(key, false));
				}
			}
		});

	}

	private Tracker getGaTracker()
	{
		return mTracker;
	}

	private GoogleAnalytics getGaInstance()
	{
		return mGa;
	}
	
	public void applicationStart() {
		EasyTracker.getInstance(m_Activity).activityStart(m_Activity);
	}

	public void applicationStop() {
		EasyTracker.getInstance(m_Activity).activityStop(m_Activity);
	}

	public void sendScreenView(String screenName) {
		Tracker easyTracker = EasyTracker.getInstance(m_Activity);
		easyTracker.set(Fields.SCREEN_NAME, screenName);
		easyTracker.send(MapBuilder.createAppView().build());
	}

	public void trackEvent(String category, String action,
			String label, long value) {
		EasyTracker.getInstance(m_Activity).send(
				MapBuilder.createEvent(category, action, label, value).build());
	}

	public void dispatchEvents() {
		GAServiceManager.getInstance().dispatchLocalHits();
	}
}
