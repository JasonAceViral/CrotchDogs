package com.aceviral;

import android.content.Intent;

public interface GameServicesInterface {

	public void onStart();
	
	public void onStop();

	public void onActivityResult(int request, int response, Intent data);
	
	public boolean isAvailable();
	
	public boolean isSignedIn();
	
	public void signIn();

	public void signOut();
	
	public void showAchievements();

	public void showLeaderboards();

	public void showLeaderboard(String id);

	public void updateAchievement(String achId, float progress, int steps);

	public void updateLeaderboard(String id,float score);
	
	// Cloud
	
	public boolean cloudIsAvailable();
	
	public void cloudFetchData();
	
	public String cloudLoadAllData();
	
	public String cloudLoadKey(String key);
	
	public void cloudSaveDictionaryData(String dictData);
	
	public void cloudSaveKey(String key, String data);
	
	public void cloudSynchronize();
}
