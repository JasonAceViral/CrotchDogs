package com.aceviral.utility.admob;

import android.app.Activity;
import android.util.Log;
import android.widget.Toast;

import com.aceviral.InterstitialInterface;
import com.aceviral.utility.AVUtility;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdRequest.Builder;
import com.google.android.gms.ads.InterstitialAd;
import com.unity3d.player.UnityPlayer;

public class AdMobInterstitial extends AdListener implements InterstitialInterface {

	private final Activity m_Activity;
	private static AdMobInterstitial _Instance;
	private InterstitialAd interstitial;
	private String m_AdmobMediationKey;
	private boolean adHasLoaded = false;
	private boolean adIsLoading = false;
	private boolean adHasRefreshedSinceView = false;
	private boolean waitingToSeeAd = false;
	private int failedRefreshAttempts = 0;
	private long timeOfOriginalRequest;
	private boolean isVideoInterstitial = false;

	public boolean showToastOnOnceLoadedShow = true;
	private Toast loadingToast;
	private boolean adHasBeenSetup = false;

	private final String[] m_DeviceIDArray = new String[] {
			"73DD814A2C8FBFF9A8831D6ED45E3DF3", // who knows
//			"7657BD346B8E00427267E018E2219284",// galaxy s3
			"9E5C2B9CD5A81C0EC83E657405594D8F",// sony light bar thing
			"59361A00C30BF5AD44B621999111A8F1",// nexus 4
//			"118E8E3490472C947423F2BE64C7172F",// moto g 1
//			"F19EBFE27914D4F76F4160209A200AA4", // moto g 4
			"3AE911B6C31807DB963F9847C4D213FE" // Nexus 7
	};

	public AdMobInterstitial(Activity activity) {
		_Instance = this;
		m_Activity = activity;
	}
	
