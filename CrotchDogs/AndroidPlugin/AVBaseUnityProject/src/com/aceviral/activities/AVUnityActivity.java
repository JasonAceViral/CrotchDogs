package com.aceviral.activities;

import android.app.NativeActivity;
import android.content.Context;
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
import com.aceviral.VideoRewardInterface;
import com.aceviral.analytics.AndroidGoogleAnalyitics;
import com.aceviral.googleplay.AVGoogleGameService;
import com.aceviral.houseads.AmazonHouseAds;
import com.aceviral.houseads.GoogleHouseAds;
import com.aceviral.inappbilling.InAppBilling;
import com.aceviral.utility.AVFacebook;
import com.aceviral.utility.admob.AdMobInterstitial;
import com.aceviral.utility.admob.AdMobManager;
import com.unity3d.player.UnityPlayerActivity;

public abstract class AVUnityActivity extends NativeActivity {

	public static AVUnityActivity CurrentInstance;


	private SocialInterface socialManager;
	private BannerInterface bannerManager;
	private InterstitialInterface interstitialManager;
	private AnalyticsInterface analyticsManager;
	private BillingInterface billingManager;
	private HouseAdInterface houseAdManager;	
	private GameServicesInterface gameServicesManager;

	// ################################################
	// Activity Overrides
	// ################################################
		
	public BannerInterface getBannerManager()
	{
		return bannerManager;
	}
	
	public InterstitialInterface getInterstitialManager()
	{
		return interstitialManager;
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
		if(billingManager == null)
		{
			
				billingManager = new InAppBilling(this);
		}
		
		return billingManager;
	}
	
	public HouseAdInterface getHouseAdManager(boolean isAmazon)
	{
		if(houseAdManager == null)
		{
			if(isAmazon)
			{
				houseAdManager = new AmazonHouseAds(this);
			}
			else
			{
				houseAdManager = new GoogleHouseAds(this);
			}
		}
		
		return houseAdManager;
	}
	
	public SocialInterface getSocialManager()
	{
		return socialManager;
	}
	
	public abstract String getFacebookID();
	public abstract String getAnalyticsID();
	
	public abstract VideoRewardInterface getVideoRewardManager();
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		Log.v("AV","onCreate");
		CurrentInstance = this;
		super.onCreate(savedInstanceState);
		socialManager = new AVFacebook(this,getFacebookID());
				
		interstitialManager = new AdMobInterstitial(this);
		bannerManager = new AdMobManager(this);
		analyticsManager = new AndroidGoogleAnalyitics(this, this, getAnalyticsID());//TODO getString(R.string.ga_trackingId));
		
		gameServicesManager = new AVGoogleGameService(getApplicationContext(), this);		
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
		socialManager.onActivityResult(requestCode, resultCode, data);
		gameServicesManager.onActivityResult(requestCode, resultCode, data);
	}

	@Override
	protected void onStart() {
		super.onStart();
		Log.v("AV","onStart");
		socialManager.onStart();
		analyticsManager.applicationStart();
		gameServicesManager.onStart();
	}

	@Override
	protected void onStop() {
		super.onStop();
		Log.v("AV","onStop");
		socialManager.onStop();
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


}