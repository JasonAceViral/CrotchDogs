package com.aceviral.analytics;

import java.net.URLDecoder;
import java.util.Set;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;

import com.google.analytics.tracking.android.CampaignTrackingReceiver;
import com.google.analytics.tracking.android.EasyTracker;
import com.google.analytics.tracking.android.MapBuilder;


/*
 *  A simple Broadcast Receiver to receive an INSTALL_REFERRER
 *  intent and pass it to other receivers, including
 *  the Google Analytics receiver.
 */


public class AVInstallRefferalReceiver extends BroadcastReceiver {

	@Override
	public void onReceive(Context context, Intent intent) {

		// Pass the intent to other receivers.
		Set<String> keys = intent.getExtras().keySet();

		// Call setContext() here so that we can access EasyTracker
		// to update campaign information before calling activityStart().

		if (keys.contains("referrer")) {
			try
			{
				String s = URLDecoder.decode(intent.getExtras().getString("referrer"),"UTF-8");
				Uri uri2 = Uri.parse("http://www.aceviral.com?"+s);

				String source = uri2.getQueryParameter("utm_source");
				String medium = uri2.getQueryParameter("utm_medium");


				GoogleAnalyticsTrackEvent(context,"refferer",source,medium,0);
			}
			catch(Exception e){e.printStackTrace();}

		}
		// When you're done, pass the intent to the Google Analytics receiver.
		new CampaignTrackingReceiver().onReceive(context, intent);
	}

	public void GoogleAnalyticsTrackEvent(Context context,String category, String action,
			String label, long value)
	{
		EasyTracker.getInstance(context).send(
				MapBuilder.createEvent(category, action, label, value).build());
	}

}
