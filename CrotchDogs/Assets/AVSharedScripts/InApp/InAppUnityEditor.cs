using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InAppUnityEditor : InAppUnity
    {

    	#region implemented abstract members of AVInAppUnity
    	public override void RequestPurchase (InAppID id)
    	{
            Debug.Log("Editor requesting purchase");
    		MsgBox.Show ("Debug Purchases", "Would you like to buy " + id.Name + "?", MsgBoxType.YES_NO, (response) => {
    			if (response == MsgBoxResponse.YES) {
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

    		foreach (InAppID id in InAppUnity.m_PurchaseList)
    		{
    			if (id.AndroidID == message) {
    				if (id.Managed) {
    					string scrambled = Utility.ScrambleString (message);
    					if (PlayerPrefs.GetInt (scrambled, 0) == 0) {
                            MsgBox.Show (Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                                Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
                            if(id.OnBought != null)
    						    id.OnBought ();
    						PlayerPrefs.SetInt (scrambled, 1);
    					}
    				} else {
                        MsgBox.Show (Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                            Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
                        if(id.OnBought != null)
    					    id.OnBought ();
    				}
    				
    				break;	
    			}
    		}
    	}

    	public override void PurchaseFailed (string message)
    	{
    		foreach (InAppID id in InAppUnity.m_PurchaseList)
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
    		yield return new WaitForSeconds (1);
    	}
    	#endregion
    }
}