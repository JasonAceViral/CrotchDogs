package com.aceviral.utility.admob;

import android.app.Activity;
import android.graphics.Color;
import android.util.Log;
import android.view.Gravity;
import android.view.ViewGroup.LayoutParams;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import com.aceviral.BannerInterface;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdSize;
import com.google.android.gms.ads.AdRequest.Builder;
import com.google.android.gms.ads.AdView;

/**
 * Creates and manages an active AdMob AdView within a single activity.
 * Occasionally Android decides to destroy certain views, so the manager
 * will recreate views where it needs to
 * 
 * @author Phil <phil.smith@aceviral.com>
 * @version 1.1
 * @since 2013-09-23
 * 
 */
public class AdMobManager extends AdListener implements BannerInterface
{

	private final Activity m_Activity;
	private RelativeLayout m_MainLayout;
	private LinearLayout m_AdMobLinearLayout;
	private AdView m_AdmobView;
	private static AdMobManager _Instance;
	private String m_AdmobMediationKey;
	private boolean adShowing;

	private final String[] m_DeviceIDArray = new String[] { "73DD814A2C8FBFF9A8831D6ED45E3DF3", // who knows
			"7657BD346B8E00427267E018E2219284" ,//galaxy s3
			"9E5C2B9CD5A81C0EC83E657405594D8F" ,//sony light bar thing
			"59361A00C30BF5AD44B621999111A8F1",//nexus 4
			"118E8E3490472C947423F2BE64C7172F",//moto g 1
			"F19EBFE27914D4F76F4160209A200AA4", //moto g 4
			"3AE911B6C31807DB963F9847C4D213FE" // Nexus 7
	};


	public AdMobManager(Activity activity) {
		_Instance = this;
		m_Activity = activity;
	}

	public void setupAdvertsWithKey(final String key) {
		m_AdmobMediationKey = key;
		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Log.v("AdMobManager", "Setting up AdMob adverts with key: " + key);
				LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);

				m_MainLayout = new RelativeLayout(m_Activity.getApplicationContext());
				m_MainLayout.setLayoutParams(params);

				m_AdMobLinearLayout = new LinearLayout(m_Activity.getApplicationContext());
				m_AdMobLinearLayout.setLayoutParams(params);

				m_MainLayout.addView(m_AdMobLinearLayout);

				//m_AdMobLinearLayout.setOrientation(LinearLayout.HORIZONTAL);
				m_AdMobLinearLayout.setGravity(android.view.Gravity.BOTTOM | android.view.Gravity.CENTER_HORIZONTAL);

