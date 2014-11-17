using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

namespace AceViral {
	public class AVFacebook { // Do not refactor this to 'Facebook', please. This class refers to external Facebook object!!!

		// =-=-=-=-=-=-=-=
		// Constants
		// =-=-=-=-=-=-=-=

		private const string prefkey_FirstSignIn = "FBFirstSignIn";
		private const string prefkey_LastPostTime = "FBLastPostTime";
		private const string prefkey_LastInviteTime = "FBLastInviteTime";
		private const string prefkey_AllUserData = "FBAllFriendData";
		private const string prefkey_ProfilePicPrefix = "FBProfilePic_";

		// =-=-=-=-=-=-=-=
		// Vars
		// =-=-=-=-=-=-=-=

		private bool m_IsGameViewObscured;
		private List<AVFacebookFriend> m_FriendList;
		private AVFacebookFriend m_CurrentUserProfile;
		private bool m_RequestingFriendAndPics = false;
		private bool m_RequestingFriendScores = false;
		private bool m_HasUserSignedInPreviously = false;
		private bool m_IsLoggingIn = false;
		private bool m_CheckFacebookLikeOnceLoggedIn = false;


		// =-=-=-=-=-=-=-=
		// Singleton
		// =-=-=-=-=-=-=-=

		private static AVFacebook _Instance;
		public static AVFacebook Instance{
			get{
				if (_Instance == null) {
					_Instance = new AVFacebook ();
				}
				return _Instance;
			}
		}

		public AVFacebook()
		{
			if (m_FriendList == null)
			{
				m_FriendList = new List<AVFacebookFriend>();
			}
		}

		// =-=-=-=-=-=-=-=
		// Events
		// =-=-=-=-=-=-=-=

		public Action<bool> OnLoggedIn;
		private void SendOnLoggedInEvent(bool loginSuccess){
			if(loginSuccess)
				LoadFriendsFromPrefs();
			if (OnLoggedIn != null) {
				OnLoggedIn (loginSuccess);
			}
		}

		/// <summary>
		/// It's advisable to only hook into your code once to make sure you don't give the user two rewards, etc.
		/// Hence why it's not sent with OnLoggedIn; it's useful to have that event hooked in multiple places (e.g. multiple menus)
		/// </summary>
		public Action OnFirstSignIn;
		private void SendOnFirstSignInEvent(){
			if (OnFirstSignIn != null) {
				OnFirstSignIn ();
			}
		}

		public Action OnLoggedOut;
		private void SendOnLoggedOutEvent(){
			if (OnLoggedOut != null) {
				OnLoggedOut ();
			}
		}

		public Action OnPostSuccess;
		private void SendOnPostSuccessEvent(){
			if (OnPostSuccess != null) {
				OnPostSuccess ();
			}
		}

		public Action OnInviteSuccess;
		private void SendOnInviteSuccessEvent(){
			if (OnInviteSuccess != null) {
				OnInviteSuccess ();
			}
		}

		public Action OnFacebookPageLiked;
		private void SendFacebookPageLikedSuccessEvent(){
			if (OnFacebookPageLiked != null) {
				OnFacebookPageLiked ();
			}
		}

		public Action OnFacebookPageNotLiked;
		private void SendFacebookPageNotLikedEvent(){
			if (OnFacebookPageNotLiked != null) {
				OnFacebookPageNotLiked ();
			}
		}

		public Action<bool> OnGameViewObscured;
		private void SendOnGameViewObscuredEvent(bool flag){
			if (OnGameViewObscured != null) {
				OnGameViewObscured (flag);
			}
		}

		public Action<AVFacebookFriend[]> OnScoresReceived;
		private void SendOnScoresReceivedEvent(AVFacebookFriend[] scores){
			if (OnScoresReceived != null) {
				OnScoresReceived (scores);
			}
		}

		// =-=-=-=-=-=-=-=
		// Core
		// =-=-=-=-=-=-=-=

