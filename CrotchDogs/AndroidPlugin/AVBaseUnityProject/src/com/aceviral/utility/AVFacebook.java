package com.aceviral.utility;

import java.util.Arrays;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.util.Locale;
import java.util.Queue;

import org.json.JSONArray;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.Window;
import android.view.WindowManager;

import com.aceviral.SocialInterface;
import com.aceviral.activities.AVUnityActivity;
import com.facebook.AccessToken;
import com.facebook.FacebookException;
import com.facebook.FacebookOperationCanceledException;
import com.facebook.FacebookRequestError;
import com.facebook.HttpMethod;
import com.facebook.Request;
import com.facebook.Request.GraphUserCallback;
import com.facebook.RequestBatch;
import com.facebook.Response;
import com.facebook.Session;
import com.facebook.Session.StatusCallback;
import com.facebook.SessionState;
import com.facebook.model.GraphUser;
import com.facebook.widget.WebDialog;
import com.facebook.widget.WebDialog.OnCompleteListener;
import com.unity3d.player.UnityPlayer;

@SuppressLint("DefaultLocale")
public class AVFacebook implements SocialInterface
{

	// ############################################################
	// -----
	// Member Variables
	// -----
	// ############################################################

	private final AVUnityActivity m_MainActivity;
	private Session m_FacebookSession;
	private String m_CurrentUserID;
	private String m_CurrentUserName = "";
	private final StatusCallback m_StatusCallback = new SessionStatusCallback();
	private final GraphUserCallback m_UserRequestCallback = new CurrentUserRequestCallback();
	private static final List<String> PERMISSIONS = Arrays.asList("publish_actions");

	private final Queue<String> m_AchievementQueue = new LinkedList<String>();

	private final Queue<String> m_MessageQueue = new LinkedList<String>();
	private int m_ScoreToSend = -1;

	private final String APP_ID;

	// Error Codes
	private final int ACCESS_TOKEN_REQUIRED = 104;
	private final int ACHIEVEMENT_ALREADY_EARNED = 3501;

	// ############################################################
	// -----
	// Activity Event Methods
	// -----
	// ############################################################

	public AVFacebook(AVUnityActivity activity,String appid){
		m_MainActivity = activity;
		APP_ID = appid;
		OpenSessionIfAlreadySignedIn();
		//genHashKeyForFacebook(activity);
	}

	public void onStart()
	{
		m_FacebookSession = Session.getActiveSession();
		if(m_FacebookSession != null){
			m_FacebookSession.addCallback(m_StatusCallback);

		}


	}

	/*public void genHashKeyForFacebook(Activity activity)
	{

		PackageInfo info;
		try {

			info = activity.getPackageManager().getPackageInfo(activity.getPackageName(), PackageManager.GET_SIGNATURES);

			for (Signature signature : info.signatures) {
				MessageDigest md = MessageDigest.getInstance("SHA1");
				md.update(signature.toByteArray());
				String hash = new String(Base64.encode(md.digest(), 0));
				System.out.println("hash key ="+ hash); Log.v("hash key", hash);

			}

		} catch (NameNotFoundException e) {

			Log.e("AVname not found", e.toString());

		}

		catch (NoSuchAlgorithmException e) {

			Log.e("AVno such an algorithm", e.toString());

		} catch (Exception e) {

			Log.e("AVexception", e.toString());

		}

	}*/

	public void onStop(){
		m_FacebookSession = Session.getActiveSession();
		if(m_FacebookSession != null){
			m_FacebookSession.removeCallback(m_StatusCallback);
		}
	}

	public void onActivityResult(int requestCode, int resultCode, Intent data){
		m_FacebookSession = Session.getActiveSession();
		if(m_FacebookSession != null){
			System.out.println("SESSION " + Session.getActiveSession());
			m_FacebookSession.onActivityResult(m_MainActivity, requestCode, resultCode, data);
		}
	}

	// ############################################################
	// -----
	// Interface Methods
	// -----
	// ############################################################

	public void login(){
		m_FacebookSession = Session.getActiveSession();
		if(m_FacebookSession == null){
			this.MigrateAccessToken();
		}

		if (m_FacebookSession != null && !m_FacebookSession.isOpened() && !m_FacebookSession.isClosed()){
			m_FacebookSession.openForRead(new Session.OpenRequest(m_MainActivity).setPermissions(Arrays.asList("basic_info")).setCallback(m_StatusCallback));
		} else {
			// If the user has never signed in to Facebook (or has signed out)
			OpenNewSessionAndAskToSignIn();
		}


	}

	public void OpenSessionIfAlreadySignedIn(){
		if(!this.MigrateAccessToken()){
			Session.openActiveSession(m_MainActivity, false, m_StatusCallback);
		}
	}

