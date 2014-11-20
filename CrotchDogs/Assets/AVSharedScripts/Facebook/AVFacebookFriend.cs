using System;
using UnityEngine;

namespace AceViral {

    [System.Serializable]
    public class AVFacebookFriend
    {
    	public AVFacebookFriend(string id, string name, string score){
    		ID = id;
    		Name = name;
    		Score = int.Parse(score);
    	}

    	public AVFacebookFriend(string id, string name, int score){
    		ID = id;
    		Name = name;
    		Score = score;
    	}
    	public string ID;
    	public string Name;
    	public int Score;
    	public bool DownloadFailed = false;
    	public Texture2D ProfilePicture;
    	public Action<Texture2D> OnProfilePictureDownloaded;
    }
}

