using UnityEngine;
using System.Collections;

public class AVRatingsManagerAndroid : AVRatingsManager
{
	
	protected override void OnStart ()
	{
		base.OnStart ();
	}

	public override void AskToRate ()
	{
		AVAndroidInterface.RateGame ("Angry Gran Run", "com.aceviral.angrygranrun");
		base.AskToRate ();
	}
}
