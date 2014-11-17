package com.aceviral;

public interface AnalyticsInterface 
{
	public void applicationStart();

	public void applicationStop();

	public void sendScreenView(String screenName);

	public void trackEvent(String category, String action,String label, long value);

	public void dispatchEvents();
}
