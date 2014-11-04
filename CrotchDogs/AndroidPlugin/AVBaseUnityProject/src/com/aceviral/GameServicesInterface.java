package com.aceviral;

import com.aceviral.googleplay.LoadHandler;

import android.content.Intent;

public interface GameServicesInterface {

	public void onStart();
	
	public void onStop();

	public void onActivityResult(int request, int response, Intent data);
	
	public void signIn();

	public void signOut();

	public void unlockAchievement(String achId);

	public void incrementAchievement(String achId,int steps);

	public void showAchievements();

	public void showLeaderboards();

	public void showLeaderboard(String id);

	public void updateLeaderboard(String id,float score);

	public boolean isSignedIn();
	
	void load(final String currentData, final LoadHandler loadHandler);

	void Save(String saveJSON);
}
