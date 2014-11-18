using UnityEngine;
using System.Collections;

public class AVRatingsManagerEditor : AVRatingsManager
{
	protected override void OnStart ()
	{
		base.OnStart ();
	}

	public override void AskToRate ()
	{
		Debug.Log ("Asking to rate");
		base.AskToRate ();
	}
}