	void setupIntersitialOnUIThread()
	{
		interstitial = null;
		adHasLoaded = false;
		adIsLoading = false;
		adHasRefreshedSinceView = false;
		waitingToSeeAd = false;
		failedRefreshAttempts = 0;
		
		m_Activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Log.v("AdMobInterstitial",
						"Setting up AdMob Interstitial with key: "
								+ m_AdmobMediationKey);

				interstitial = new InterstitialAd(m_Activity);
				interstitial.setAdUnitId(m_AdmobMediationKey);
				interstitial.setAdListener(_Instance);
				interstitial.loadAd(GenerateAdRequest());
				if(isVideoInterstitial)
					UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsLoading", "none");

				Log.v("AdMobInterstitial", "Loading interstitial");
			}
		});
	}
    
	
	////////////////
	//// Interface
	////////////////
	    
    public void setInterstitialTypeIsVideo(boolean isVideo) {
        
		isVideoInterstitial = isVideo;
	}
    
	public void createInterstitialWithKey(final String adKey) {
		
		if(adHasBeenSetup)
		{
			Log.w("AdMobInterstitial", "Not creating interstitial as it has already been created!");
			return;
		}
        
		m_AdmobMediationKey = adKey;
		adHasBeenSetup = true;
		setupIntersitialOnUIThread();
	}
    
    public boolean isInterstitialReady() {
		Log.w("AdMobInterstitial", "Advert Is ready: " + adHasLoaded);
		return adHasLoaded;
	}
    
	public void showInterstitial() {
		
		Log.v("AdMobInterstitial", "show advert for key: " + m_AdmobMediationKey);
		
		if(adHasLoaded && adHasRefreshedSinceView)
		{
			Log.w("AdMobInterstitial", "Showing advert");
			waitingToSeeAd = false;
			
			m_Activity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					Log.w("AdMobInterstitial", "Show() via main ui thread. Has ad loaded? " + interstitial.isLoaded());
					
					if (interstitial.isLoaded())
						interstitial.show();
					else
					{
						adHasLoaded = false;
						adHasRefreshedSinceView = false;
						LoadNewAdThenShow();
					}
				}
			});
		}
		else
		{
			if(!adHasLoaded)
				Log.w("AdMobInterstitial", "AdHasLoaded is not true. Calling LoadNewAdThenShow()");
			if(!adHasRefreshedSinceView)
				Log.w("AdMobInterstitial", "AdHasRefreshedSinceView is not true. Calling LoadNewAdThenShow()");
			LoadNewAdThenShow();
		}
	}
	
	public void loadInterstitialIfNotAlready() {
		if(!!adIsLoading && !adHasRefreshedSinceView)
	    {
	    	  LoadNewAdvert();
	     }
	}

	public void cancelAutoShowInterstitial() {
		
		waitingToSeeAd = false;
	}
	
    
	////////////////
	//// Private Implementation
	////////////////
	
	void LoadNewAdvert() {

		if(adIsLoading)
			return;
		
		if (interstitial != null) {
			Log.v("AdMobInterstitial", "Requesting new admob interstitial advert");
			
			if(!adHasRefreshedSinceView) 
			{
				m_Activity.runOnUiThread(new Runnable() {
					@Override
					public void run() {
						adHasLoaded = false;
						adIsLoading = true;
						interstitial.loadAd(GenerateAdRequest());
						if(isVideoInterstitial)
							UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsLoading", "none");
					}
				});
			}
			else if(waitingToSeeAd)
			{
			    showInterstitial();
			}
		} else
			Log.v("AdMobInterstitial", "Cannot request new advert as the setup function has not been invoked.");
	}

	private AdRequest GenerateAdRequest() {
		AVUtility.DebugOut("AdMobInterstitial", "Generating new ad request");
		Builder builder = new Builder();
		for (String devID : m_DeviceIDArray) {
			builder.addTestDevice(devID);
		}
		AdRequest adRequest = builder.build();
		return adRequest;
	}
	
	void LoadNewAdThenShow()
	{
		Log.w("AdMobInterstitial", (adIsLoading ? "Ad is still loading" : "Ad requires loading") + ". ad will show once loaded");
		if(waitingToSeeAd)
		{
			long timeWaitedForLoad = System.currentTimeMillis() - timeOfOriginalRequest;
			Log.w("AdMobInterstitial", "Still waiting for ad. First ad requested " + (timeWaitedForLoad / 1000f) + " seconds ago");
			if(timeWaitedForLoad > 30000) // 30 seconds
			{
				Log.w("AdMobInterstitial", "Advert request has taken more than 30 seconds. recreating system");
				setupIntersitialOnUIThread();
				return;
			}
		}
		else
		{
			timeOfOriginalRequest = System.currentTimeMillis();
			waitingToSeeAd = true;
		}
		
		HideToastIfShowing();
		
		if(showToastOnOnceLoadedShow)
		{
			Log.w("AdMobInterstitial", "Going to try and show TOAST");
			
			m_Activity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					HideToastIfShowing();
					loadingToast = Toast.makeText(m_Activity, "Loading...", Toast.LENGTH_SHORT);
					loadingToast.show();
				}
			});
		}

		LoadNewAdvert();
	}
	
	////////////////
	//// Overrides
	////////////////
	
	@Override
	public void onAdLoaded() {
		Log.w("AdMobInterstitial", "Ad has become ready");
		
		adHasLoaded = true;
		adIsLoading = false;
		adHasRefreshedSinceView = true;
		failedRefreshAttempts = 0;
		
		HideToastIfShowing();
		
		if(isVideoInterstitial)
			UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsReady", "none");
		else UnityPlayer.UnitySendMessage("AVAdvertisingManager", "InterstitialIsReady", "none");
		
		if(waitingToSeeAd)
	    {
	        waitingToSeeAd = false;
	        showInterstitial();
	    }
	}

	@Override
    public void onAdFailedToLoad(int errorCode) {
      String message = String.format("onAdFailedToLoad (%s)", getErrorReason(errorCode));
      Log.d("AdMobInterstitial", "Failed to load ad with error: " + message);

      adHasLoaded = false;
      adIsLoading = false;
      adHasRefreshedSinceView = false;
      failedRefreshAttempts++;
      
      HideToastIfShowing();
      
      if(isVideoInterstitial)
			UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialIsNotReady", "none");
		else UnityPlayer.UnitySendMessage("AVAdvertisingManager", "InterstitialIsNotReady", "none");
      
      if(waitingToSeeAd)
      {
	      if(errorCode == AdRequest.ERROR_CODE_NO_FILL)
	      {
	    	  if(isVideoInterstitial)
	  			UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialHasNoFill", "none");
	      }
      }

      if(failedRefreshAttempts <= 2)
      {
    	  LoadNewAdvert();
      }
      else 
      {
    	  failedRefreshAttempts = 0; // Reset to 0. This allows 3 failures to happen per user request
    	  
    	  if(waitingToSeeAd && isVideoInterstitial)
          {
    	      if(errorCode == AdRequest.ERROR_CODE_NO_FILL)
    	      {
    	    	  UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialHasNoFill", "none");
    	      }
    	      else 
    	      {
    	    	  UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialGeneralFail", "none");
    	      }
          }
      }  
    }
	
	@Override
	public void onAdOpened() {
		waitingToSeeAd = false;
		
		if(isVideoInterstitial)
			UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialWillPresentScreen", "none");
		else UnityPlayer.UnitySendMessage("AVAdvertisingManager", "InterstitialWillPresentScreen", "none");
	}
	
	@Override
	public void onAdClosed() {
		adHasRefreshedSinceView = false;
		LoadNewAdvert();
		
		if(isVideoInterstitial)
			UnityPlayer.UnitySendMessage("AVAdvertisingManager", "VideoInterstitialWillDismiss", "none");
		else UnityPlayer.UnitySendMessage("AVAdvertisingManager", "InterstitialWillDismiss", "none");
	}
	
	private void HideToastIfShowing() {
		
		if(loadingToast != null)
		{
			loadingToast.cancel();
			loadingToast = null;
		}
	}
	
	/** Gets a string error reason from an error code. */
	  private String getErrorReason(int errorCode) {
	    String errorReason = "";
	    switch(errorCode) {
	      case AdRequest.ERROR_CODE_INTERNAL_ERROR:
	        errorReason = "Internal error";
	        break;
	      case AdRequest.ERROR_CODE_INVALID_REQUEST:
	        errorReason = "Invalid request";
	        break;
	      case AdRequest.ERROR_CODE_NETWORK_ERROR:
	        errorReason = "Network Error";
	        break;
	      case AdRequest.ERROR_CODE_NO_FILL:
	        errorReason = "No fill";
	        break;
	    }
	    return errorReason;
	  }
}
