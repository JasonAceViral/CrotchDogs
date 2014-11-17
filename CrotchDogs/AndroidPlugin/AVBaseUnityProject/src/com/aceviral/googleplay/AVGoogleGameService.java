package com.aceviral.googleplay;

import java.util.HashMap;
import java.util.Map;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;

import com.aceviral.GameServicesInterface;
import com.google.android.gms.appstate.AppStateManager;
import com.google.android.gms.common.api.ResultCallback;
import com.google.android.gms.games.Games;
import com.unity3d.player.UnityPlayer;

public class AVGoogleGameService implements GameHelper.GameHelperListener,GameServicesInterface
{

	protected GameHelper mHelper;

	Context cont;
	final int REQUEST_ACHIEVEMENTS = 332211;
	final int REQUEST_LEADERBOARDS = 332311;
	public static final int CLIENT_GAMES = GameHelper.CLIENT_GAMES;
	public static final int CLIENT_APPSTATE = GameHelper.CLIENT_APPSTATE;
	public static final int CLIENT_PLUS = GameHelper.CLIENT_PLUS;
	public static final int CLIENT_ALL = GameHelper.CLIENT_ALL;

	// Requested clients. By default, that's just the games client.
	protected int mRequestedClients = CLIENT_GAMES;

	// stores any additional scopes.
	private String[] mAdditionalScopes;

	protected String mDebugTag = "BaseGameActivity";
	protected boolean mDebugLog = false;

	private Activity activity;

	private static boolean canUseAchievements;

	public AVGoogleGameService(Context cont, Activity act)
	{
		this.activity = act;
		this.cont = cont;
		mHelper = new GameHelper(act,GameHelper.CLIENT_GAMES);
		if (mDebugLog)
		{
			mHelper.enableDebugLog(mDebugLog, mDebugTag);
		}
		mHelper.setup(this);

	}
	
	protected AVGoogleGameService(Context cont, Activity act,
			int requestedClients)
	{
		this.activity = act;
		this.cont = cont;
		setRequestedClients(requestedClients);
	}

	protected void setRequestedClients(int requestedClients,String... additionalScopes)
	{
		mRequestedClients = requestedClients;
		mAdditionalScopes = additionalScopes;
	}

	// Public methods
	
	public void onStart()
	{
		mHelper.onStart(activity);
	}

	public void onStop()
	{
		mHelper.onStop();
	}

	public void onActivityResult(int request, int response, Intent data)
	{
		mHelper.onActivityResult(request, response, data);
	}
	
	public boolean isAvailable()
	{
		return mHelper.isAvailable();
	}

	public boolean isSignedIn()
	{
		return mHelper.isSignedIn();
	}

	public void signIn()
	{
		System.out.println("signingin");
		mHelper.beginUserInitiatedSignIn();
	}

	public void signOut()
	{
		mHelper.signOut();
		canUseAchievements = false;
	}
	
	public void showAchievements()
	{
		if (canUseAchievements)
		{
			activity.startActivityForResult(
					Games.Achievements.getAchievementsIntent(mHelper.getApiClient()),REQUEST_ACHIEVEMENTS);
		}
	}
	
	public void showLeaderboards()
	{
		if (canUseAchievements)
		{
			activity.startActivityForResult(
					Games.Leaderboards.getAllLeaderboardsIntent(mHelper.getApiClient()),REQUEST_ACHIEVEMENTS);
		}
	}
	
	public void showLeaderboard(String leaderboardId)
	{
		if (canUseAchievements)
		{
			activity.startActivityForResult(
					Games.Leaderboards.getLeaderboardIntent(mHelper.getApiClient(),leaderboardId),REQUEST_ACHIEVEMENTS);
		}
	}
	
	public void updateAchievement(String achievementId, float progress, int steps)
	{
		if (canUseAchievements)
		{
			if(progress >= 1f)
				Games.Achievements.unlock(mHelper.getApiClient(), achievementId);
			else if(steps > 0)
				Games.Achievements.increment(mHelper.getApiClient(), achievementId, steps);
		}
	}

	public void updateLeaderboard(String leaderboardId, float score)
	{
		if (canUseAchievements && score > 0)
		{
			Games.Leaderboards.submitScore(mHelper.getApiClient(), leaderboardId, (long)score);
		}
	}

	// ////////////////
	// Cloud Storage
	// ////////////////
	
	private HashMap<String, String> cloudCache;
	private final int cloudStateKey = 0;
	private boolean cloudCanSave = false;
	
	public boolean cloudIsAvailable() {
		return mHelper.isAvailable();
	}

	@Override
	public void cloudFetchData() {
		AppStateManager.load(mHelper.getApiClient(), cloudStateKey).setResultCallback(new ResultCallback<AppStateManager.StateResult>() {
			@Override
			public void onResult(AppStateManager.StateResult result) {
				AppStateManager.StateConflictResult conflictResult = result.getConflictResult();
				AppStateManager.StateLoadedResult loadedResult = result.getLoadedResult();
				if (loadedResult != null) {
					processStateLoaded(loadedResult);
				} else if (conflictResult != null) {
					processStateConflict(conflictResult);
				}
			}
		});
	}

