using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InAppUnityMac : InAppUnity{
    	
    	public void Awake()
    	{
    		StartCoroutine(FlushAsync());
    	}
    	
    	IEnumerator FlushAsync(){
    		while(true){
    			UnityMacBinding.FlushPurchases();
    			yield return new WaitForSeconds(1.0f);
    		}
    	}
    	
    	public override void RequestPurchase (InAppID id)
    	{
    		if(id.Managed){
    			string scrambled = Utility.ScrambleString(id.MacID);	
    			if(PlayerPrefs.GetInt(scrambled, 0) == 1){
                    MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                        Localization.Instance.GetString(Localization.eLocalKeys.AV_Purchase_Failed_Already_Owned));
    				return;
    			}
    		}
    		UnityMacBinding.RequestInAppPurchase(id.MacID);
    	}

    	public override void RestorePurchases ()
    	{
    		UnityMacBinding.RequestRestorePurchases();
    	}

    	public static void PurchaseNotification(string message)
    	{
    		Debug.Log("Got purchase notification! " + message);
    		foreach (InAppID id in InAppUnity.m_PurchaseList)
    		{
    			if(id.MacID == message){
    				if(id.Managed){
    					string scrambled = Utility.ScrambleString(message);
    					if(PlayerPrefs.GetInt(scrambled, 0) == 0){
                            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                                Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
    						id.OnBought();
    						PlayerPrefs.SetInt(scrambled, 1);
    					}
    				} else {
                        MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases), 
                            Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Congratulations_Purchase_Of_x_Successful, id.Name));
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
    	
        protected override void OnPurchaseFailed (InAppID id)
    	{
    		
    	}
    	
    	public override void RequestPurchasePrices ()
    	{
    		
    	}
    }
}