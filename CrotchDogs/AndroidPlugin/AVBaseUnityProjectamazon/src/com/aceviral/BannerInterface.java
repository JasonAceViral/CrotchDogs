package com.aceviral;

public interface BannerInterface {

	public void setBannerAdConfiguration(int config);
	
	public void setupAdvertsWithKey(final String adMobkey,final String amazonKey);

	public void displayAdvert();

	public void hideAdvert();
	
	public void loadNewBannerAd();

	public int getAdvertHeight();
	
	public void onDestroy();
}
