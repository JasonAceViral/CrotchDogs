using System.Collections;
using UnityEngine;

public class AVUnityFacebookAndroid : AVFacebookUnity {
	private bool m_WasWaitingOnLoginToLike = false;
	private bool m_WaitForLikePostToReward = false;

	void Start() {
		this.gameObject.name = "FacebookObject";
	}

	public void LoggedInFirer(string s)
	{
		FireFacebookLogin();
	}

	public override void Login() {
		AVAndroidInterface.Facebook.User.Login();
	}

	public override bool IsLoggedIn() {
		return AVAndroidInterface.Facebook.User.IsLoggedIn();
	}

    public override void PostMessage(string msg) {
        AVAndroidInterface.Facebook.Feed.Post(msg);
    }

	public override void CovertPost(string msg) {
		if (IsLoggedIn()) {
			AVAndroidInterface.Facebook.Feed.Post(msg);
		}
	}

	public override void Like() {
		if (HasRecievedFacebookReward()) {
			return;
		}

		if (!IsLoggedIn()) {
			m_WasWaitingOnLoginToLike = true;
			Login();
		} else {
			SetRecievedFacebookReward();
			OnGivenFacebookLoginReward();
		}
	}

	public override void UpdateScoreAPIScore(int score) {
#if UNITY_ANDROID
		if(IsLoggedIn()){
			if(AVFacebook.Scores.GetUser() != null)
			{
				if(score > AVFacebook.Scores.GetUser().Score)
				{
                    //if (AVAndroidInterface.Facebook.Permissions.HasPublishPermissions())
					{
						AVAndroidInterface.Facebook.Scores.Send(score);
					}
//					else
//					{
//                        Debug.Log("starting send scores update show not have permission");
//						AVMsgBox.Show("Facebook", "To share your score with your friends, we must also request publish permissions.", AVMsgBoxType.OK, (response) =>
//						{
//                            Debug.Log("starting send scores update send after not have permission");
//							AVAndroidInterface.Facebook.Scores.Send(score);
//						});
//					}
				}
			}
			RetrieveFriendScores();
		}
#endif
	}

	public override void RetrieveFriendScores() {
		StartCoroutine(RetrieveFriendScoresAsync());
	}

	private IEnumerator RetrieveFriendScoresAsync() {
		AVAndroidInterface.Facebook.Scores.Get();
		yield break;
	}


	public void OnAllFriendScoreRecieved(string message)
	{
		FireFacebookFriendsLoaded();
	}

	public override string GetUserID() {
		return AVAndroidInterface.Facebook.User.GetUserID();
	}

	public override void SendMessageViaFriendPicker()
	{
	}

	public override void SendChallenge(string title, string message)
	{
		AVAndroidInterface.Facebook.User.SendChallenge(title,message);
	}
	
	public override string GetUserName() {
		return AVAndroidInterface.Facebook.User.GetUserName();
	}

	#region Callbacks

	public void OnSessionStateChange(string state) {

		Debug.Log("state changed");
		if (state == "opened") {

			Debug.Log("logged in at AVFacebookunityandroid");
			//if (IsLoggedIn())
			//{
			//    FireFacebookLogin();
			//}
			AVFacebook.Scores.DownloadAllProfilePictures();
			SendFBAchievements();
			if (m_WasWaitingOnLoginToLike)
			{
				if (AVAndroidInterface.Facebook.Permissions.HasPublishPermissions())
				{
					m_WasWaitingOnLoginToLike = false;
					SetRecievedFacebookReward();
					OnGivenFacebookLoginReward();
				}
				else
				{
					AVMsgBox.Show("Facebook", "To share your score with your friends, we must also request publish permissions.", AVMsgBoxType.OK, (response) =>
					{
						m_WasWaitingOnLoginToLike = false;
						SetRecievedFacebookReward();
						OnGivenFacebookLoginReward();
					});
				}
			}
		}

	}

	public void OnMessagePostedSuccess(string message) {
		if (m_WaitForLikePostToReward) {
			m_WaitForLikePostToReward = false;
			SetRecievedFacebookReward();
			OnGivenFacebookLoginReward();
		} else {
			AVMsgBox.Show("Facebook", "Posted message successfully: " + message);
		}
	}

	public void OnMessagePostedFailed(string message) {

	}

    public override void RequestMissionSetActivity(int missionSet) { 
	}

	#endregion

	public override void AddAchievementToBatch(string achievementName) {
#if UNITY_ANDROID
		AVAndroidInterface.Facebook.Achievements.Add(achievementName);
#endif
	}

	public override void SendAchievementBatch() {
#if UNITY_ANDROID
		AVAndroidInterface.Facebook.Achievements.SendBatch();
#endif
	}

	public override bool IsAvailableOnPlatform() {
		return true;
	}
}
