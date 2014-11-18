// C# example
using UnityEditor;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using System.Diagnostics;
 
class PerformBuild
{
	#region Per-Project Variables
	
	// Generally, stick to editing ONLY these variables and platform setups
	
	private static string ANDROID_KEY_ALIAS_PASSWORD = "phillovesgames";
	private static string ANDROID_KEY_STORE_PASSWORD = "phillovesgames";
	
	private const int WEBPLAYER_SCREEN_WIDTH = 640;
	private const int WEBPLAYER_SCREEN_HEIGHT = 480;
	
	// Add any project settings you may want here, there are many different options available
	// http://docs.unity3d.com/Documentation/ScriptReference/PlayerSettings.html
	
	private static void SetupIPhoneBuild(){
		PlayerSettings.strippingLevel = StrippingLevel.StripByteCode;
	}
	
	private static void SetupAndroidBuild(){
		PlayerSettings.strippingLevel = StrippingLevel.StripByteCode;
		if(ANDROID_KEY_ALIAS_PASSWORD != string.Empty && ANDROID_KEY_STORE_PASSWORD != string.Empty){
			PlayerSettings.Android.keyaliasPass = ANDROID_KEY_ALIAS_PASSWORD;
			PlayerSettings.Android.keystorePass = ANDROID_KEY_STORE_PASSWORD;
		}
	}
	
	private static void SetupWebPlayerBuild(){
		PlayerSettings.defaultWebScreenWidth = WEBPLAYER_SCREEN_WIDTH;
		PlayerSettings.defaultWebScreenHeight = WEBPLAYER_SCREEN_HEIGHT;
	}
	
	#endregion
	
	#region Build Paths
	
	// It is VERY important to leave these build paths alone else the build server will not
	// be able to re-package your app and send it back to you.
	
	private static string m_BaseBuildPath = "";
	
	private static void ParseCommandLine(){
		string[] cmdLine = System.Environment.GetCommandLineArgs ();
		m_BaseBuildPath = cmdLine[cmdLine.Length-1];
	}
	
	static string GetIPhoneBuildPath(){
		return m_BaseBuildPath + "iPhone";
	}
	
	static string GetAndroidBuildPath(){
		return m_BaseBuildPath + "Android/Android.apk";
	}
	
	static string GetWebPlayerBuildPath(){
		return m_BaseBuildPath + "WebPlayer";
	}
	
	#endregion
	
	static string[] GetBuildScenes ()
	{
		List<string> names = new List<string> ();
 
		foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes) {
			if (e == null)
				continue;
 
			if (e.enabled)
				names.Add (e.path);
		}
		return names.ToArray ();
	}
	
    [UnityEditor.MenuItem("AceViral/Build Server/Test iOS Build")]
	static void TestiOSBuild ()
	{ 
		ParseCommandLine();
		string[] scenes = GetBuildScenes ();		
		System.Console.WriteLine("Starting iPhone Build");
		SetupIPhoneBuild();
		BuildPipeline.BuildPlayer (scenes, "TestiOSBuild", BuildTarget.iPhone, BuildOptions.None);
		OniOSPostprocessBuild(BuildTarget.iPhone, "TestiOSBuild");
	}
 
	static void CommandLineBuild ()
	{ 
		ParseCommandLine();
		string[] scenes = GetBuildScenes ();
		if (scenes == null || scenes.Length == 0){
			System.Console.WriteLine("BUILD FAIL: No scenes included in build");
			return;
		}
		
		for (int i=0; i<scenes.Length; ++i) {
			System.Console.WriteLine (string.Format ("Scene[{0}]: \"{1}\"", i, scenes [i]));
		}
		
		System.Console.WriteLine("Starting iPhone Build");
		SetupIPhoneBuild();
		BuildPipeline.BuildPlayer (scenes, GetIPhoneBuildPath(), BuildTarget.iPhone, BuildOptions.None);
		OniOSPostprocessBuild(BuildTarget.iPhone, GetIPhoneBuildPath());
		
//		System.Console.WriteLine("Starting Android Build");
//		SetupAndroidBuild();
//		BuildPipeline.BuildPlayer (scenes, GetAndroidBuildPath(), BuildTarget.Android, BuildOptions.None);
//		
//		System.Console.WriteLine("Starting Web Player Build");
//		SetupWebPlayerBuild();
//		BuildPipeline.BuildPlayer (scenes, GetWebPlayerBuildPath(), BuildTarget.WebPlayer, BuildOptions.None);
	}

	public static void OniOSPostprocessBuild(BuildTarget target, string pathToBuildProject){
		UnityEngine.Debug.Log("----Custom Script---Executing post process build phase."); 		
		string objCPath = Application.dataPath + "/Plugins/iOS";
		Process myCustomProcess = new Process();		
		myCustomProcess.StartInfo.FileName = "python";
        myCustomProcess.StartInfo.Arguments = string.Format("Assets/Editor/post_process.py \"{0}\" \"{1}\"", pathToBuildProject, objCPath);
        myCustomProcess.StartInfo.UseShellExecute = false;
        myCustomProcess.StartInfo.RedirectStandardOutput = false;
		myCustomProcess.Start(); 
		myCustomProcess.WaitForExit();
		UnityEngine.Debug.Log("----Custom Script--- Finished executing post process build phase.");  
	}
}