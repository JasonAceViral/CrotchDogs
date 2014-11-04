using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FacebookLoginHandler();
public delegate void FacebookFriendHandler();

public abstract class AVFacebookUnity : MonoBehaviour
{
	public static event FacebookLoginHandler onLogin;
	public static event FacebookFriendHandler onFriendsFound;

	#region Per-Project Stuff
	// Get this from developers.facebook.com
	public static string AppID = "590632264348207";

	public void FireFacebookLogin()
	{
		

#if UNITY_WP8
		AVWindowsPhonePlugin.FB.GetUserName();
#else
		if (onLogin != null)
		{
			onLogin();
		}
#endif
	}

	public void FireFacebookFriendsLoaded()
	{
		if (onFriendsFound != null)
		{
			onFriendsFound();
		}
	}

	protected void OnGivenFacebookLoginReward()
	{
		//PlayerData.Coins += 1000;
		//AudioManager.PlayShopUpgrade ();
		//NMenuManager.Manager.UpdateMenuCoinDisplay ();
		//AVMsgBox.Show ("Facebook", "Thank you for signing into Facebook. Here's 1000 free coins!");
		//PlayerPrefs.SaveAll ();
	}

	public bool HasRecievedFacebookReward()
	{
		return false;// (PlayerData.FacebookLiked == 1);
	}

	protected void SetRecievedFacebookReward()
	{
		//PlayerData.FacebookLiked = 1;
	}

	protected string GetBitlyLink()
	{
		return "http://bit.ly/RBnqnX";
	}

	protected void SendFBAchievements()
	{
		//AchievementData.SendAchievementsToFacebook ();
	}
	#endregion
	#region Singleton Init
	// Different platforms select a different implementation
	private static AVFacebookUnity m_Instance;

	public static AVFacebookUnity Instance
	{
		get
		{
			if (m_Instance == null)
			{
#if UNITY_EDITOR
				m_Instance = new GameObject().AddComponent<AVFacebookUnityEditor>();
#elif UNITY_ANDROID
				m_Instance = new GameObject().AddComponent<AVUnityFacebookAndroid>();
#elif UNITY_IPHONE
				m_Instance = new GameObject().AddComponent<AVFacebookUnityIOS>();
#elif UNITY_STANDALONE_OSX
				m_Instance = new GameObject().AddComponent<AVFacebookUnityMac>();
#elif UNITY_WEBPLAYER
				m_Instance = new GameObject().AddComponent<AVFacebookWeb>();
#elif UNITY_METRO 
				m_Instance = new GameObject().AddComponent<AVFacebookUnityWin>();
#elif UNITY_WP8
				m_Instance = new GameObject().AddComponent<AVFacebookUnityWP8>();
#endif
				m_Instance.StartCoroutine(m_Instance.GetFriendScoresOnLogin());
				m_Instance.Initialize();
			}

			return m_Instance;
		}
	}
	#endregion
	#region Interface
	public abstract bool IsAvailableOnPlatform();

	public abstract void Login();

	public abstract bool IsLoggedIn();

    public abstract void PostMessage(string msg);

	public abstract void CovertPost(string msg);

	public abstract void UpdateScoreAPIScore(int score);

	public abstract void RetrieveFriendScores();

	public abstract void Like();

	public abstract string GetUserID();

	public abstract string GetUserName();

	public abstract void RequestMissionSetActivity(int missionSet);

	public abstract void AddAchievementToBatch(string achievementName);

	public abstract void SendMessageViaFriendPicker();

	public abstract void SendChallenge(string title, string message);

	public abstract void SendAchievementBatch();
	#endregion
	#region Friend Data
	public List<AVFacebookFriend> Friends = new List<AVFacebookFriend>();
	private Queue<AVFacebookFriend> DownloadQueue = new Queue<AVFacebookFriend>();
	#endregion
	#region Helper Functions
	public event Action FriendStatRetrieved;

	public virtual void OnFriendStatRetrieved()
	{
		if (FriendStatRetrieved != null)
			FriendStatRetrieved();
	}

