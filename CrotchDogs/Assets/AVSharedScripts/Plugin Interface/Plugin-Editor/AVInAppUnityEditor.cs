using UnityEngine;
using System.Collections;

public class AVInAppUnityEditor : AVInAppUnity
{
	
	public void Awake ()
	{
		this.gameObject.name = "AVInAppUnity";
		
	}
	#region implemented abstract members of AVInAppUnity
	public override void RequestPurchase (AVInAppID id)
	{
        Debug.Log("Editor requesting purchase");
		AVMsgBox.Show ("Debug Purchases", "Would you like to buy " + id.Name + "?", AVMsgBoxType.YES_NO, (response) => {
			if (response == AVMsgBoxResponse.YES) {
				PurchaseNotification (id.AndroidID);
			} else {
				PurchaseFailed (id.AndroidID);
			}
		});
	}

	public override void RestorePurchases ()
	{
		Debug.Log ("Restoring purchases");
	}

	public void PurchaseNotification (string message)
	{
		Debug.Log ("EDITOR CALLBACK RECIEVED!!!! [ " + message + " ]");

		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if (id.AndroidID == message) {
				if (id.Managed) {
					string scrambled = AVUtility.ScrambleString (message);
					if (PlayerPrefs.GetInt (scrambled, 0) == 0) {
						AVMsgBox.Show ("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
                        if(id.OnBought != null)
						    id.OnBought ();
						PlayerPrefs.SetInt (scrambled, 1);
					}
				} else {
					AVMsgBox.Show ("In App Purchases", "Congratulations! Your purchase of " + id.Name + " was successful!");
                    if(id.OnBought != null)
					    id.OnBought ();
				}
				
				break;	
			}
		}
	}

	public override void PurchaseFailed (string message)
	{
		foreach (AVInAppID id in AVInAppUnity.m_PurchaseList)
		{
			if (id.AndroidID == message) {
				OnPurchaseFailed (id);
				break;
			}
		}
	}

	public override void OnPurchaseDataReceived (string data)
	{
		
	}

	public override void RequestPurchasePrices ()
	{
		StartCoroutine (DelayPurchasePrices ());
	}

	private IEnumerator DelayPurchasePrices ()
	{
//		float min = 0.2f, max = 0.7f;
//		yield return new WaitForSeconds (Random.Range (min, max));
//		BronzePurchase.Price = "$0.99";
//		yield return new WaitForSeconds (Random.Range (min, max));
//		DiamondPurchase.Price = "$9.99";
//		yield return new WaitForSeconds (Random.Range (min, max));
		yield return new WaitForSeconds (1);
	}
	#endregion
}