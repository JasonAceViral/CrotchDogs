package com.aceviral;

import android.content.Intent;

public interface SocialInterface 
{
	public void onStart();
	public void onStop();
	public void onActivityResult(int requestCode, int resultCode, Intent data);
	
	public void login();
	public boolean isLoggedIn();
	public String getUserID();
	public String getUserName();
	public void sendChallenge(String title,String message);
	
	public void postScore(int score);
	public void getScores();
	
	public void postMessage(String message);
	
	public void addAchievement(String id);
	public void sendAchievementBatch();
	
	public boolean hasPublishPermissions();
	public void requestPublishPermissions();
}
