package com.aceviral.activities;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

import com.aceviral.R;
import com.google.android.gcm.GCMBaseIntentService;

public class GCMIntentService extends GCMBaseIntentService
{

	public static final String SENDER_ID = "157541033944";

	public GCMIntentService()
	{
		super(SENDER_ID);
		Log.d("GCMIntentService", SENDER_ID);
	}

	@Override
	protected void onError(Context arg0, String arg1)
	{
		// TODO Auto-generated method stub
		Log.d("onError", arg1);

	}

	@Override
	protected void onMessage(Context arg0, Intent arg1)
	{
		// TODO Auto-generated method stub
		Log.d("onMessage", String.valueOf(arg1.getExtras()));

		final int NOTIF_ID = 1002036458;
		CharSequence chars = getResources().getString(R.string.app_name);
		NotificationManager notifManager = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
		Notification note = new Notification(R.drawable.ic_launcher, chars, System.currentTimeMillis());
		PendingIntent intent = PendingIntent.getActivity(this, 0, new Intent(this, AVUnityActivity.class), 0);
		note.setLatestEventInfo(this, getResources().getString(R.string.app_name), String.valueOf(arg1.getExtras().getString("message")), intent);

		notifManager.notify(NOTIF_ID, note);


	}

	@Override
	protected void onRegistered(Context arg0, String arg1)
	{
		// TODO Auto-generated method stub
		Log.d("onRegistered", arg1);

	}

	@Override
	protected void onUnregistered(Context arg0, String arg1)
	{
		// TODO Auto-generated method stub
		Log.d("onUnregistered", arg1);

	}

}