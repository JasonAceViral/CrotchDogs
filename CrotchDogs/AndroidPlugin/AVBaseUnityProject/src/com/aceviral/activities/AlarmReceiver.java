package com.aceviral.activities;

import com.aceviral.R;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;

public class AlarmReceiver extends BroadcastReceiver
{

	@Override
	public void onReceive(Context context, Intent arg1)
	{
		showNotification(context,arg1);
	}

	@SuppressWarnings("deprecation")
	private void showNotification(Context context,Intent arg1)
	{
		String name = arg1.getExtras().getString("name");

		System.out.println("ready to post notification");
		String message = name + " has challenged you";
		int icon = R.drawable.ic_launcher;
		long when = System.currentTimeMillis();
		NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
		Notification notification = new Notification(icon, message, when);
		notification.sound = Uri.parse("android.resource://"+ context.getPackageName() + "/" + R.raw.notif);
		String title = "Bikemania 2";

		Intent intent = new Intent(context, AVUnityActivity.class);
		PendingIntent pIntent = PendingIntent.getActivity(context, 0, intent, 0);
		notification.setLatestEventInfo(context, title, message, pIntent);
		notification.flags |= Notification.FLAG_AUTO_CANCEL;
		notificationManager.notify(0, notification);
	}

}