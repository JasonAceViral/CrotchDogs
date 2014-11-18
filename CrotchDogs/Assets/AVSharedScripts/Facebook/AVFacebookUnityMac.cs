
public class AVFacebookUnityMac : AVFacebookUnity {

    #region implemented abstract members of AVFacebookUnity
    public override void Login() {

    }

    public override bool IsLoggedIn() {
        return false;
    }


	public override void SendMessageViaFriendPicker()
	{

	}

    public override void PostMessage(string msg) {

    }

	public override void SendChallenge(string title, string message)
	{
	}

    public override void CovertPost(string msg) {

    }

    public override void RequestMissionSetActivity(int missionSet) {

    }

    //	public override void PostScore (int score)
    //	{
    //		
    //	}

    public override void UpdateScoreAPIScore(int score) {

    }

    public override void RetrieveFriendScores() {

    }

    public override void Like() {

    }

    public override string GetUserID() {
        return string.Empty;
    }public override string GetUserName() {
        return string.Empty;
    }
    #endregion

    public override void AddAchievementToBatch(string achievementName) {
    }

    public override void SendAchievementBatch() {
    }

    public override bool IsAvailableOnPlatform() {
        return false;
    }
}
