package com.aceviral.houseads;

import android.app.Activity;

import com.aceviral.HouseAdInterface;

public class AmazonHouseAds implements HouseAdInterface
{

	private Activity m_Activity;

	public AmazonHouseAds(Activity activity)
	{
		this.m_Activity = activity;
	}
	
	@Override
	public void showMoreGamesInMarketPlace() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void showAppLinkInMarketPlaceWithReferral(String packageName,
			String referralAppName, String adSlotName) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void showAppLinkInMarketPlace(String packageName) {
		// TODO Auto-generated method stub
		
	}

}
