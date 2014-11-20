using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InAppUnityWeb : InAppUnity {
    	public override void RequestPurchase (InAppID id)
    	{
    	}

    	public override void RestorePurchases ()
    	{
    	}

    	public override void PurchaseFailed (string message)
    	{
    	}

    	public override void OnPurchaseDataReceived (string data)
    	{
    	}

    	public override void RequestPurchasePrices ()
    	{
    	}
    }
}
