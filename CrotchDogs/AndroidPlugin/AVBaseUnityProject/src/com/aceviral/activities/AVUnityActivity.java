package com.aceviral.activities;

import android.app.AlarmManager;
import android.app.AlertDialog;
import android.app.NativeActivity;
import android.app.PendingIntent;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.os.Handler.Callback;
import android.os.Message;
import android.util.Log;

import com.aceviral.AnalyticsInterface;
import com.aceviral.BannerInterface;
import com.aceviral.BillingInterface;
import com.aceviral.GameServicesInterface;
import com.aceviral.HouseAdInterface;
import com.aceviral.InterstitialInterface;
import com.aceviral.R;
import com.aceviral.SocialInterface;
import com.aceviral.analytics.AndroidGoogleAnalyitics;
import com.aceviral.googleplay.AVGoogleGameService;
import com.aceviral.houseads.AmazonHouseAds;
import com.aceviral.houseads.GoogleHouseAds;
import com.aceviral.inappbilling.InAppBilling;
import com.aceviral.utility.admob.AdMobInterstitial;
import com.aceviral.utility.admob.AdMobManager;
import com.google.android.gcm.GCMRegistrar;
import com.unity3d.player.UnityPlayerActivity;

public abstract class AVUnityActivity extends NativeActivity {

	public static AVUnityActivity CurrentInstance;

	
	private static String RandomGCMIDSenderID = GCMIntentService.SENDER_ID;

	private BannerInterface bannerManager;
	private InterstitialInterface interstitialManager;
	private InterstitialInterface videoManager;
	private AnalyticsInterface analyticsManager;
	private BillingInterface billingManager;
	private HouseAdInterface houseAdManager;	
	private GameServicesInterface gameServicesManager;

	// ################################################
	// Activity Overrides
	// ################################################
		
	public BannerInterface getBannerManager()
	{
		if(bannerManager == null) {
			bannerManager = new AdMobManager(this);
		}
		return bannerManager;
	}
	
	public InterstitialInterface getInterstitialManager()
	{
		if(interstitialManager == null) {
			interstitialManager = new AdMobInterstitial(this);
		}
		return interstitialManager;
	}
	
	public InterstitialInterface getVideoManager()
	{
		if(videoManager == null) {
			videoManager = new AdMobInterstitial(this);
			videoManager.setInterstitialTypeIsVideo(true);
		}
		return videoManager;
	}
	
	public AnalyticsInterface getAnalyticsManager()
	{
		return analyticsManager;
	}
	
	public GameServicesInterface getGameServicesManager()
	{
		return gameServicesManager;
	}
	
	public BillingInterface getBillingManager()
	{		
		return billingManager;
	}
	
	public HouseAdInterface getHouseAdManager()
	{
		return houseAdManager;
	}
	
	public abstract String getFacebookID();
	public abstract String getAnalyticsID();
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		Log.v("AV","onCreate");
		CurrentInstance = this;
		super.onCreate(savedInstanceState);
				
		analyticsManager = new AndroidGoogleAnalyitics(this, this, getAnalyticsID());//TODO getString(R.string.ga_trackingId));
		billingManager = new InAppBilling(this);
		houseAdManager = new GoogleHouseAds(this);
		gameServicesManager = new AVGoogleGameService(getApplicationContext(), this);	
		
