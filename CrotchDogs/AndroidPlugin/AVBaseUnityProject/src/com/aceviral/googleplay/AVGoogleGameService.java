package com.aceviral.googleplay;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;

import com.aceviral.GameServicesInterface;
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

//	protected GamesClient getGamesClient()
//	{
//		return mHelper.getGamesClient();
//	}
//
//	protected AppStateClient getAppStateClient()
//	{
//		return mHelper.getAppStateClient();
//	}
//
//	protected PlusClient getPlusClient()
//	{
//		return mHelper.getPlusClient();
//	}

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

//	protected void showAlert(String title, String message)
//	{
//		mHelper.showAlert(title, message);
//	}
//
//	protected void showAlert(String message)
//	{
//		mHelper.showAlert(message);
//	}

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

//	protected void reconnectClients(int whichClients)
//	{
//		mHelper.reconnectClients(whichClients);
//	}

//	protected String getScopes()
//	{
//		return mHelper.getScopes();
//	}

//	protected String[] getScopesArray()
//	{
//		return mHelper.getScopesArray();
//	}

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
		UnityPlayer.UnitySendMessage("AVGooglePlayInterface", "SignInComplete", "false");
	}

	@Override
	public void onSignInSucceeded()
	{

		System.out.println("SIGNED IN");
		canUseAchievements = true;

		//game.getBase().checkPreviouslyAchievedAchievements();
		UnityPlayer.UnitySendMessage("AVGooglePlayInterface", "SignInComplete", "true");
	}

	public void unlockAchievement(String achievementId)
	{
		if (canUseAchievements)
		{
			Games.Achievements.unlock(mHelper.getApiClient(), achievementId);
			//mHelper.mGamesClient.unlockAchievement(achievementId);
		}
	}

	public void incrementAchievement(String achievementId, int numSteps)
	{
		if (canUseAchievements && numSteps > 0)
		{
			Games.Achievements.increment(mHelper.getApiClient(), achievementId, numSteps);
			//mHelper.mGamesClient.incrementAchievement(achievementId, numSteps);
		}

	}

	public void showAchievements()
	{
		if (canUseAchievements)
		{
			activity.startActivityForResult(
					Games.Achievements.getAchievementsIntent(mHelper.getApiClient()),REQUEST_ACHIEVEMENTS);
					//mHelper.mGamesClient.getAchievementsIntent(),
					//REQUEST_ACHIEVEMENTS);
		}
	}

	public void updateLeaderboard(String leaderboardId, float score)
	{
		if (canUseAchievements && score > 0)
		{
			Games.Leaderboards.submitScore(mHelper.getApiClient(), leaderboardId, (long)score);
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

	public void showLeaderboards()
	{
		if (canUseAchievements)
		{
			activity.startActivityForResult(
					Games.Leaderboards.getAllLeaderboardsIntent(mHelper.getApiClient()),REQUEST_ACHIEVEMENTS);
					//mHelper.mGamesClient.getAllLeaderboardsIntent(),
					//REQUEST_LEADERBOARDS);
		}
	}


	@Override
	public void load(String currentData, LoadHandler loadHandler) {
		// TODO Auto-generated method stub
		
	}


	@Override
	public void Save(String saveJSON) {
		// TODO Auto-generated method stub
		
	}
	
//	public void load(final String currentData, final LoadHandler loadHandler)
//	{
//		AppStateManager.load(mHelper.getApiClient(), key).setResultCallback(new ResultCallback<AppStateManager.StateResult>()
//		{
//
//			@Override
//			public void onResult(AppStateManager.StateResult result)
//			{
//				AppStateManager.StateConflictResult conflictResult = result.getConflictResult();
//				AppStateManager.StateLoadedResult loadedResult = result.getLoadedResult();
//				if (loadedResult != null)
//				{
//					processStateLoaded(loadedResult, loadHandler);
//				}
//				else if (conflictResult != null)
//				{
//					processStateConflict(conflictResult, currentData, loadHandler);
//				}
//			}
//		});
//	}

//	private void processStateConflict(final AppStateManager.StateConflictResult result, final String localGame, final LoadHandler loadHandler)
//	{
//		// Need to resolve conflict between the two states.
//		// In this example, we use the resolution strategy of taking the union
//		// of the two sets of cleared levels, which means preserving the
//		// maximum star rating of each cleared level:
//		byte[] serverData = result.getServerData();
//		// byte[] localData = result.getLocalData();
//
//		// String localGame = new String(localData);
//		final String serverGame = new String(serverData);
//
//		activity.runOnUiThread(new Runnable()
//		{
//			@Override
//			public void run()
//			{
//				AlertDialog.Builder builder = new AlertDialog.Builder(activity);
//				builder.setTitle("Save Data Conflict");
//				builder.setMessage("A conflict has been detected with the cloud save, please select which version to proceed with.").setCancelable(false).setNegativeButton("Local", new DialogInterface.OnClickListener()
//				{
//					@Override
//					public void onClick(DialogInterface dialog, int id)
//					{
//						AppStateManager.resolve(mHelper.getApiClient(), key, result.getResolvedVersion(), localGame.getBytes());
//						loadHandler.onLoad(localGame);
//
//						canSave = true;
//						dialog.cancel();
//					}
//				}).setPositiveButton("Cloud", new DialogInterface.OnClickListener()
//				{
//					@Override
//					public void onClick(DialogInterface dialog, int id)
//					{
//						AppStateManager.resolve(mHelper.getApiClient(), key, result.getResolvedVersion(), serverGame.getBytes());
//						loadHandler.onLoad(serverGame);
//
//						canSave = true;
//						dialog.cancel();
//					}
//				});
//				AlertDialog alert = builder.create();
//				try
//				{
//					alert.show();
//				}
//				catch (Exception e)
//				{
//					AppStateManager.resolve(mHelper.getApiClient(), key, result.getResolvedVersion(), serverGame.getBytes());
//					loadHandler.onLoad(serverGame);
//					canSave = true;
//				}
//			}
//		});
//
//		// SaveGame resolvedGame = localGame.unionWith(serverGame);
//		// AppStateManager.resolve(mHelper.getApiClient(), result.getStateKey(),
//		// result.getResolvedVersion(), resolvedGame.toBytes());
//	}
//
//	private void processStateLoaded(AppStateManager.StateLoadedResult result, LoadHandler loadHandler)
//	{
//		String data = new String(result.getLocalData());
//		loadHandler.onLoad(data);
//	}
//
//	public void Save(String saveJSON)
//	{
//		if (canSave)
//		{
//		byte[] data = saveJSON.getBytes();
//		AppStateManager.update(mHelper.getApiClient(), key, data);
//		}
//	}
}
