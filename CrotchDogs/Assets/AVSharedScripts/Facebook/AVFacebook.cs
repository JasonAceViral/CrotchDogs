using System.Collections.Generic;
using System;
using UnityEngine;

public class AVFacebook {


	public static bool IsAvailableOnPlatform() {
		return AVFacebookUnity.Instance.IsAvailableOnPlatform();
	}

	public static void SignIn() {
		AVFacebookUnity.Instance.Login();
	}


	public static bool IsLoggedIn() {
		return AVFacebookUnity.Instance.IsLoggedIn();
	}

	public static void Post(string msg, bool covert) {
		if (covert) AVFacebookUnity.Instance.CovertPost(msg); else AVFacebookUnity.Instance.PostMessage(msg);
	}

	public static void RequestMissionSetComplete(int index) {
		AVFacebookUnity.Instance.RequestMissionSetActivity(index);
	}

	public class Achievements {
		//private static List<AchievementData.Achievement> _achievementsSentThisSession = new List<AchievementData.Achievement>();
		//public static void AddAchievementToBatch(AchievementData.Achievement ach) {
		//    if (!_achievementsSentThisSession.Contains(ach)) {
		//        _achievementsSentThisSession.Add(ach);
		//        AVFacebookUnity.Instance.AddAchievementToBatch(ach.GetFilename());
		//    }
		//}

		//public static void SendAchievementBatch() {
		//    AVFacebookUnity.Instance.SendAchievementBatch();
		//}
	}

	/// <summary>
	/// Sub-class for scores for organisational purposes.
	/// </summary>
	public class Scores {

		public static void RequestScores() {
			AVFacebookUnity.Instance.RetrieveFriendScores();
		}

		public static void PostNewScore(int score) {
			AVFacebookUnity.Instance.UpdateScoreAPIScore(score);
		}

		public static AVFacebookFriend GetUser() {
			foreach (AVFacebookFriend friend in GetFriendList()) {
				if (friend.ID == AVFacebookUnity.Instance.GetUserID()) {
					return friend;
				}
			}
			return null;
		}

		public static int GetUserScore() {
			AVFacebookFriend user = GetUser();
			if (user != null) return user.Score;
			return 0;
		}

		public static AVFacebookFriend GetNextHighestFriend(int score) {
			return AVFacebookUnity.Instance.GetNextBestFriend(score);
		}

		public static AVFacebookFriend[] GetFriendList() {
			return AVFacebookUnity.Instance.Friends.ToArray();
		}

		public static void SortFriends() {
			AVFacebookUnity.Instance.SortFriendsByScore();
		}

		public static AVFacebookFriend[] GetSortedFriendList() {
			AVFacebookUnity.Instance.SortFriendsByScore();
			return AVFacebookUnity.Instance.Friends.ToArray();
		}

		public static void DownloadFriendProfilePicture(AVFacebookFriend friend) {
			AVFacebookUnity.Instance.DownloadProfilePictureInBackground(friend);
		}

		public static void DownloadAllProfilePictures() {
			foreach (AVFacebookFriend friend in GetFriendList()) {
				if (friend.ProfilePicture != null) {
					AVFacebookUnity.Instance.DownloadProfilePictureInBackground(friend);
				}
			}
		}

		public static bool HasFriends() {
			if (AVFacebookUnity.Instance.Friends.Count > 0) return true;
			return false;
		}
	}

	public static void SendMessageViaFriendPicker()
	{
		AVFacebookUnity.Instance.SendMessageViaFriendPicker();
	}

	public static void SendChallenge(string title,string message)
	{
		AVFacebookUnity.Instance.SendChallenge(title,message);
	}
}