		try{
			GCMRegistrar.checkDevice(this);
			GCMRegistrar.checkManifest(this);

			final String regId = GCMRegistrar.getRegistrationId(this);
			if (regId.equals("")) {
				Log.v("GCMIntentService", "Registering with ID: " + RandomGCMIDSenderID);
				GCMRegistrar.register(this, RandomGCMIDSenderID);
			} else {
				Log.v("AV", "Already registered");
			}
			//m_HasInitializedPushNotifications = true;
		} catch( Exception e){

		}
	}
	
	public String getGCMRegistrationId()
	{
		return GCMRegistrar.getRegistrationId(this);
	}

	
	@Override
	public void onDestroy() {
		Log.v("AV","onDestroy");
		bannerManager.onDestroy();
		if(billingManager != null)
		{
			billingManager.onDestroy();
		}
		super.onDestroy();
	}

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		Log.v("AV","onActivityResult");
		if(billingManager != null){
			billingManager.onActivityResult(requestCode, resultCode, data) ;
		} 
		super.onActivityResult(requestCode, resultCode, data);
		gameServicesManager.onActivityResult(requestCode, resultCode, data);
	}

	@Override
	protected void onStart() {
		super.onStart();
		analyticsManager.applicationStart();
		gameServicesManager.onStart();
	}

	@Override
	protected void onStop() {
		super.onStop();
		analyticsManager.applicationStop();
		gameServicesManager.onStop();
	}
		
	public boolean isAndroidFourPointThree() {
		return (android.os.Build.VERSION.SDK_INT > android.os.Build.VERSION_CODES.JELLY_BEAN_MR1);
	}

	public boolean isOnline() {
		ConnectivityManager cm = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
		NetworkInfo ni = cm.getActiveNetworkInfo();
		return ni != null && ni.isConnected();
	}
	
	
	public void pushNotificationsClear()
	{
		System.out.println("notification cancel");
		AlarmManager alarmManager = (AlarmManager) getApplicationContext().getSystemService(Context.ALARM_SERVICE);

		Intent alarmIntent = new Intent(this, AlarmReceiver.class);
		PendingIntent sender = PendingIntent.getBroadcast(this, 192834, alarmIntent, PendingIntent.FLAG_UPDATE_CURRENT);

		// Cancel alarms
		try
		{
			alarmManager.cancel(sender);
		}
		catch (Exception e)
		{
		}
	}

	public void pushNotificationsStart(String name,String timeTxt)
	{
		System.out.println("notification start " + name);
		long time = Long.parseLong(timeTxt);
		System.out.println("notification time1 " + System.currentTimeMillis());
		System.out.println("notification time2 " + (time*1000));
		Intent alarmIntent = new Intent(this, AlarmReceiver.class);
		alarmIntent.putExtra("name", name);

		//long time = System.currentTimeMillis() + 10000;

		PendingIntent sender = PendingIntent.getBroadcast(this, 192834,	alarmIntent, PendingIntent.FLAG_UPDATE_CURRENT);

		AlarmManager alarmManager = (AlarmManager) getSystemService(ALARM_SERVICE);
		alarmManager.set(AlarmManager.RTC_WAKEUP, (time*1000),
				sender);
	}
	
	public void emailSupport(String sendto, String subject) {
		/* Create the Intent */
		final Intent emailIntent = new Intent(android.content.Intent.ACTION_SEND);

		/* Fill it with Data */
		emailIntent.setType("plain/text");
		emailIntent.putExtra(android.content.Intent.EXTRA_EMAIL, new String[]{sendto});
		emailIntent.putExtra(android.content.Intent.EXTRA_SUBJECT, subject);
		emailIntent.putExtra(android.content.Intent.EXTRA_TEXT, "");

		/* Send it off to the Activity-Chooser */
		startActivity(Intent.createChooser(emailIntent, "Send mail..."));

	}
	
	public void showWarning(final String title,final String subject)
	{
		runOnUiThread(new Runnable()
		{

			@Override
			public void run() {
				try {
					AlertDialog.Builder builder = new AlertDialog.Builder(
							AVUnityActivity.this);
					builder.setTitle(title);
					builder.setMessage(subject)
									.setCancelable(false)
									.setPositiveButton("Ok",
											new DialogInterface.OnClickListener() {
										@Override
										public void onClick(DialogInterface dialog,int id) {
										}
									});
					AlertDialog alert = builder.create();
					alert.show();
				} catch (Exception e) {
				}
			}
			
		});
		
	}
	
	public boolean isFromNotification()
	{
		return false;
	}

}