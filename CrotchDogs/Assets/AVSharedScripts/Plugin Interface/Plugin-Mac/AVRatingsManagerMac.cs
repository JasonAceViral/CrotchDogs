using UnityEngine;
using System.Collections;

public class AVRatingsManagerMac : AVRatingsManager
{
	
	protected override void OnStart ()
	{
		base.OnStart ();
	}

	public override void AskToRate ()
	{
		//Debug.Log("Asking to rate");
		//AVRatingsManagerBinding.AskToRate("Angry Gran Run");
		base.AskToRate ();
	}
}


