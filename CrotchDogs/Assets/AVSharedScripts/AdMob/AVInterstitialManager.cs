using System;
using UnityEngine;
using AVHiddenInterface;

namespace AceViral {
    public class AVInterstitialManager : MonoBehaviour {

        public static AVInterstitialManager m_Instance;

        public static AVInterstitialManager Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new GameObject ().AddComponent<AVInterstitialManager> ();
                    m_Instance.gameObject.name = "AVAdvertisingManager";
                }
                return m_Instance;
            }
        }

        private Boolean interstitialCreated = false;
        private Boolean videoInterstitialCreated = false;

        public void CreateInterstitial() {

            if (!interstitialCreated)
            {
                interstitialCreated = true;

                #if UNITY_ANDROID
                if(AppConstants.CompileForAmazonAppStore)
                    AndroidInterface.Interstitials.CreateInterstitialWithKey(AppConstants.Amazon.AdMobInterstitialKey);
                else 
                    AndroidInterface.Interstitials.CreateInterstitialWithKey(AppConstants.Android.AdMobInterstitialKey);
                #elif UNITY_IPHONE
                IPhoneInterface.Interstitials.CreateInterstitialWithKey(AppConstants.IOS.AdMobInterstitialKey);
                #endif
            }
        }

        public void CreateVideoInterstitial() {

            if (!videoInterstitialCreated)
            {
                videoInterstitialCreated = true;

                #if UNITY_ANDROID
                if(AppConstants.CompileForAmazonAppStore)
                    AndroidInterface.Interstitials.CreateInterstitialWithKey(AppConstants.Amazon.AdMobVideoKey);
                else 
                    AndroidInterface.Interstitials.CreateVideoInterstitialWithKey(AppConstants.Android.AdMobVideoKey);
                #elif UNITY_IPHONE
                IPhoneInterface.Interstitials.CreateVideoWithKey(AppConstants.IOS.AdMobVideoKey);
                #endif
            }
        }

        public void ShowInterstitial(){
            #if UNITY_ANDROID
            AndroidInterface.Interstitials.ShowInterstitial ();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.ShowInterstitial();
            #endif
        }

        public void LoadInterstitialIfNotAlready(){
            #if UNITY_ANDROID
            AndroidInterface.Interstitials.LoadInterstitialIfNotAlready ();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.LoadInterstitialIfNotAlready();
            #endif
        }

        public bool IsInterstitialReady(){
            #if UNITY_ANDROID
            return AndroidInterface.Interstitials.IsInterstitialReady();
            #elif UNITY_IPHONE
            return IPhoneInterface.Interstitials.IsInterstitialReady();
            #else
            return false;
            #endif
        }

        public void CancelAutoShowInterstitial() {

            #if UNITY_ANDROID
            AndroidInterface.Interstitials.CancelAutoShowInterstitial();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.CancelAutoShowInterstitial();
            #endif
        }

        public void ShowVideoInterstitial(){

            if(!videoInterstitialCreated)
                CreateVideoInterstitial();

            #if UNITY_ANDROID
            AndroidInterface.Interstitials.ShowVideoInterstitial();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.ShowVideo();
            #endif
        }

        public void LoadVideoInterstitialIfNotAlready() {

            if(!videoInterstitialCreated)
                CreateVideoInterstitial();

            #if UNITY_ANDROID
            AndroidInterface.Interstitials.LoadVideoInterstitialIfNotAlready();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.LoadVideoIfNotAlready();
            #endif
        }

        public bool IsVideoInterstitialReady(){

            if(!videoInterstitialCreated)
                CreateVideoInterstitial();

            #if UNITY_ANDROID
            return AndroidInterface.Interstitials.IsVideoInterstitialReady();
            #elif UNITY_IPHONE
            return IPhoneInterface.Interstitials.IsVideoReady();
            #else
            return false;
            #endif


        }

        public void CancelAutoShowVideoInterstitial() {

            if(!videoInterstitialCreated)
                CreateVideoInterstitial();

            #if UNITY_ANDROID
            AndroidInterface.Interstitials.CancelAutoShowVideoInterstitial();
            #elif UNITY_IPHONE
            IPhoneInterface.Interstitials.CancelAutoShowVideo();
            #endif
        }

        // Native function calls

        public System.Action InterstitialPresentedAction;
        public System.Action InterstitialDismissedAction;

        public System.Action DidReceiveVideoReward;


        void ReceivedAdvertAward(string unused) {

            if (DidReceiveVideoReward != null)
                DidReceiveVideoReward();
        }

        void VideoInterstitialIsLoading(string unused) {

        }

        void VideoInterstitialIsReady(string unused) {

        }

        void VideoInterstitialIsNotReady(string unused) {

        }

        void InterstitialIsReady(string unused) {

        }

        void InterstitialIsNotReady(string unused) {

        }

        void VideoInterstitialHasNoFill(string unused) {

            MsgBox.Show (Localization.Instance.GetString(Localization.eLocalKeys.AV_Video_Advert), 
                Localization.Instance.GetString(Localization.eLocalKeys.AV_No_Content_Available_At_This_Time));
        }

        void VideoInterstitialGeneralFail(string unused) {

            MsgBox.Show (Localization.Instance.GetString(Localization.eLocalKeys.AV_Video_Advert), 
                Localization.Instance.GetString(Localization.eLocalKeys.AV_An_Error_Occurred_Try_Later));
        }

        void VideoInterstitialWillPresentScreen(string unused) {

        }

        void VideoInterstitialWillDismiss(string unused) {

        }

        void InterstitialWillPresentScreen(string unused) {

            if (InterstitialPresentedAction != null) { 
                InterstitialPresentedAction ();
            }
        }

        void InterstitialWillDismiss(string unused) {

            if (InterstitialDismissedAction != null) {
                InterstitialDismissedAction ();
            }
        }
    }
}