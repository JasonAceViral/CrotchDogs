using System;
using UnityEngine;

public delegate void ProfilePictureDownloaded();

[System.Serializable]
public class AVFacebookFriend
{

    public event ProfilePictureDownloaded OnProfilePictureDownloaded;

	public AVFacebookFriend(string id, string name, string score){
		ID = id;
		Name = name;
		Score = int.Parse(score);
	}
	public string ID;
	public string Name;
	public int Score;
	public Texture2D ProfilePicture;

    public void FirePictureDownloaded()
    {
        if (OnProfilePictureDownloaded != null)
        {
            OnProfilePictureDownloaded();
        }
    }

}

