using System;
using System.Runtime.InteropServices;

public class AVGameCenterUnity
{
	// defines
	public const string LEADERBOARD_SCORE = "com.aceviral.angrygranrun.leaderboard.score";
	public const string LEADERBOARD_DIST = "com.aceviral.angrygranrun.leaderboard.distance";
	
	//imports
#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")] private static extern void _avGameCenterShowAchievements	( );
	[DllImport ("__Internal")] private static extern void _avGameCenterShowLeaderboards	( );
	[DllImport ("__Internal")] private static extern void _avGameCenterPostScore		(int score, string leaderboard );
	[DllImport ("__Internal")] private static extern void _avGameCenterPostAchievement	(float progress, string achievement );
#endif
	public static void ShowLeaderboards()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_avGameCenterShowLeaderboards();
#endif
	}
	
	public static void ShowAchievements()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_avGameCenterShowAchievements();
#endif
	}
	
	public static void SendAchievement(float progress, string achievement)
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_avGameCenterPostAchievement(progress, achievement);
#endif
	}
	
	public static void SendScore(int score, string leaderboard)
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_avGameCenterPostScore(score, leaderboard);
#endif
	}
}