	private void processStateConflict(final AppStateManager.StateConflictResult result) {
		// Need to resolve conflict between the two states.
		// In this example, we use the resolution strategy of taking the union
		// of the two sets of cleared levels, which means preserving the
		// maximum star rating of each cleared level:
		byte[] serverData = result.getServerData();
		// byte[] localData = result.getLocalData();

		// String localGame = new String(localData);
		final String serverGame = new String(serverData);

		activity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				AlertDialog.Builder builder = new AlertDialog.Builder(activity);
				builder.setTitle("Save Data Conflict");
				builder.setMessage("A conflict has been detected with the cloud save, please select which version to proceed with.").setCancelable(false).setNegativeButton("Local", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int id) {
						AppStateManager.resolve(mHelper.getApiClient(), cloudStateKey, result.getResolvedVersion(), getFormattedCloudDataString().getBytes());
						InformUnityCloudHasUpdated();

						cloudCanSave = true;
						dialog.cancel();
					}
				}).setPositiveButton("Cloud", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int id) {
						AppStateManager.resolve(mHelper.getApiClient(), cloudStateKey, result.getResolvedVersion(), serverGame.getBytes());
						processLoadedCloudData(serverGame);
						InformUnityCloudHasUpdated();

						cloudCanSave = true;
						dialog.cancel();
					}
				}).setCancelable(false);
				AlertDialog alert = builder.create();
				try {
					alert.show();
				} catch (Exception e) {
					AppStateManager.resolve(mHelper.getApiClient(), cloudStateKey, result.getResolvedVersion(), serverGame.getBytes());
					processLoadedCloudData(serverGame);
					InformUnityCloudHasUpdated();
					cloudCanSave = true;
				}
			}
		});

		// SaveGame resolvedGame = localGame.unionWith(serverGame);
		// AppStateManager.resolve(mHelper.getApiClient(), result.getStateKey(),
		// result.getResolvedVersion(), resolvedGame.toBytes());
	}

	private void processStateLoaded(AppStateManager.StateLoadedResult result) {
		System.out.println(result);
		if (result.getLocalData() != null && result.getLocalData().length > 0) {
			String data = new String(result.getLocalData());
			processLoadedCloudData(data);
		} else {
			System.out.println("AV error loading there was nothing");
		}
		cloudCanSave = true;

		InformUnityCloudHasUpdated();
	}

	private void processLoadedCloudData(String data) {
		if (cloudCache != null) {
			cloudCache.clear();
			cloudCache = null;
		}

		if (data == null || data.length() == 0) {
			return;
		}

		cloudCache = new HashMap<String, String>();
		String[] allKeyValues = data.split("\\|_\\|");

		for (int i = 0; i < allKeyValues.length; i++) {
			String[] keyData = allKeyValues[i].split("\\|-\\|");
			if (keyData.length == 2) {
				cloudCache.put(keyData[0], keyData[1]);
			}
		}
	}

	// User interface

	@Override
	public String cloudLoadAllData() {
		return getFormattedCloudDataString();
	}

	@Override
	public String cloudLoadKey(String key) {
		if (cloudCache != null && cloudCache.containsKey(key))
			return cloudCache.get(key);
		return "";
	}

	@Override
	public void cloudSaveDictionaryData(String dictData) {
		if (cloudCache == null) {
			cloudCache = new HashMap<String, String>();
		}

		String[] allKeyValues = dictData.split("\\|_\\|");

		for (int i = 0; i < allKeyValues.length; i++) {
			String[] keyData = allKeyValues[i].split("\\|-\\|");
			if (keyData.length == 2) {
				cloudCache.put(keyData[0], keyData[1]);
			}
		}
	}

	@Override
	public void cloudSaveKey(String key, String data) {
		if (cloudCache == null) {
			cloudCache = new HashMap<String, String>();
		}

		cloudCache.put(key, data);
	}

	@Override
	public void cloudSynchronize() {
		if (cloudCanSave) {
			String parseData = getFormattedCloudDataString();

			byte[] data = parseData.getBytes();
			AppStateManager.update(mHelper.getApiClient(), cloudStateKey, data);
		}
	}

	private String getFormattedCloudDataString() {
		if (cloudCache == null)
			return "";

		String parseData = "";

		for (Map.Entry<String, String> entry : cloudCache.entrySet()) {
			if (parseData.length() > 0) {
				parseData += "|_|";
			}

			parseData += entry.getKey() + "|-|" + entry.getValue();
		}
		return parseData;
	}

	private void InformUnityCloudHasUpdated() {
		UnityPlayer.UnitySendMessage("AVCloudManager", "CloudUpdateAvailable", "");
	}
	
	// Non-Public methods

	protected void enableDebugLog(boolean enabled, String tag)
	{
		mDebugLog = true;
		mDebugTag = tag;
		if (mHelper != null)
		{
			mHelper.enableDebugLog(enabled, tag);
		}
	}

	public String getInvitationId()
	{
		return mHelper.getInvitationId();
	}

	protected boolean hasSignInError()
	{
		return mHelper.hasSignInError();
	}

	protected GameHelper.SignInFailureReason getSignInError()
	{
		return mHelper.getSignInError();
	}

	@Override
	public void onSignInFailed()
	{
		// TODO Auto-generated method stub
		System.out.println("SIGN IN FAILED");
		canUseAchievements = false;
		UnityPlayer.UnitySendMessage("AVGameServicesInterface", "SignInComplete", "failure");
	}

	@Override
	public void onSignInSucceeded()
	{

		System.out.println("SIGNED IN");
		canUseAchievements = true;

		//game.getBase().checkPreviouslyAchievedAchievements();
		UnityPlayer.UnitySendMessage("AVGameServicesInterface", "SignInComplete", "success");
	}
}
