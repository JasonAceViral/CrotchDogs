using UnityEngine;
using System.Collections;

public class AVRateMe {

    // Declare a delegate type for processing a book:
    public delegate void RateMePopupClosed();

    public static void ShowRateMePopup(RateMePopupClosed popupDel)
	{
        AVMsgBox.Show("Rate This App", "Are you enjoying this app?", AVMsgBoxType.YES_NO, (response) =>
		{
            if (response == AVMsgBoxResponse.YES)
			{
                UserIsEnjoying(popupDel);
            }
            else
            {
                if(popupDel != null)
                    popupDel();
            }
		});
	}

    public static void UserIsEnjoying(RateMePopupClosed popupDel)
    {
        AVMsgBox.Show("Rate This App", "Would you like to rate this app?", AVMsgBoxType.YES_NO, (response) =>
        {
            if (response == AVMsgBoxResponse.YES)
            {
                #if UNITY_IPHONE
                Application.OpenURL("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=" + 
                AVAppConstants.iOSAppId + "&onlyLatestVersion=true&pageNumber=0&sortOrdering=3&type=Purple+Software");
                #elif UNITY_ANDROID
                if(AVAppConstants.CompileForAmazonAppStore)
                {
                    Application.OpenURL("market://details?id=" + AVAppConstants.AndroidPackageName);
                }
                else
                {
                    Application.OpenURL("market://details?id=" + AVAppConstants.AndroidPackageName);

                    //Application.OpenURL(https://play.google.com/store/apps/details?id=" + AVAppConstants.AndroidPackageName);
                }
				#elif UNITY_METRO || UNITY_WP8
					AVWindowsPhonePlugin.AVWindowsPhoneUtility.OpenMarketplaceForReview();
                #endif
            }

            if(popupDel != null)
                popupDel();
        });
    }
}
