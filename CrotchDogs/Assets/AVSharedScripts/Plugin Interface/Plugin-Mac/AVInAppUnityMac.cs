
using UnityEngine;
using System.Collections;

public class AVInAppUnityMac : AVInAppUnity{
	
	public void Awake()
	{
		this.gameObject.name = "AVInAppUnity";
		StartCoroutine(FlushAsync());
	}
	
	IEnumerator FlushAsync(){
		while(true){
			AVUnityMacBinding.FlushPurchases();
			yield return new WaitForSeconds(1.0f);
		}
	}
	
	public override void RequestPurchase (AVInAppID id)
	{
		if(id.Managed){
			string scrambled = AVUtility.ScrambleString(id.MacID);	
			if(PlayerPrefs.GetInt(scrambled, 0) == 1){
				AVMsgBox.Show("In App Purchases", "Sorry! You've already purchased this ONE-TIME In App Purchase. You can only receive this product once per installation.");
				return;
			}
		}
		AVUnityMacBinding.RequestInAppPurchase(id.MacID);
	}

	public override void RestorePurchases ()
	{
		AVUnityMacBinding.RequestRestorePurchases();
	}

	public static void PurchaseNotification(string message)
	{
		Debug.Log("Got purchase notification! " + message);
		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if(id.MacID == message){
				if(id.Managed){
					string scrambled = AVUtility.ScrambleString(message);
					if(PlayerPrefs.GetInt(scrambled, 0) == 0){
						AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
						id.OnBought();
						PlayerPrefs.SetInt(scrambled, 1);
					}
				} else {
					AVMsgBox.Show("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
					id.OnBought();
				}
				
				break;	
			}
		}
	}
	public override void PurchaseFailed (string message)
	{
		
	}
	
	public override void OnPurchaseDataReceived (string data)
	{
		
	}
	
	public override void OnPurchaseFailed (AVInAppID id)
	{
		
	}
	
	public override void RequestPurchasePrices ()
	{
		
	}
}