				m_Activity.addContentView(m_MainLayout, new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT));

				// This function can potentially be called more than once so check for an
				// existing admob view first.
				if(m_AdmobView == null){
					m_AdmobView = new AdView(m_Activity.getApplicationContext());
					m_AdmobView.setAdUnitId(key);
					m_AdmobView.setAdSize(AdSize.BANNER);
					m_AdmobView.setBackgroundColor(Color.BLACK);
					//m_AdmobView.loadAd();
				}
				LoadNewAdvert();
			}
		});
	}

	public void onDestroy()
	{
		m_AdmobView.destroy();
	}

	private AdRequest GenerateAdRequest() {
		Log.v("AdMobManager", "Generating new ad request");
		Builder builder = new Builder();
		for (String devID : m_DeviceIDArray)
		{
			builder.addTestDevice(devID);
		}
		com.google.android.gms.ads.AdRequest adRequest = builder.build();
		return adRequest;
	}
	
	public void loadNewBannerAd()
	{
		LoadNewAdvert();
	}

	private void LoadNewAdvert(){
		Log.v("AdMobManager", "Requesting new admob advert");
		if(m_AdmobView != null){
			m_AdmobView.loadAd(GenerateAdRequest());
		} else {
			Log.e("AdMobManager", "Admob View was null when trying to load a new advert");
			recreateAdmobSystem();
		}
	}

	public void setBannerAdConfiguration(int config) {

		Log.w("AdMobManager CONFIG", "Attempting to set config: " + config);

		int gravitySetting = 0;

		// For some reason I couldn't create an enum in Java and bitwise was not being allowed by compiler!
		// STUPID ERRORS. So manually check

		if(config == 1) gravitySetting = Gravity.TOP; // eAdConfigTop
		else if(config == 2) gravitySetting = Gravity.BOTTOM; // eAdConfigBottom

		else if(config == 4) gravitySetting = Gravity.LEFT; // eAdConfigLeft
		else if(config == 8) gravitySetting = Gravity.RIGHT; // eAdConfigRight

		else if(config == 16) gravitySetting = Gravity.CENTER_VERTICAL | Gravity.CENTER_HORIZONTAL; // eAdConfigCenter

		else if(config == 6) gravitySetting = Gravity.BOTTOM | Gravity.LEFT; // eAdConfigBottomLeft
		else if(config == 10) gravitySetting = Gravity.BOTTOM | Gravity.RIGHT; // eAdConfigBottomRight
		else if(config == 18) gravitySetting = Gravity.BOTTOM | Gravity.CENTER_HORIZONTAL; // eAdConfigBottomCenter

		else if(config == 5) gravitySetting = Gravity.TOP | Gravity.LEFT; // eAdConfigTopLeft
		else if(config == 9) gravitySetting = Gravity.TOP | Gravity.RIGHT; // eAdConfigTopRight
		else if(config == 17) gravitySetting = Gravity.TOP | Gravity.CENTER_HORIZONTAL; // eAdConfigTopCenter

		setAdvertGravity(gravitySetting);
	}

	public void setAdvertGravity(final int gravity){

		Log.w("AdMobManager CONFIG", "GRAVITY BEING SET");

		if(m_AdMobLinearLayout != null)
		{
			m_Activity.runOnUiThread(new Runnable()
			{
				@Override
				public void run()
				{
					m_AdMobLinearLayout.setGravity(gravity);
				}
			});
		}
		else
		{
			Log.e("AdMobManager", "AdMob Linear Layout was null when trying set gravity");
			recreateAdmobSystem();
		}
	}


	public void displayAdvert() {

		Log.v("UNITY","displayAdvert");
		if(adShowing)
			return;

		adShowing = true;

		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try {
					Log.v("AdMobManager", "Adding advert to view");
					if(m_AdMobLinearLayout != null){
						m_AdMobLinearLayout.addView(m_AdmobView);
					} else {
						Log.e("AdMobManager", "AdMob Linear Layout was null when trying to show advert");
						recreateAdmobSystem();
					}
				} catch (Exception e) {
					Log.w("AdMobManager", "Exception observed when trying to show advert: " + e.getMessage());
					e.printStackTrace();
				}
			}
		});
	}

	public void hideAdvert() {

		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try {
					Log.v("AdMobManager", "Removing advert from view");
					if(m_AdMobLinearLayout != null){
						adShowing = false;
						m_AdMobLinearLayout.removeView(m_AdmobView);
					} else {
						Log.e("AdMobManager", "AdMob Linear Layout was null when trying to hide advert");
						recreateAdmobSystem();
					}
				} catch (Exception e) {
					Log.w("AdMobManager", "Exception observed when trying to hide advert: " + e.getMessage());
					e.printStackTrace();
				}
			}
		});
	}

	private void recreateAdmobSystem(){
		releaseAdmobViews();
		setupAdvertsWithKey(m_AdmobMediationKey);
	}

	private void releaseAdmobViews(){

		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run()
			{
				if(m_AdMobLinearLayout != null){
					m_AdMobLinearLayout.removeAllViews();
					m_AdMobLinearLayout = null;
				}
				if(m_MainLayout != null){
					m_MainLayout.removeAllViews();
					m_MainLayout = null;
				}
			}
		});
	}

	public int getAdvertHeight(){
		return (m_AdmobView == null) ? 0 : m_AdmobView.getHeight();
	}
}
