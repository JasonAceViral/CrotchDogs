package com.aceviral;

public interface InterstitialInterface 
{
	public void setInterstitialTypeIsVideo(boolean isVideo);
	
	public void createInterstitialWithKey(final String key);
	
	public boolean isInterstitialReady();

	public void showInterstitial();

	public void loadInterstitialIfNotAlready();

	public void cancelAutoShowInterstitial();
}
