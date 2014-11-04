package com.aceviral.houseads;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;

import com.aceviral.HouseAdInterface;

public class GoogleHouseAds implements HouseAdInterface
{
	
	private Activity m_Activity;

	public GoogleHouseAds(Activity activity)
	{
		this.m_Activity = activity;
	}

	public void showMoreGamesInMarketPlace()
	{
		Intent myIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("market://search?q=pub:Ace Viral"));
		m_Activity.startActivity(myIntent);
	}

	public void showAppLinkInMarketPlaceWithReferral(String packageName, String referralAppName, String adSlotName)
	{
		Intent myIntent = new Intent(Intent.ACTION_VIEW,Uri.parse("market://details?id="+packageName+"&referrer=utm_source%3D"+referralAppName+"%26utm_medium%3D"+adSlotName));
		m_Activity.startActivity(myIntent);
	}

	public void showAppLinkInMarketPlace(String packageName)
	{
		Intent myIntent = new Intent(Intent.ACTION_VIEW,Uri.parse("market://details?id="+packageName));
		m_Activity.startActivity(myIntent);
	}

}