		public void Initialize(){
			FB.Init (OnInitializationComplete, OnHideUnity);
						m_HasUserSignedInPreviously = ((PlayerPrefs.GetInt (prefkey_FirstSignIn, 0)==1));
		}

		private void OnInitializationComplete(){
			if (IsLoggedIn ()) 
			{
				SendOnLoggedInEvent (true);
			}
		}

		private void OnHideUnity(bool isGameShown){
			m_IsGameViewObscured = !isGameShown;
			SendOnGameViewObscuredEvent (!isGameShown);
		}

		public void Login(){
			if (!IsLoggedIn() && !m_IsLoggingIn) {
				m_IsLoggingIn = true;

				FB.Login ("publish_actions, user_games_activity, user_likes, user_friends", OnLoginRequestFinished);
			}
		}

		private void OnLoginRequestFinished(FBResult result){
			m_IsLoggingIn = false;
			if (result.Error != null && result.Error != string.Empty) {
				Debug.LogError ("Facebook Login Error: " + result.Error);
				return;
			}

			SendOnLoggedInEvent (IsLoggedIn());
			if (IsLoggedIn ()) {
				CheckForFirstSignIn ();

				if (m_CheckFacebookLikeOnceLoggedIn)
				{
					CompleteGetUserLikesRequest();
					m_CheckFacebookLikeOnceLoggedIn = false;
				}
			}
		}

		public bool IsLoggedIn(){
			return FB.IsLoggedIn;
		}

		public void LogOut(){
			FB.Logout ();
			RemoveAllFriendScores ();
			SendOnLoggedOutEvent ();
		}

		public string GetCurrentUserId(){
			return FB.UserId;
		}

		public bool IsGameViewObscured(){
			return m_IsGameViewObscured;
		}

		/// <summary>
		/// Determines whether the user has signed in previously. Useful for displaying different login buttons (e.g. rewards).
		/// </summary>
		/// <returns><c>true</c> if this instance has user previously signed in; otherwise, <c>false</c>.</returns>
		public bool HasUserPreviouslySignedIn(){
			return m_HasUserSignedInPreviously;
		}

		private void CheckForFirstSignIn(){
			if (!m_HasUserSignedInPreviously) {
								PlayerPrefs.SetInt (prefkey_FirstSignIn, 1);
				m_HasUserSignedInPreviously = true;
				SendOnFirstSignInEvent ();
			}
		}

		public bool HasPostedToday(){
			try{
								return (DateTime.Now.Date - DateTime.Parse (PlayerPrefs.GetString (prefkey_LastPostTime)).Date).Days <= 0;
			} catch (Exception e){
				AVDebug.DumpException (e);
				return false;
			}
		}

		public bool HasInvitedFriendsToday(){
			try{
								return (DateTime.Now.Date - DateTime.Parse (PlayerPrefs.GetString (prefkey_LastInviteTime)).Date).Days <= 0;
			} catch (Exception e){
				AVDebug.DumpException (e);
				return false;
			}
		}

		// =-=-=-=-=-=-=-=
		// Sharing
		// =-=-=-=-=-=-=-=

		public void OpenPostDialog(string name, string caption){
			FB.Feed (linkName: name, linkCaption: caption, callback: OnPostDialogRequestFinished, link:"http://aceviral.com/mobile/links/games/batman");
		}

		private void OnPostDialogRequestFinished(FBResult result){
			if (string.IsNullOrEmpty (result.Error)) {
				if (result.Text.Contains ("cancelled") && result.Text.Contains ("true")) {
					Debug.LogError ("User cancelled facebook post: " + result.Text);
				} else {
					SendOnPostSuccessEvent ();
										PlayerPrefs.SetString (prefkey_LastPostTime, DateTime.Now.Date.ToString());
				}
			} else {
				Debug.LogError ("Error while facebooked: " + result.Error);
			}
		}

		// =-=-=-=-=-=-=-=
		// Invitations
		// =-=-=-=-=-=-=-=

