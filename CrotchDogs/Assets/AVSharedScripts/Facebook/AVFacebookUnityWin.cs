#if UNITY_WP8
using AVWindowsPhonePlugin;
#endif
public class AVFacebookUnityWin : AVFacebookUnity {

	protected override void Initialize()
	{
		base.Initialize();
	}
	

	public bool eventsAdded = false;    //only do this ONCE...
	public void AddEventsToPlugin()
	{
		if (!eventsAdded)
		{
						#if UNITY_WP8
		    AVWindowsPhonePlugin.FB.OnGotName += AVFacebookUnity.Instance.OnNameReceived;
		    AVWindowsPhonePlugin.FB.OnGotFriend += AVFacebookUnity.Instance.OnFriendScoreRecieved;
		    AVWindowsPhonePlugin.FB.OnGotFriendWithApp += AVFacebookUnity.Instance.OnFriendScoreRecievedWithApp;

			eventsAdded = true;
						#endif
		}
	}

	public override void SendMessageViaFriendPicker()
	{
		
	}

	public override void SendChallenge(string title, string message)
	{
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.PostMessage(title +  "#" + message);
				#endif
	}

	public override bool IsAvailableOnPlatform() {
		return true;
	}

	public override void Login() {

		//AVWindowsPhonePlugin.Flurry.Init("fasdf");
		print("JELLYSMASH IS CALLING WP8 LOGIN...");

				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.Login();
				#endif
	}

	public override bool IsLoggedIn() {
				#if UNITY_WP8
		return AVWindowsPhonePlugin.FB.IsLoggedIn();
				#else
				return false;
				#endif
		//return false;
	}

	public override string GetUserName() {
		return userName;//TODO
	}

	public void RequestPublish() {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.RequestPublish();
				#endif
	}

	public string GetAccessToken() {
				#if UNITY_WP8
		return AVWindowsPhonePlugin.FB.GetAccessToken();
				#else
				return "";
				#endif
		//return string.Empty;
	}

	public override void PostMessage(string msg) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.PostMessage(msg);
				#endif
	}

	public override void CovertPost(string msg) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.CovertPost(msg);
				#endif
	}

	public override void UpdateScoreAPIScore(int score) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.UpdateScoreAPIScore(score);
				#endif
	}

	public void DeleteNotifications (string requestType) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.DeleteNotifications(requestType);
				#endif
	}

	public void SendNotificationRequest (string IDs, string message, string data) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.SendNotificationRequest(IDs, message, data);
				#endif
	}

	public override void RetrieveFriendScores() {
				#if UNITY_WP8
				AVWindowsPhonePlugin.FB.RetrieveFriendScores();
				#endif
	}

	public override void RetrieveFriendsWithApp() {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.RetrieveFriendsWithApp();
				#endif
	}

	public void RetrieveAllFriends () {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.RetrieveAllFriends();
				#endif
	}

	public override void Like() {
		//Login();
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.Like();
				#endif
	}

	public override string GetUserID() {
		return userId;
		//return string.Empty;
	}

	public override void RequestMissionSetActivity(int missionSet) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.RequestMissionSetActivity(missionSet);
				#endif
	}

	public void RequestMissionSetActivity(string fnName, int missionSet, string value) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.RequestMissionSetActivity(fnName, missionSet, value);
				#endif
	}

	public override void AddAchievementToBatch(string achievementName) {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.AddAchievementToBatch(achievementName);
				#endif
	}

	public override void SendAchievementBatch() {
				#if UNITY_WP8
		AVWindowsPhonePlugin.FB.SendAchievementBatch();
				#endif
	}
}
