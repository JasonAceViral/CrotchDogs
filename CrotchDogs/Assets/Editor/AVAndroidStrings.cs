using UnityEngine;
using System.Collections;

namespace AVSharedScripts {
    public class AVAndroidStrings : MonoBehaviour {

        public static string GetClassToReplaceWith(string packagename)
        {
            return "package " + packagename + ";\n\n" + classP1;
        }

        private static string classP1 = "import android.content.res.Configuration;\n"+
            "import android.graphics.PixelFormat;\n"+
            "import android.os.Bundle;\n"+
            "import android.view.KeyEvent;\n"+
            "import android.view.MotionEvent;\n"+
            "import android.view.Window;\n"+
            "import android.view.WindowManager;\n"+
            "\n" +
            "import com.aceviral.VideoRewardInterface;\n"+
            "import com.aceviral.activities.AVUnityActivity;\n"+
            "import com.unity3d.player.UnityPlayer;\n"+
            "\n"+
            "public class UnityPlayerNativeActivity extends AVUnityActivity\n" +
            "{\n"+
            "\tprotected UnityPlayer mUnityPlayer;\t\t// don't change the name of this variable; referenced from native code\n"+
            "\n"+
            "\t// Setup activity layout\n" +
            "\t@Override public void onCreate (Bundle savedInstanceState)\n"+
            "\t{\n"+
            "\t\trequestWindowFeature(Window.FEATURE_NO_TITLE);\n"+
            "\t\tsuper.onCreate(savedInstanceState);\n"+
            "\n"+
            "\t\tgetWindow().takeSurface(null);\n"+
            "\t\tsetTheme(android.R.style.Theme_NoTitleBar_Fullscreen);\n"+
            "\t\tgetWindow().setFormat(PixelFormat.RGB_565);\n"+
            "\n"+
            "\t\tmUnityPlayer = new UnityPlayer(this);\n"+
            "\t\tif (mUnityPlayer.getSettings ().getBoolean (\"hide_status_bar\", true))\n"+
            "\t\t\tgetWindow ().setFlags (WindowManager.LayoutParams.FLAG_FULLSCREEN,\n"+
            "\t\t\t                       WindowManager.LayoutParams.FLAG_FULLSCREEN);\n"+
            "\n"+
            "\t\tsetContentView(mUnityPlayer);\n"+
            "\t\tmUnityPlayer.requestFocus();\n"+
            "\t}\n"+
            "\n"+
            "\t// Quit Unity\n\t@Override public void onDestroy ()\n"+
            "\t{\n"+
            "\t\tmUnityPlayer.quit();\n"+
            "\t\tsuper.onDestroy();\n"+
            "\t}\n"+
            "\n"+
            "\t// Pause Unity\n"+
            "\t@Override protected void onPause()\n"+
            "\t{\n"+
            "\t\tsuper.onPause();\n"+
            "\t\tmUnityPlayer.pause();\n"+
            "\t}\n"+
            "\n"+
            "\t// Resume Unity\n"+
            "\t@Override protected void onResume()\n"+
            "\t{\n"+
            "\t\tsuper.onResume();\n"+
            "\t\tmUnityPlayer.resume();\n"+
            "\t}\n"+
            "\n"+
            "\t// This ensures the layout will be correct.\n"+
            "\t@Override public void onConfigurationChanged(Configuration newConfig)\n"+
            "\t{\n"+
            "\t\tsuper.onConfigurationChanged(newConfig);\n"+
            "\t\tmUnityPlayer.configurationChanged(newConfig);\n"+
            "\t}\n"+
            "\n"+
            "\t// Notify Unity of the focus change.\n"+
            "\t@Override public void onWindowFocusChanged(boolean hasFocus)\n"+
            "\t{\n"+
            "\t\tsuper.onWindowFocusChanged(hasFocus);\n"+
            "\t\tmUnityPlayer.windowFocusChanged(hasFocus);\n"+
            "\t}\n"+
            "\n"+
            "\t// For some reason the multiple keyevent type is not supported by the ndk.\n"+
            "\t// Force event injection by overriding dispatchKeyEvent().\n"+
            "\t@Override public boolean dispatchKeyEvent(KeyEvent event)\n"+
            "\t{\n"+
            "\t\tif (event.getAction() == KeyEvent.ACTION_MULTIPLE)\n"+
            "\t\t\treturn mUnityPlayer.injectEvent(event);\n"+
            "\t\treturn super.dispatchKeyEvent(event);\n"+
            "\t}\n"+
            "\n"+
            "\t// Pass any events not handled by (unfocused) views straight to UnityPlayer\n"+
            "\t@Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }\n"+
            "\t@Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }\n"+
            "\t@Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }\n"+
            "\t/*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }\n"+
            "\n"+
            "\t@Override\n\tpublic String getFacebookID() {\n"+
            "\t\treturn getString(R.string.applicationId);\n"+
            "\t}\n"+
            "\n"+
            "\t@Override\n\tpublic String getAnalyticsID() {\n"+
            "\t\treturn getString(R.string.ga_trackingId);\n"+
            "\t}\n"+
            "\t\n"+
            "\t\n"+
            "\t@Override\n"+
            "\tpublic VideoRewardInterface getVideoRewardManager() \n"+
            "\t{\n"+
            "\t\t\n"+
            "\t\treturn null;\n"+
            "\t}\n"+
            "}";
    }
}
