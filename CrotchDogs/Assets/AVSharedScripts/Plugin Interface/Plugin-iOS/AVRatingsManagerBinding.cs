using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AVRatingsManagerBinding {
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void _avAskToRate( string appName );
#endif

	public static void AskToRate(string appName) {
#if UNITY_IPHONE
		_avAskToRate( appName );
#endif
	}
}
