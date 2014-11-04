using UnityEngine;
using System.Collections;

public delegate void InAppBoughtFunction();
public class AVInAppID {
	
	
	public string Name;
	public string iOSID;
	public string MacID;
	public string AndroidID;
	public string AmazonID;
    public string WindowsPhoneID;
	public bool Managed;
	public InAppBoughtFunction OnBought;
	public string Price = string.Empty;
	public string Display;
    public bool isConsumable = false;
}
