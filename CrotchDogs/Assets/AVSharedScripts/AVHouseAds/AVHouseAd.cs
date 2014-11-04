using System;
using UnityEngine;

[System.Serializable]
public class AVHouseAd
{
	public bool HD, active, defaultAd;
	public int updateTime;
	public float x, y;
	public string slotId, adURL, imageURL;
	public Texture2D image;

	public void AdvertPressed ()
	{
		AVHouseAdInterface.OpenHouseAdUrl(adURL, slotId);
	}
}

