package com.aceviral.activities;

import com.aceviral.R;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class AlarmReceiver extends BroadcastReceiver 
{

	@Override
	public void onReceive(Context context, Intent arg1)
	{
		showNotification(context);
	}
	
	@SuppressWarnings("deprecation")
	private void showNotification(Context context)
	{
		System.out.println("ready to post notification");
		String message = "Get extra coins and gems in the sale!";
		int icon = R.drawable.ic_launcher;
		long when = System.currentTimeMillis();
		NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
		Notification notification = new Notification(icon, message, when);
		String title = context.getString(R.string.app_name);

		Intent intent = new Intent(context, AVUnityActivity.class);
		PendingIntent pIntent = PendingIntent.getActivity(context, 0, intent, 0);
		notification.setLatestEventInfo(context, title, message, pIntent);
		notification.flags |= Notification.FLAG_AUTO_CANCEL;
		notificationManager.notify(0, notification);
	}

}
