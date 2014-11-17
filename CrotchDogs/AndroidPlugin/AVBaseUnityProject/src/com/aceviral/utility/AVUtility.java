package com.aceviral.utility;

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.util.Log;

public class AVUtility {
	
	// Having just one debug function that handles whether to log debug info
	// is more useful than many 'if(debug) log.v()' everywhere in the code
	private static final boolean LogDebugOut = false;
	
	public static Activity MainActivity;
	
	public static void UnitySendMessageSafe(String gameObjectName, String method, String data) {
		if (gameObjectName == null || gameObjectName.equals("")) {
			Log.e("AVSharedScripts", "UnitySendMessageSafe gameObjectName null/empty", new Exception());
			return;
		}
		if (method == null || method.equals("")) {
			Log.e("AVSharedScripts", "UnitySendMessageSafe method null/empty (" + gameObjectName + ")", new Exception());
			return;
		}
		if (data == null) {
			Log.e("AVSharedScripts", "UnitySendMessageSafe data null (" + gameObjectName + "," + method + ")", new Exception());
			data = "";
		}
		try {
			UnityPlayer.UnitySendMessage(gameObjectName, method, data);
		} catch (Exception e) {
			Log.e("AVSharedScripts", "UnityPlayer.UnitySendMessage failed: " + e.getLocalizedMessage());
			e.printStackTrace();
		}
	}
	
	public static void DebugOut(String tag, String msg){
		if(LogDebugOut)
			Log.v(tag,msg);
	}	
	
	public static void MakeDialogBox(String title, String msg){
		DebugOut("MsgBoxManager", title + ": " + msg);
		UnityPlayer.UnitySendMessage("MsgBoxManager", "ShowOkDialogFromExternalRequest", title+"#"+msg);
	}
}
