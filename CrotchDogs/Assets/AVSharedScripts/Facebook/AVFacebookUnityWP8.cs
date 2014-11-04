#if UNITY_WP8
using AVWindowsPhonePlugin;



public class AVFacebookUnityWP8 : AVFacebookUnity {

	protected override void Initialize()
	{
		AddEventsToPlugin();
		AVWindowsPhonePlugin.FB.onAVFriendsFound += ROFLCOPTER;
		base.Initialize();
	}

	private void ROFLCOPTER()
	{
		AVDebug.Log("fire friends found");
		FireFacebookFriendsLoaded();
	}


	public bool eventsAdded = false;    //only do this ONCE...
	public void AddEventsToPlugin()
	{
		if (!eventsAdded)
		{
		    AVWindowsPhonePlugin.FB.OnGotName += AVFacebookUnity.Instance.OnNameReceived;
		    AVWindowsPhonePlugin.FB.OnGotFriend += AVFacebookUnity.Instance.OnFriendScoreRecieved;
		    AVWindowsPhonePlugin.FB.OnGotFriendWithApp += AVFacebookUnity.Instance.OnFriendScoreRecievedWithApp;

			eventsAdded = true;
		}
	}

	public override void SendMessageViaFriendPicker()
	{
		
	}

	public override void SendChallenge(string title, string message)
	{
		AVWindowsPhonePlugin.FB.PostMessage(title +  " " + message);
	}

	public override bool IsAvailableOnPlatform() {
		return true;
	}

	public override void Login() {

		//AVWindowsPhonePlugin.Flurry.Init("fasdf");
		print("JELLYSMASH IS CALLING WP8 LOGIN...");
		AVWindowsPhonePlugin.FB.Login();
	}

	public override bool IsLoggedIn() {
		return AVWindowsPhonePlugin.FB.IsLoggedIn();
		//return false;
	}

	public override string GetUserName() {
		return userName;//TODO
	}

	public void RequestPublish() {
		AVWindowsPhonePlugin.FB.RequestPublish();
	}

	public string GetAccessToken() {
		return AVWindowsPhonePlugin.FB.GetAccessToken();
		//return string.Empty;
	}

	public override void PostMessage(string msg) {
		AVWindowsPhonePlugin.FB.PostMessage(msg);
	}

	public override void CovertPost(string msg) {
		AVWindowsPhonePlugin.FB.CovertPost(msg);
	}

	public override void UpdateScoreAPIScore(int score) {
		AVWindowsPhonePlugin.FB.UpdateScoreAPIScore(score);
	}

	public void DeleteNotifications (string requestType) {
		AVWindowsPhonePlugin.FB.DeleteNotifications(requestType);
	}

	public void SendNotificationRequest (string IDs, string message, string data) {
		AVWindowsPhonePlugin.FB.SendNotificationRequest(IDs, message, data);
	}

	public override void RetrieveFriendScores() {
		AVWindowsPhonePlugin.FB.RetrieveFriendScores();
	}

	public override void RetrieveFriendsWithApp() {
		AVWindowsPhonePlugin.FB.RetrieveFriendsWithApp();
	}

	public void RetrieveAllFriends () {
		AVWindowsPhonePlugin.FB.RetrieveAllFriends();
	}

	public override void Like() {
		//Login();
		AVWindowsPhonePlugin.FB.Like();
	}

	public override string GetUserID() {
		return AVWindowsPhonePlugin.FB.GetUserID();
		//return string.Empty;
	}

	public override void RequestMissionSetActivity(int missionSet) {
		AVWindowsPhonePlugin.FB.RequestMissionSetActivity(missionSet);
	}

	public void RequestMissionSetActivity(string fnName, int missionSet, string value) {
		AVWindowsPhonePlugin.FB.RequestMissionSetActivity(fnName, missionSet, value);
	}

	public override void AddAchievementToBatch(string achievementName) {
		AVWindowsPhonePlugin.FB.AddAchievementToBatch(achievementName);
	}

	public override void SendAchievementBatch() {
		AVWindowsPhonePlugin.FB.SendAchievementBatch();
	}
}
#endif