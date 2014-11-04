package com.aceviral.utility.admob;

import android.app.Activity;
import android.util.Log;

import com.aceviral.InterstitialInterface;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdRequest.Builder;
import com.google.android.gms.ads.InterstitialAd;

public class AdMobInterstitial extends AdListener implements InterstitialInterface
{

	private final Activity m_Activity;
	private InterstitialAd interstitial;
	private String m_AdmobMediationKey;
	private boolean adHasLoaded = false;

	private final String[] m_DeviceIDArray = new String[] {
			"73DD814A2C8FBFF9A8831D6ED45E3DF3", // who knows
			"7657BD346B8E00427267E018E2219284",// galaxy s3
			"9E5C2B9CD5A81C0EC83E657405594D8F",// sony light bar thing
			"59361A00C30BF5AD44B621999111A8F1",// nexus 4
			"118E8E3490472C947423F2BE64C7172F",// moto g 1
			"F19EBFE27914D4F76F4160209A200AA4", // moto g 4
			"3AE911B6C31807DB963F9847C4D213FE" // Nexus 7
	};

	public AdMobInterstitial(Activity activity) {
		m_Activity = activity;
	}

	public void createInterstitialWithKey(final String adKey) {
		m_AdmobMediationKey = adKey;

		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				interstitial = new InterstitialAd(m_Activity);
				interstitial.setAdUnitId(m_AdmobMediationKey);
				interstitial.setAdListener(AdMobInterstitial.this);
				interstitial.loadAd(generateAdRequest());

				Log.v("AdMobInterstitial", "Loading interstitial");
			}
		});
	}

	public void loadNewAdvert() {
		if (interstitial != null) {
			Log.v("AdMobInterstitial",
					"Requesting new admob interstitial advert");

			m_Activity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					adHasLoaded = false;
					interstitial.loadAd(generateAdRequest());
				}
			});
		} else
			Log.v("AdMobInterstitial",
					"Cannot request new advert as the setup function has not been invoked.");
	}

	private AdRequest generateAdRequest() {
		Builder builder = new Builder();
		for (String devID : m_DeviceIDArray) {
			builder.addTestDevice(devID);
		}
		AdRequest adRequest = builder.build();
		return adRequest;
	}

	public boolean isInterstitialReady() {
		Log.w("AdMobInterstitial READY", "Is ready: " + adHasLoaded);
		return adHasLoaded;
	}

	public void showIntersitial() {

		m_Activity.runOnUiThread(new Runnable()
		{
			@Override
			public void run()
			{
				if (interstitial.isLoaded())
					interstitial.show();
			}
		});
	}

	@Override
	public void onAdLoaded() {
		// TODO Auto-generated method stub
		Log.w("AdMobInterstitial READY", "Ad has become ready");
		adHasLoaded = true;
	}

	@Override
	public void onAdClosed() {
		// TODO Auto-generated method stub
		loadNewAdvert();
	}
}