		public void OpenInviteDialog(string title, string message){
			FB.AppRequest(message, null, null, null, null, "", title, OnInviteDialogRequestFinished);
		}

		private void OnInviteDialogRequestFinished(FBResult result){
			Debug.Log ("OnInviteDialogRequestFinished: " + result.Text);
			if (string.IsNullOrEmpty (result.Error)) {
				if (result.Text.Contains ("cancelled") && result.Text.Contains ("true")) {
					Debug.LogError ("User cancelled facebook invite: " + result.Text);
				} else {
					SendOnInviteSuccessEvent ();
										PlayerPrefs.SetString (prefkey_LastInviteTime, DateTime.Now.Date.ToString());
				}
			} else {
				Debug.LogError ("Error while facebooked: " + result.Error);
			}
		}

		// =-=-=-=-=-=-=-=
		// Scores
		// =-=-=-=-=-=-=-=

		/// <summary>
		/// Publishs the new score. Requires that the user has already downloaded their score from FB to prevent any unwanted overwriting.
		/// </summary>
		/// <param name="score">Score to submit.</param>
		/// <param name="isHigherScoreBetter">If set to <c>true</c> a higher score than current will be submitted.</param>
		public void PublishNewScore(int score, bool isHigherScoreBetter){
			if (IsLoggedIn ()) {
				int currentScore = GetCurrentUserScore ();
				if(currentScore >= 0){
					if ((isHigherScoreBetter && currentScore < score) || (!isHigherScoreBetter && currentScore > score)) {
						m_CurrentUserProfile.Score = score;
						Dictionary<string, string> queryDictionary = new Dictionary<string, string> ();
						queryDictionary ["score"] = score.ToString ();
						FB.API("/me/scores", Facebook.HttpMethod.POST, OnPublishScoreRequestFinished, queryDictionary);
					}
				}
			}
		}

		/// <summary>
		/// Publishs the new score. Forces Facebook to accept any value given, so scores can be overwritten with better or worse scores.
		/// Use with caution; it's recommended to use PublishNewScore(int score, bool isHigherScoreBetter) instead to prevent 
		/// overwriting with a worse score.
		/// </summary>
		/// <param name="score">Score to publish</param>
		public void PublishNewScoreForced(int score){
			if (IsLoggedIn ()) {
				m_CurrentUserProfile.Score = score;
				Dictionary<string, string> queryDictionary = new Dictionary<string, string> ();
				queryDictionary ["score"] = score.ToString ();
				FB.API("/me/scores", Facebook.HttpMethod.POST, OnPublishScoreRequestFinished, queryDictionary);
			}
		}

		/// <summary>
		/// Returns -1 if the player's score hasn't downloaded yet
		/// </summary>
		/// <returns>The current user score.</returns>
		public int GetCurrentUserScore(){
			if (m_CurrentUserProfile != null) {
				return m_CurrentUserProfile.Score;
			}
			if (FindCurrentUsersProfile () != null) {
				return m_CurrentUserProfile.Score;
			}
			return -1;
		}

		private void OnPublishScoreRequestFinished(FBResult r){
			//Debug.Log("Result: " + r.Text);
			SortFriendScores();
			SendOnScoresReceivedEvent (m_FriendList.ToArray());
		}

