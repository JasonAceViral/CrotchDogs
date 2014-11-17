using UnityEngine;
using System.Collections;

namespace AceViral {

	public class RatingsManager {

	    // Declare a delegate type for processing a book:
	    public delegate void RateMePopupClosed();

        public static void AskIfEnjoyingThenRateMe(RateMePopupClosed popupDel)
		{
            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_Rate_This_App), 
                Localization.Instance.GetString(Localization.eLocalKeys.AV_Are_You_Enjoying_This_App), MsgBoxType.YES_NO, (response) =>
			{
	            if (response == MsgBoxResponse.YES)
				{
                    ShowRateMePopup(popupDel);
	            }
	            else
	            {
	                if(popupDel != null)
	                    popupDel();
	            }
			});
		}

        public static void ShowRateMePopup(RateMePopupClosed popupDel)
	    {
            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_Rate_This_App), 
                Localization.Instance.GetString(Localization.eLocalKeys.AV_Would_You_Like_To_Rate), MsgBoxType.YES_NO, (response) =>
	        {
	            if (response == MsgBoxResponse.YES)
	            {
	                #if UNITY_IPHONE
	                Application.OpenURL("http://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=" + 
                        AppConstants.IOS.AppId + "&onlyLatestVersion=true&pageNumber=0&sortOrdering=3&type=Purple+Software");
	                #elif UNITY_ANDROID
	                if(AppConstants.CompileForAmazonAppStore)
	                {
                        Application.OpenURL("market://details?id=" + AppConstants.Android.PackageName);
	                }
	                else
	                {
                        Application.OpenURL("market://details?id=" + AppConstants.Android.PackageName);

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
}