	IEnumerator GetFriendScoresOnLogin()
	{
		while (!IsLoggedIn())
			yield return new WaitForSeconds(1.0f);
		AVFacebook.Scores.RequestScores();
	}
	// Gets called when friend data comes in (async)
	public void AddFriend(string ID, string name, string score)
	{
		// Check whether we already have friends stored - update the score if they do
		for (int i = 0; i < Friends.Count; i++)
		{
			if (Friends[i].ID == ID)
			{
				if (Friends[i].Score < int.Parse(score))
				{
					Friends[i].Score = int.Parse(score);
					OnFriendStatRetrieved();
				}
				return;
			}
		}
		AVFacebookFriend newFriend = new AVFacebookFriend(ID, name, score);
		DownloadProfilePictureInBackground(newFriend);
		Friends.Add(newFriend);
		//OnFriendStatRetrieved();
	}

	public void SortFriendsByScore()
	{
		Friends.Sort(delegate(AVFacebookFriend x, AVFacebookFriend y)
		{
			if (x == null)
			{
				return (y == null) ? 0 : 1;
			}
			else
			{
				if (y == null)
				{
					return -1;
				}
				else
				{
					if (x.Score > y.Score)
						return -1;
					if (y.Score > x.Score)
						return 1;
					return 0;
				}
			}
		});
	}

	public AVFacebookFriend GetNextBestFriend(int currentScore)
	{
		if (!IsLoggedIn() || Friends.Count <= 0)
			return null;
		SortFriendsByScore();
		AVFacebookFriend nextBestFriend = Friends[0];
		for (int i = 1; i < Friends.Count; i++)
		{
			nextBestFriend = (Friends[i].Score >= currentScore && Friends[i].Score < nextBestFriend.Score) ? Friends[i] : nextBestFriend;
		}
		return nextBestFriend;
	}

	public void DownloadProfilePictureInBackground(AVFacebookFriend friendToDownload)
	{
		DownloadQueue.Enqueue(friendToDownload);
	}
	#endregion

	protected virtual void Initialize()
	{
		
	}

	void Update()
	{
		if (!downloadingImage)
		{
			if (DownloadQueue.Count > 0)
			{
				if (DownloadQueue.Peek().ProfilePicture == null)
				{
					StartCoroutine(DownloadProfileImage(DownloadQueue.Peek()));
				}
				DownloadQueue.Dequeue();
			}
		}
	}

	bool downloadingImage = false;
	protected string userName;
	protected string userId;

	public void OnNameReceived(string name)
	{
		PassName(name);
#if UNITY_WP8
		if (onLogin != null)
		{
			onLogin();
		}
#endif
	}

	public void PassName(string name)
	{
		//AVFacebookFriend user = Main.mainIns.facebookManager.user;
		//if (user != null)
		{
		    userName = name;
		}

	}
	public void PassId(string id)
	{
		//AVFacebookFriend user = Main.mainIns.facebookManager.user;
		//if (user != null)
		{
		    userId = id;
		}

	}

	public void OnFriendScoreRecieved(string message)
	{
		string[] splitMessage = message.Split('#');
		AddFriend(splitMessage[0], splitMessage[1], splitMessage[2]);
	}

	public void OnFriendScoreRecievedWithApp(string message)
	{
		Debug.Log("JELLYSMASH DEBUG: IOS FRIEND IS " + message);
		string[] splitMessage = message.Split('#');
		AddFriendWithApp(splitMessage[0], splitMessage[1], splitMessage[2]);

	}

	private void AddFriendWithApp(string id, string name, string score)
	{
		AddFriend(id, name, score);
	}

	//public OnGotFriendFunction OnFriendScoreRecieved;
	//public OnGotFriendWithAppFunction OnFriendScoreRecievedWithApp;

	public IEnumerator DownloadProfileImage(AVFacebookFriend friend)
	{
		downloadingImage = true;
		int size = 64;
#if UNITY_METRO
		size = 128;
#endif
		WWW imageDownload = new WWW("https://graph.facebook.com/" + friend.ID + "/picture?width="+size+"&height="+size);
		yield return imageDownload;
		try
		{
			friend.ProfilePicture = new Texture2D(64, 64);
			if (imageDownload.error == null || imageDownload.error == string.Empty)
			{
				imageDownload.LoadImageIntoTexture(friend.ProfilePicture);
			}

			// Loads a default image (resources) if we can't actually download the friends profile pic
			if (friend.ProfilePicture == null || friend.ProfilePicture.width < 64)
			{
				friend.ProfilePicture = Resources.Load("Art/facebook-profile") as Texture2D;
			}
		}
		catch (Exception e)
		{
            Debug.LogWarning("Error downloading image: " + e.Message);
		}
		downloadingImage = false;
	}


	public virtual void RetrieveFriendsWithApp()
	{
	}
}