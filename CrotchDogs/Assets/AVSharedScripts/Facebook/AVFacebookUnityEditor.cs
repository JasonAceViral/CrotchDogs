using System.Collections;
using UnityEngine;

public class AVFacebookUnityEditor : AVFacebookUnity {
	bool m_LoggedIn = false;

	void Awake() {
		this.gameObject.name = "FacebookObject[Editor]";
		if (HasRecievedFacebookReward()) {
			m_LoggedIn = true;
		}
	}

	public override void SendMessageViaFriendPicker()
	{
		
	}
	

	public override void Login() {
		m_LoggedIn = true;
		FireFacebookLogin();
	}

	public override bool IsLoggedIn() {
		return m_LoggedIn;
	}

	public override void SendChallenge(string title, string message)
	{
	}

    public override void PostMessage(string msg) {
        AVDebug.Log("AVFacebookUnity: Posting message: " + msg);
    }
	public override void CovertPost(string msg) {

	}

	public override void RequestMissionSetActivity(int missionSet) {
		AVDebug.Log("AVFacebookUnityEditor: Requesting Mission set activity" + missionSet);
	}

	public override void Like() {
		Application.OpenURL("http://www.facebook.com/AngryGranRun");
		OnGivenFacebookLoginReward();
		SetRecievedFacebookReward();
		Login();
	}

	public override void UpdateScoreAPIScore(int score) {
		if (IsLoggedIn()) {
			AVFacebook.Scores.GetUser().Score = Mathf.Max(score, AVFacebook.Scores.GetUser().Score);
		}
	}

	public override void RetrieveFriendScores() {
		StartCoroutine(SendFakeFriendScores(true));
	}

	private IEnumerator SendFakeFriendScores(bool gradually) {
		//AVDebug.Log("Simulating friends scores retrieval");

		bool slowConnection = (Random.Range(0.0f, 1.0f) > 0.8f);
		float connectionModifier = (slowConnection) ? Random.Range(3.0f, 5.0f) : 0.0f;
		//if(slowConnection) AVDebug.Log("Simulating slow connection: Modified by " + connectionModifier.ToString() + " seconds");

		if (gradually) {
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
			gradually = false;
		}

		int minScore = 0, maxScore = 300;

		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("andrewHunt93", "Andrew Hunt", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("goodsum", "Aron Springfield", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("1130755867", "Phil Smith", "0");
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("bradley.fail.3", "Bradley Fail", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("5", "Cliff Anderson", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("craig.watson.3950", "Craig Watson", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("7", "Dave Lightwave", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("8", "Gary Shrimpling", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("9", "Gavril Lavigne", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("WWWebster", "James Webster", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("11", "Jenny Burbeck", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("574696934", "Matthew Thompson", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("13", "PHP Matt", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("14", "PHP Tim", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("15", "Shazad Mohammed", Random.Range(minScore, maxScore).ToString());
		if (gradually)
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) + connectionModifier);
		AddFriend("timtupman", "Tim Webster", Random.Range(minScore, maxScore).ToString());

		FireFacebookFriendsLoaded();
	}

	public override string GetUserID() {
		return "1130755867";
	}

	public override string GetUserName()
	{
		return "Unity Editor";
	}

	public override void AddAchievementToBatch(string achievementName) {
	}

	public override void SendAchievementBatch() {
	}

	public override bool IsAvailableOnPlatform() {
		return true;
	}
}
