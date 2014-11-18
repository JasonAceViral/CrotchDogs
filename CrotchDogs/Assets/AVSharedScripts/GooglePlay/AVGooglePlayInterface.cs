using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

class AVGooglePlayInterface : MonoBehaviour {

//    #if UNITY_IPHONE && !UNITY_EDITOR
//    [DllImport ("__Internal")] private static extern void _avGooglePlayLoginWithClientId( string key );
//    [DllImport ("__Internal")] private static extern bool _avGooglePlayIsLoggedIn( );
//	[DllImport ("__Internal")] private static extern bool _avGooglePlayShowAchievements( );
//	[DllImport ("__Internal")] private static extern bool _avGooglePlayShowLeaderboards( );
//	[DllImport ("__Internal")] private static extern bool _avGooglePlayShowLeaderboard( string key );
//	[DllImport ("__Internal")] private static extern bool _avGooglePlayUnlockAchievement( string key );
//	[DllImport ("__Internal")] private static extern bool _avGooglePlayUpdateLeaderboard( string key, float score );
//    #endif
//
    public delegate void GooglePlaySignInResponse(bool success);
    static GooglePlaySignInResponse gpResponse;

    public static AVGooglePlayInterface m_Instance;

    public static AVGooglePlayInterface Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject().AddComponent<AVGooglePlayInterface>() as AVGooglePlayInterface;
                m_Instance.gameObject.name = "AVGooglePlayInterface";
            }
            return m_Instance;
        }
    }

    public void SignIn(GooglePlaySignInResponse resp)
    {
        gpResponse = resp;

        if (IsSignedIn())
        {
            if(gpResponse != null) gpResponse(true);
            return;
        }

        #if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.SignIn();
        }

        #elif !UNITY_EDITOR && UNITY_IPHONE

        //_avGooglePlayLoginWithClientId(AVAppConstants.iOSGooglePlusClientId);

        #endif
    }

    // Invoked from native code
    void SignInComplete(string success) {
        if (gpResponse != null)
            gpResponse(success == "true");
    }

    public static void SignOut()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.SignOut();
        }

		#endif
	}

    public static bool IsSignedIn()
    {
		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        return AVAndroidInterface.GameServices.IsSignedIn();
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //return _avGooglePlayIsLoggedIn();

		#endif

		return false;
    }
		
    public static void ShowAchievements()
	{
		if (!IsSignedIn ())
			return;

		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.ShowAchievements();
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //_avGooglePlayShowAchievements();

		#endif
	}

    public static void ShowLeaderboards()
	{
		if (!IsSignedIn ())
			return;

		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.ShowLeaderboards();
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //_avGooglePlayShowLeaderboards();

		#endif
	}

    public static void ShowLeaderboard(string boardId)
	{
		if (!IsSignedIn ())
			return;

		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.ShowLeaderboard(boardId);
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //_avGooglePlayShowLeaderboard(boardId);

		#endif
	}

    public static void UnlockAchievement(string achId)
	{
		if (!IsSignedIn ())
			return;

		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.UnlockAchievement(achId);
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //_avGooglePlayUnlockAchievement(achId);

		#endif
	}

    public static void UpdateLeaderboard(string boardId, float score)
	{
		if (!IsSignedIn ())
			return;

		#if UNITY_ANDROID && !UNITY_EDITOR

        if(!AVAppConstants.CompileForAmazonAppStore)
        {
        AVAndroidInterface.GameServices.UpdateLeaderboard(boardId, score);
        }

        #elif UNITY_IPHONE && !UNITY_EDITOR

        //_avGooglePlayUpdateLeaderboard(boardId, score);

		#endif
	}
}
