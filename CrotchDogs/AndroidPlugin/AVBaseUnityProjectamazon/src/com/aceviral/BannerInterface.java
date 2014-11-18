package com.aceviral;

public interface BannerInterface {

	public void setBannerAdConfiguration(int config);
	
	public void setupAdvertsWithKey(final String key);

	public void displayAdvert();

	public void hideAdvert();

	public int getAdvertHeight();
	
	public void onDestroy();
}