		/// <summary>
		/// Asyncronously gets the current user's friend list with scores. Listen with OnScoresReceived to receive
		/// the scores when they come through.
		/// </summary>
		public void RequestNewFriendScores(){
			if (IsLoggedIn () && !m_RequestingFriendScores) {
				m_RequestingFriendScores = true;
				FB.API (FB.AppId + "/scores", Facebook.HttpMethod.GET, (result) => {
					m_RequestingFriendScores = false;
					if (result.Error != null && result.Error != string.Empty) {
						Debug.LogError ("Facebook Request Scores Error: " + result.Error + " TEXT: " + result.Text);
						return;
					}
					JSONNode users = JSON.Parse (result.Text) ["data"];
					foreach (JSONNode node in users.Childs) {
						UpdateFriendListWithUserJSONData (node);
					}

					#if UNITY_EDITOR
					UpdateFriendData ("Test A", "7", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test B", "11", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test C", "12", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test D", "10", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test E", "4", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test F", "5", UnityEngine.Random.Range (0, 99999));
					UpdateFriendData ("Test G", "6", UnityEngine.Random.Range (0, 99999));
					#endif

					SortFriendScores();
					CheckForProfilePictureToDownload ();

					if (m_FriendList != null && m_FriendList.Count > 0) {
						SendOnScoresReceivedEvent (m_FriendList.ToArray ());
					}
					SendOnScoresReceivedEvent (null);			
				});
			} else {
				Debug.LogWarning ("Facebook Error: Attempted to get scores whilst not logged in. / Already requesting scores");
			}
		}

		private void SortFriendScores(){
			m_FriendList.Sort (delegate(AVFacebookFriend p1, AVFacebookFriend p2) {
				return p2.Score.CompareTo (p1.Score);
			});
		}

		public void LikeFacebookPage(){

			Application.OpenURL("https://www.facebook.com/" + AppConstants.Facebook.PageId);
		}

		/// <summary>
		/// Asyncronously gets the current user's friend list with scores. Listen with OnScoresReceived to receive
		/// the scores when they come through.
		/// </summary>
		public void RequestGetUserLikes(){
			if (IsLoggedIn ()) 
			{
				FB.API ("me/permissions", Facebook.HttpMethod.GET, (result) => {

					if (result.Error != null && result.Error != string.Empty) {
						Debug.LogError ("Facebook Get Permissions Error: " + result.Error + " TEXT: " + result.Text);
						return;
					}

					JSONNode data = JSON.Parse (result.Text) ["data"];
					foreach (JSONNode node in data.Childs) 
					{
						Debug.Log("Facebook Permissions Request Node: " + node.ToString());

						JSONNode perm = JSON.Parse (result.Text) ["permission"];
						if(perm.ToString() == "user_likes")
						{
							JSONNode status = JSON.Parse (result.Text) ["status"];
							if(status.ToString() == "granted")
							{
								Debug.Log("Facebook found likes permission granted");

								CompleteGetUserLikesRequest();
								return;
							}
						}
					}   

					m_CheckFacebookLikeOnceLoggedIn = true;
					Debug.Log("Facebook no permissions found for likes. Requesting permissions!");
					FB.Login ("publish_actions, user_games_activity, user_likes", OnLoginRequestFinished);
				});        
			} 
			else 
			{
				m_CheckFacebookLikeOnceLoggedIn = true;
				Debug.LogWarning("Facebook Error: Attempted to user's Likes whilst not logged in. ");
				Login();
			}
		}

		void CompleteGetUserLikesRequest()
		{
			FB.API ("me/likes/" + AppConstants.Facebook.PageId, Facebook.HttpMethod.GET, (result) => {

				if (result.Error != null && result.Error != string.Empty) {
					Debug.LogError ("Facebook Request Likes Error: " + result.Error + " TEXT: " + result.Text);
					return;
				}

				Debug.Log("Facebook Like Result: " + result.Text);

				JSONNode users = JSON.Parse (result.Text) ["data"];
				foreach (JSONNode node in users.Childs) {
					if(node != null){
						Debug.Log("Facebook User has liked page");
						SendFacebookPageLikedSuccessEvent();
						return;
					}
				}   

				Debug.Log("Facebook User has not liked page");
				SendFacebookPageNotLikedEvent();
			});
		}

		/// <summary>
		/// Checks for any profile pictures that haven't yet downloaded, then asyncronously downloads them. 
		/// If a picture fails to download then it's marked as failed and continues to the next until the list is complete.
		/// You can listen for AVFacebookFriend.OnProfilePictureDownloaded to get notified of any complete downloads.
		/// </summary>
		public void CheckForProfilePictureToDownload(){
			for (int i = 0; i < m_FriendList.Count; i++) 
			{
				//Debug.Log("Friend: " + m_FriendList[i].Name + " + " + m_FriendList[i].ProfilePicture + " failed? " + m_FriendList[i].DownloadFailed);
				if (m_FriendList [i].ProfilePicture == null && !m_FriendList [i].DownloadFailed) 
				{
					FB.API(query:GetPictureURL(m_FriendList [i].ID, AppConstants.Facebook.PreferredPicSize, AppConstants.Facebook.PreferredPicSize, null), method: Facebook.HttpMethod.GET, 
						callback: ((FBResult result) => 
							{
								if(result.Error != null && result.Error != string.Empty)
								{
									m_FriendList[i].DownloadFailed = true;
									Debug.LogWarning ("Download failed for: " + m_FriendList[i].Name);
								}
								else if(result.Texture != null)
								{
									m_FriendList[i].ProfilePicture = result.Texture;
									CacheProfilePictureForUser(m_FriendList[i]);
									if(m_FriendList[i].OnProfilePictureDownloaded != null)
									{
										m_FriendList[i].OnProfilePictureDownloaded(m_FriendList[i].ProfilePicture);
									}
								} else 
								{
									Debug.LogError ("Picture was null for: " + m_FriendList[i].Name);
								}
								CheckForProfilePictureToDownload();
							}));
					return;
				}
			}

			FindCurrentUsersProfile ();
			SaveFriendsToPrefs();
		}

		private void UpdateFriendListWithUserJSONData(JSONNode eventNode){
			if (eventNode != null) {
				string name = eventNode ["name"].Value;
				string userID = eventNode ["id"].Value;
				int score = 0;
				int.TryParse (eventNode ["score"].Value, out score);

				UpdateFriendData (name, userID, score);
			} else {
				Debug.LogError ("Facebook Request Scores Error: JSON Node was null/empty");
			}
		}

		private void UpdateFriendData(string name, string userID, int score){
			if (userID != null && userID != string.Empty) {
				for (int i = 0; i < m_FriendList.Count; i++) {
					if (m_FriendList [i].ID.CompareTo (userID) == 0) {
						if (score > m_FriendList [i].Score) {
							m_FriendList [i].Score = score;
						}
						return;
					}
				}
				Debug.Log("Adding Facebook friend '" + name + "'");
				m_FriendList.Add (new AVFacebookFriend (userID, name, score));

			}
		}

		private AVFacebookFriend FindCurrentUsersProfile(){
			if (m_FriendList != null) {
				for (int i = 0; i < m_FriendList.Count; i++) {
					if(m_FriendList[i].ID == FB.UserId){
						m_CurrentUserProfile = m_FriendList [i];
						return m_CurrentUserProfile;
					}
				}
			}
			return null;
		}

		public bool HasFriendScoresAvailable(){
			if (m_FriendList != null && m_FriendList.Count > 0) {
				return true;
			}
			return false;
		}

		public AVFacebookFriend[] GetCachedFriendScores(){
			if (m_FriendList != null && m_FriendList.Count > 0) {
				return m_FriendList.ToArray ();
			}
			return null;
		}

		public void RemoveAllFriendScores(){
			if (m_FriendList != null && m_FriendList.Count > 0) {
				m_FriendList.Clear ();
				m_FriendList = null;
			}
		}

		public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
		{
			string url = string.Format("/{0}/picture", facebookID);
			string query = width != null ? "&width=" + width.ToString() : "";
			query += height != null ? "&height=" + height.ToString() : "";
			query += type != null ? "&type=" + type : "";
			if (query != "") url += ("?g" + query);
			return url;
		}

		public void GetFriendsAndProfilePictures()
		{
			if (IsLoggedIn () && !m_RequestingFriendAndPics) {
				m_RequestingFriendAndPics = true;
				FB.API ("me/friends", Facebook.HttpMethod.GET, (result) => {
					m_RequestingFriendAndPics = false;
					if (result.Error != null && result.Error != string.Empty) {
						Debug.LogError ("Facebook Request Friends Error: " + result.Error + " TEXT: " + result.Text);
						return;
					}

					Debug.Log("Result: " + result.Text);

					JSONArray userArray = (JSONArray)JSON.Parse (result.Text) ["data"];
					foreach (JSONNode node in userArray.Childs) {
						UpdateFriendListWithUserJSONData (node);
					}

					#if UNITY_EDITOR
					UpdateFriendData ("Test A", "7", 0);
					UpdateFriendData ("Test B", "11", 0);
					UpdateFriendData ("Test C", "12", 0);
					UpdateFriendData ("Test D", "10", 0);
					UpdateFriendData ("Test E", "4", 0);
					UpdateFriendData ("Test F", "5", 0);
					UpdateFriendData ("Test G", "6", 0);
					#endif

					CheckForProfilePictureToDownload ();
				});
			} else {
				Debug.LogWarning ("Facebook Error: Attempted to get friends and profile pictures whilst not logged in.");
			}
		}

		private void CacheProfilePictureForUser(AVFacebookFriend friend)
		{
			#if !UNITY_WEBPLAYER
			string cachePath = Path.Combine(Application.persistentDataPath, "FacebookPP/");
			Directory.CreateDirectory(cachePath);

			string savePath = cachePath + friend.ID;
			FileStream cache = new FileStream(savePath, FileMode.Create);
			BinaryWriter w = new BinaryWriter(cache);
			w.Write(friend.ProfilePicture.EncodeToPNG());
			w.Close();
			cache.Close();
			#endif
		}

		private void LoadFriendsFromPrefs()
		{
			if (m_FriendList == null)
			{
				m_FriendList = new List<AVFacebookFriend>();
			}
			else if (m_FriendList.Count > 0)
			{
				m_FriendList.Clear();
			}

						string rawFriendData = PlayerPrefs.GetString(prefkey_AllUserData);

			if (!string.IsNullOrEmpty(rawFriendData))
			{
				string[] eachFriend = rawFriendData.Split(new string[]{ "||" }, StringSplitOptions.None);

				for (int i = 0; i < eachFriend.Length-1; i++)
				{
					string[] friendData = eachFriend[i].Split(new string[]{ "|" }, StringSplitOptions.None);
					bool profilePicDownloaded = bool.Parse(friendData[2]);

					AVFacebookFriend friend = new AVFacebookFriend(friendData[0], friendData[1], 0);

					#if !UNITY_WEBPLAYER
					if(profilePicDownloaded)
					{
						string loadPath = Path.Combine(Application.persistentDataPath, "FacebookPP/" + friend.ID);

						if (File.Exists(loadPath))
						{
							byte[] imageBytes = System.IO.File.ReadAllBytes(loadPath);
							Texture2D imageTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
							imageTex.LoadImage(imageBytes);

							// Sometimes our ad images come in corrupted so check for this
							if (imageTex != null && imageTex.width > 20 && imageTex.height > 20)
							{
								friend.ProfilePicture = imageTex;
							}
							else
							{
								Debug.LogError("AVFacebook.RecoverProfilePicFromCache - Error loading image: " + loadPath);
							}
						}
					}
					#endif
					m_FriendList.Add(friend);
				}

				Debug.Log("Facebook friend info recovered from cache.");
				FindCurrentUsersProfile ();
			}

			GetFriendsAndProfilePictures();
		}

		private void SaveFriendsToPrefs()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			foreach (AVFacebookFriend friend in m_FriendList)
			{
				bool profilePicDownloaded = friend.ProfilePicture != null;
				sb.AppendFormat("{0}|{1}|{2}||", friend.ID, friend.Name, profilePicDownloaded);
			}

			PlayerPrefs.SetString(prefkey_AllUserData, sb.ToString());
			PlayerPrefs.Save();
			Debug.Log("Facebook friend info has been cached.");
		}

		// =-=-=-=-=-=-=-=
		// Custom Actions
		// =-=-=-=-=-=-=-=
	}
}