	public void OpenNewSessionAndAskToSignIn(){
		Session.openActiveSession(m_MainActivity, true, m_StatusCallback);
		m_FacebookSession = Session.getActiveSession();
	}

	public void logout(){
		m_FacebookSession = Session.getActiveSession();
		if(!m_FacebookSession.isClosed()){
			m_FacebookSession.closeAndClearTokenInformation();
		}
	}

	public boolean isLoggedIn(){
		if(m_FacebookSession != null){
			return m_FacebookSession.isOpened();
		}
		return false;
	}

	public String getUserID(){
		return m_CurrentUserID;
	}

	public String getUserName(){
		return m_CurrentUserName;
	}

	public void postMessage(String message){
		PublishFeedMessage(message);
	}


	private static final int PICK_FRIENDS_ACTIVITY = 1;
	public static final String BITLY_LINK = "http://bit.ly/1kICX6G";
	
	public void sendChallenge(final String title,final String message)
	{
		m_MainActivity.runOnUiThread(new Runnable()
		{

			@Override
			public void run()
			{
				Message m = sendChallengeHandler.obtainMessage();
				Bundle data = new Bundle();
				data.putString("title", title);
				data.putString("message", message);
				m.setData(data);
				sendChallengeHandler.sendMessage(m);
			}

		});

	}

	private final Handler sendChallengeHandler = new Handler()
	{
		@Override
		public void handleMessage(Message msg)
		{
			Bundle params = new Bundle();

			params.putString("message", msg.getData().getString("message"));
			params.putString("title", msg.getData().getString("title"));
			showDialogWithoutNotificationBar("apprequests", params);

		}
	};

	private WebDialog dialog;
	private String dialogAction;
	private Bundle dialogParams;
	private void showDialogWithoutNotificationBar(String action, Bundle params)
	{
		dialog = new WebDialog.Builder(m_MainActivity, Session.getActiveSession(),
				action, params).setOnCompleteListener(
						new WebDialog.OnCompleteListener()
						{
							@Override
							public void onComplete(Bundle values,
									FacebookException error)
							{
								if (error != null
										&& !(error instanceof FacebookOperationCanceledException))
								{
									/*
									 * (activity).showError( getResources().getString(
									 * R.string.network_error), false);
									 */
								}

								dialog = null;
								dialogAction = null;
								dialogParams = null;
							}
						}).build();

		Window dialog_window = dialog.getWindow();
		dialog_window.setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
				WindowManager.LayoutParams.FLAG_FULLSCREEN);

		dialogAction = action;
		dialogParams = params;

