package com.aceviral.utility;

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.util.Log;

public class AVUtility {
	
	// Having just one debug function that handles whether to log debug info
	// is more useful than many 'if(debug) log.v()' everywhere in the code
	private static final boolean LogDebugOut = false;
	
	public static Activity MainActivity;
	
	public static void DebugOut(String tag, String msg){
		if(LogDebugOut)
			Log.v(tag,msg);
	}	
	
	public static void MakeDialogBox(String title, String msg){
		DebugOut("MsgBoxManager", title + ": " + msg);
		UnityPlayer.UnitySendMessage("MsgBoxManager", "ShowOkDialogFromExternalRequest", title+"#"+msg);
	}
}
