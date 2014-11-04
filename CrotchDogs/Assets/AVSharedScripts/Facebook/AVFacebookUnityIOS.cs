#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AVFacebookUnityIOS : AVFacebookUnity {
	
	[DllImport ("__Internal")] private static extern void _avFacebookOpenSession();
	[DllImport ("__Internal")] private static extern void _avFacebookLogin( string appID );
	[DllImport ("__Internal")] private static extern bool _avFacebookIsLoggedIn();
    [DllImport ("__Internal")] private static extern void _avFacebookPostMessage( string message, bool showConfirmDialog ,string bitly);
	[DllImport ("__Internal")] private static extern void _avFacebookSubmitNewScore( int score );
	[DllImport ("__Internal")] private static extern void _avFacebookRequestScores( );
	
	[DllImport ("__Internal")] private static extern void _avFacebookRequestMissionSet( int mSet );
	[DllImport ("__Internal")] private static extern void _avFacebookAddAchievementToBatch( string achievement );
	[DllImport ("__Internal")] private static extern void _avFacebookSendAchievementBatch( );

	private bool m_WaitingOnLoginToLike = false;
	private bool m_WaitingOnLogin = false;

	private string m_UserID = string.Empty;
	private string m_UserName = string.Empty;

    private string bitly = "http://bit.ly/1kICX6G";
	
	void Start()
	{
		this.gameObject.name = "FacebookObject";
		_avFacebookOpenSession();
	}

	public override void SendMessageViaFriendPicker()
	{
			//AVAndroidInterface.Facebook.User.SendMessageViaFriendPicker();
	}


	// Login
	public override void Login ()
	{
		_avFacebookLogin(AppID);
	}
	
	// Logged In	
	public override bool IsLoggedIn ()
	{
		return _avFacebookIsLoggedIn();
	}

    public void OnAllFriendScoreRecieved(string message)
    {
        FireFacebookFriendsLoaded();
    }


    public override void PostMessage (string msg)
    {
        _avFacebookPostMessage(msg, true,bitly);
    }
	
	public override void Like ()
	{
		Login();
		m_WaitingOnLoginToLike = true;
	}

	public override void CovertPost (string msg) {
		if(IsLoggedIn()){
            _avFacebookPostMessage(msg, false,bitly);	
		}
	}
	
	public void onFacebookStateOpen(string message){
		if(m_WaitingOnLogin){
			m_WaitingOnLogin = false;	
            FireFacebookLogin();
		}
		if(m_WaitingOnLoginToLike){
			m_WaitingOnLoginToLike = false;
			//CovertPost("I'm playing Angry Gran Run! Get it yourself");
			SetRecievedFacebookReward ();
			OnGivenFacebookLoginReward ();
		}
	}

	public void onFacebookStateClosed(string message){
		
	}

	public void onFacebookStateFailedLogin(string message){
    
	}
	
	public override void RetrieveFriendScores ()
	{
		_avFacebookRequestScores();
	}
	
    public void OnFriendScoreRecievedOnFriendScoreRecieved(string message) {		
		string[] splitMessage = message.Split('#');
		AddFriend(splitMessage[0], splitMessage[1],splitMessage[2]);
	}
	
	public void OnUserIDRecieved(string message) {
			m_UserID = message;
	}
	public void OnUserNameRecieved(string message) {
			m_UserName = message;
	}

	public override void UpdateScoreAPIScore (int score)
	{
		if(IsLoggedIn()){
            if(AVFacebook.Scores.GetUser() != null){
								AVFacebook.Scores.GetUser().Score = Mathf.Max(score, AVFacebook.Scores.GetUser().Score);
								_avFacebookSubmitNewScore(AVFacebook.Scores.GetUser().Score);
			}
			RetrieveFriendScores();
		}
	}

	public override void SendChallenge(string title, string message)
	{
			//AVAndroidInterface.Facebook.User.SendChallenge(title,message);
	}
	
	private IEnumerator LoginAndUpdateScoreAPIScore(int score)
	{
		m_WaitingOnLogin = true;
		Login();
		while(m_WaitingOnLogin){
			yield return new WaitForEndOfFrame();
		}
		_avFacebookSubmitNewScore(score);
	}

	public override string GetUserID ()
	{
			return m_UserID;
	}

		public override string GetUserName ()
	{
				return m_UserName;
	}

	public override void RequestMissionSetActivity (int missionSet)
	{
		_avFacebookRequestMissionSet(missionSet);
	}

	public override void AddAchievementToBatch (string achievementName)
	{
		_avFacebookAddAchievementToBatch(achievementName);
	}

	public override void SendAchievementBatch ()
	{
		_avFacebookSendAchievementBatch();
	}

	public override bool IsAvailableOnPlatform ()
	{
		return true;
	}
}

#endif