		dialog.show();
	}

	public void postScore(int score){
		if(!hasPublishPermissions()){
			if(score > m_ScoreToSend){ m_ScoreToSend = score; }
			m_FacebookSession.requestNewPublishPermissions(new Session.NewPermissionsRequest(m_MainActivity, PERMISSIONS));
			return;
		}
		System.out.println("starting send score");
		m_ScoreToSend = -1;
		m_MainActivity.runOnUiThread(new FacebookPostScoreRunnable(score));
	}

	public void getScores(){
		m_MainActivity.runOnUiThread(new FacebookFetchScoresRunnable());
	}

	public void RequestMissionCompletedAction(int missionIndex){
		m_MainActivity.runOnUiThread(new RequestMissionThread(missionIndex));
	}

	public void addAchievement(String achievementFileName){
		m_AchievementQueue.add(achievementFileName);
	}

	public void sendAchievementBatch(){
		if(!m_AchievementQueue.isEmpty()){
			m_MainActivity.runOnUiThread(new RequestAchievementThread());
		}
	}

	public boolean hasPublishPermissions(){
		if(m_FacebookSession == null) return false;
		return isSubsetOf(PERMISSIONS, m_FacebookSession.getPermissions());
	}

	public void requestPublishPermissions(){
		if(!hasPublishPermissions()){
			m_FacebookSession.requestNewPublishPermissions(new Session.NewPermissionsRequest(m_MainActivity, PERMISSIONS));
		}
	}

	// ############################################################
	// -----
	// UI Thread Runnables
	// -----
	// ############################################################

	private final class RequestPublishFeedBatchThread implements Runnable {
		@Override
		public void run()  {

			Bundle params = new Bundle();
			params.putString("caption", m_MessageQueue.poll());
			// params.putString("caption", "Build great social apps and get more installs.");
			// params.putString("description", "The Facebook SDK for Android makes it easier and faster to develop Facebook integrated Android apps.");
			params.putString("link", BITLY_LINK);
			//params.putString("picture", "https://raw.github.com/fbsamples/ios-3.x-howtos/master/Images/iossdk_logo.png");

			WebDialog feedDialog = (
					new WebDialog.FeedDialogBuilder(m_MainActivity,
							Session.getActiveSession(),
							params))
							.setOnCompleteListener(new OnCompleteListener() {

								@Override
								public void onComplete(Bundle values,
										FacebookException error) {
									if (error == null) {
										// When the story is posted, echo the success
										// and the post Id.
										final String postId = values.getString("post_id");
										if (postId != null) {
											// Posted
											AVUtility.MakeDialogBox("Facebook", "Successfully posted message to your feed!");
										} else {
											// User clicked the Cancel button
											//AVUtility.MakeDialogBox("Facebook", "You clicked cancel!");
										}
									} else {
										//AVUtility.MakeDialogBox("Facebook", "Posting Error: " + error.getMessage());
									}
								}
							})
							.build();
			feedDialog.show();

			//			if(m_MessageQueue.size() > 0){
			//
			//				RequestBatch batch = new RequestBatch();
			//				for(String message = m_MessageQueue.poll(); message != null; message = m_MessageQueue.poll()){
			//					Bundle postParams = new Bundle();
			//					postParams.putString("message",message);
			//					postParams.putString("link", "http://bit.ly/RBnqnX");
			//					batch.add(new Request(m_FacebookSession, "me/feed", postParams, HttpMethod.POST, new PostMessageCallback(message)));
			//				}
			//				batch.executeAsync();
			//			}
		}
	}

	private final class RequestMissionThread implements Runnable{
		private final int m_MissionIndex;
		public RequestMissionThread(int missionIndex){
			m_MissionIndex = missionIndex + 1;
		}
		@Override
		public void run() {
			String missionURL = "http://cdn.aceviral.com/mobile/angrygranrun/fb/missions/"+m_MissionIndex+".html";
			Bundle params = new Bundle();
			params.putString("mission_set", missionURL);
			new Request(m_FacebookSession, "me/angrygranrun:complete",params, HttpMethod.POST, new RequestMissionCallback()).executeAsync();
		}
	}

	private final class RequestAchievementThread implements Runnable{
		@Override
		public void run() {
			RequestBatch achievementBatch = new RequestBatch();
			String achievement = m_AchievementQueue.poll();
			while(achievement != null){
				String achievementURL = "http://cdn.aceviral.com/mobile/angrygranrun/fb/ach/"+achievement+".html";
				Bundle params = new Bundle();
				params.putString("achievement", achievementURL);
				achievementBatch.add(new Request(m_FacebookSession, "me/achievements", params, HttpMethod.POST, new AchievementCallback()));
				achievement = m_AchievementQueue.poll();
			}
			achievementBatch.executeAsync();
		}
	}

	// Allows us to run this on the UI Thread (which Facebook requests anyway)
	// We have our own so we can specify the score to upload in the constructor
	private final class FacebookPostScoreRunnable implements Runnable{
		private final int m_ScoreToPost;
		public FacebookPostScoreRunnable(int score){
			m_ScoreToPost = score;
		}

		@Override
		public void run() {
			System.out.println("posting score");
			Bundle fbParams = new Bundle();
			fbParams.putString("score", ""+m_ScoreToPost);
			Request.executeBatchAsync(new Request(Session.getActiveSession(), "me/scores", fbParams, HttpMethod.POST, new Request.Callback() {
				@Override
				public void onCompleted(Response response) {
					FacebookRequestError error = response.getError();
					if (error != null){
						HandleFacebookError(error);
					}
				}
			}));
		}
	}

	private final class FacebookFetchScoresRunnable implements Runnable {
		@Override
		public void run(){
			new Request(m_FacebookSession, APP_ID+"/scores", null, null, new RequestFriendScoresCallback()).executeAsync();
		}
	}

	// ############################################################
	// -----
	// Callback Methods
	// -----
	// ############################################################

	private class SessionStatusCallback implements Session.StatusCallback {
		@Override
		public void call(Session session, SessionState state,Exception exception) {
			onSessionStateChange(session, state, exception);
		}
	}

	private void onSessionStateChange(Session session, SessionState state, Exception e){
		UnityPlayer.UnitySendMessage("FacebookObject", "OnSessionStateChange", state.toString().toLowerCase(Locale.ENGLISH));
		if(session != null){
			switch(state){
			case OPENING:
				break;
			case OPENED:
				RequestUserData();
				break;
			case OPENED_TOKEN_UPDATED:
				if(hasPublishPermissions()){ // Stops asking for publish directly after read
					PublishFeedMessage(null);
					if(m_ScoreToSend > 0){
						postScore(m_ScoreToSend);
					}
				}
				break;
			case CREATED:
				break;
			case CREATED_TOKEN_LOADED:
				break;
			case CLOSED:
				break;
			case CLOSED_LOGIN_FAILED:
				break;
			}
		}
	}

	private class PostMessageCallback implements Request.Callback {
		private final String m_MessageSent;
		PostMessageCallback(String messageSent){
			m_MessageSent = messageSent;
		}

		@Override
		public void onCompleted(Response response) {
			FacebookRequestError error = response.getError();
			if(error != null){
				HandleFacebookError(error);
				UnityPlayer.UnitySendMessage("FacebookObject", "OnMessagePostedFailed", m_MessageSent);
			} else {
				UnityPlayer.UnitySendMessage("FacebookObject", "OnMessagePostedSuccess", m_MessageSent);
			}
		}
	}

	private final class RequestFriendScoresCallback implements Request.Callback {
		@Override
		public void onCompleted(Response response) {
			FacebookRequestError error = response.getError();
			if (error != null) {
				HandleFacebookError(error);
			} else if (m_FacebookSession == Session.getActiveSession()) {
				if (response != null) {
					JSONArray jScoreArray = (JSONArray) response.getGraphObject().getProperty("data");
					for (int i = 0; i < jScoreArray.length(); i++) {
						try {
							JSONObject jData = (JSONObject) jScoreArray.get(i);
							JSONObject jName = jData.getJSONObject("user");
							int score = (Integer) jData.get("score");
							String name = (String) jName.get("name");
							String id = (String) jName.get("id");
							UnityPlayer.UnitySendMessage("FacebookObject","OnFriendScoreRecieved", id + "#" + name + "#" + score);
						} catch (Exception e) {

						}
					}
					UnityPlayer.UnitySendMessage("FacebookObject","OnAllFriendScoreRecieved","");
				}
			}
		}
	}

	private class CurrentUserRequestCallback implements GraphUserCallback {
		@Override
		public void onCompleted(GraphUser user, Response response) {
			FacebookRequestError error = response.getError();
			if(error != null){
				Log.e("AVFacebook", "Request Mission onCompleted failed");
				HandleFacebookError(error);
			} else {
				if(user != null){
					m_CurrentUserID = user.getId();
					m_CurrentUserName = user.getName();
					UnityPlayer.UnitySendMessage("FacebookObject", "LoggedInFirer",
							"true");
				} else {
					Log.e("AVFacebook","CurrentUserRequestCallback received a NULL user!");
				}
			}
		}
	}

	private class RequestMissionCallback implements Request.Callback{
		@Override
		public void onCompleted(Response response) {
			FacebookRequestError error = response.getError();
			if(error != null){
				HandleFacebookError(error);
			}
		}
	}

	private class AchievementCallback implements Request.Callback{
		@Override
		public void onCompleted(Response response) {
			FacebookRequestError error = response.getError();
			if(error != null){
				HandleFacebookError(error);
			}
		}
	}

	// ############################################################
	// -----
	// Helper Methods
	// -----
	// ############################################################

	private boolean MigrateAccessToken(){
		SharedPreferences mPrefs = m_MainActivity.getPreferences(Context.MODE_PRIVATE);
		String access_token = mPrefs.getString("access_token", null);
		if(access_token != null){
			// Open Facebook with the old access token
			m_FacebookSession = new Session(m_MainActivity.getApplicationContext());
			m_FacebookSession.open(AccessToken.createFromExistingAccessToken(access_token, null, null, null, null), m_StatusCallback);
			Session.setActiveSession(m_FacebookSession);

			// Remove the old access token from Shared Prefs
			SharedPreferences.Editor editor = mPrefs.edit();
			editor.putString("access_token", null);
			editor.commit();
			return true;
		}
		return false;
	}

	private void RequestUserData(){
		Request.newMeRequest(m_FacebookSession, m_UserRequestCallback).executeAsync();
	}

	private void PublishFeedMessage(String message){
		// Add the message to the queue here in case there's no Facebook session or publish
		// permissions yet. This will be processed by a batch request below on the UI Thread.
		if(message != null && message.length() > 0){
			m_MessageQueue.add(message);
		}

		if(m_FacebookSession == null){
			Log.e("AVFacebook","Attempted to Publish Feed Message without an active Facebook session");
			AVUtility.MakeDialogBox("Facebook Error", "Unable to acquire an active Facebook session.");
			return;
		}

		if(!hasPublishPermissions()){
			requestPublishPermissions();
			return;
		}

		// Send all queued messages in a batch request
		m_MainActivity.runOnUiThread(new RequestPublishFeedBatchThread());
	}

	private boolean isSubsetOf(Collection<String> subset, Collection<String> superset) {
		for (String string : subset) {
			if (!superset.contains(string)) {
				return false;
			}
		}
		return true;
	}

	private void HandleFacebookError(FacebookRequestError error) {
		switch(error.getErrorCode()){
		case ACCESS_TOKEN_REQUIRED:
			Log.w("AVFacebook",error.getErrorMessage());
			break;
		case ACHIEVEMENT_ALREADY_EARNED:
			Log.w("AVFacebook", "Already earned achievement specified");
			break;
		default:
			Log.e("AVFacebook", "Error Occurred: " + error.toString());
		}
	}


}
