package com.aceviral.adcolony;

import android.app.Activity;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager.NameNotFoundException;

import com.aceviral.VideoRewardInterface;
import com.jirbo.adcolony.AdColony;
import com.jirbo.adcolony.AdColonyAd;
import com.jirbo.adcolony.AdColonyAdAvailabilityListener;
import com.jirbo.adcolony.AdColonyAdListener;
import com.jirbo.adcolony.AdColonyV4VCAd;
import com.jirbo.adcolony.AdColonyV4VCListener;
import com.jirbo.adcolony.AdColonyV4VCReward;


public class AVAdColony implements VideoRewardInterface, AdColonyAdAvailabilityListener, AdColonyV4VCListener, AdColonyAdListener
{
	private Activity m_Activity;
	private String zoneID;

	public AVAdColony(Activity activity,String appID,String zoneID)
	{
		this.m_Activity = activity;
		this.zoneID = zoneID;
		
		PackageInfo pInfo;
		try
		{
			pInfo = m_Activity.getPackageManager().getPackageInfo(m_Activity.getPackageName(), 0);
			AdColony.configure(m_Activity, "version:" + pInfo.versionName + ",store:google", appID, zoneID);
		}
		catch (NameNotFoundException e)
		{
			e.printStackTrace();
			AdColony.configure(m_Activity, "version:1.0.8,store:google", appID, zoneID);
		}
		// AdColony.configure(this, "version:1.0.8,store:google",
		// "app185a7e71e1714831a49ec7", "vz1fd5a8b2bf6841a0a4b826");
		
		// Notify this object about confirmed virtual currency.
		AdColony.addV4VCListener(this);
		AdColony.addAdAvailabilityListener(this);
	}

	@Override
	public void onAdColonyAdAttemptFinished(AdColonyAd arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onAdColonyAdStarted(AdColonyAd arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onAdColonyV4VCReward(AdColonyV4VCReward arg0) {
		
			//adColonyListener.giveReward(arg0.amount());
	}

	@Override
	public void onAdColonyAdAvailabilityChange(boolean arg0, String arg1) {
		
	}

	@Override
	public void showVideo() {
		m_Activity.runOnUiThread(new Runnable()
		{

			private AdColonyV4VCAd adcolonyAd;

			@Override
			public void run()
			{
				// if (adcolonyAd.isReady())
				{
					adcolonyAd = new AdColonyV4VCAd(zoneID).withListener(AVAdColony.this).withConfirmationDialog().withResultsDialog();
					if (adcolonyAd.getAvailableViews() <= 0)
					{
						//makeAlert("No more ads", "Come back soon");
					}
					else
					{
						adcolonyAd.show();

						//System.out.println("adcolony shows ad");
					}
				}

			}

		});
	}

}
