using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class InterstitialTest : MonoBehaviour {

        public AVtk2dButton createInterstitials, createVideos;
        public AVtk2dButton isInterstitialReady, showInterstitial, ensureLoadInterstitial, cancelAutoShowInterstitial;
        public AVtk2dButton isVideoReady, showVideo, ensureLoadVideo, cancelAutoShowVideo;
        public GUIText outputText;

        // Use this for initialization
        void Start () {

            createInterstitials.UIItem.OnClick += () =>
            {
                outputText.text = "Create Interstitial";
                AVInterstitialManager.Instance.CreateInterstitial();
            };

            isInterstitialReady.UIItem.OnClick += () =>
            {
                outputText.text = "Is Interstitial Ready? " + AVInterstitialManager.Instance.IsInterstitialReady();
            };

            showInterstitial.UIItem.OnClick += () =>
            {
                outputText.text = "Show Interstitial";
                AVInterstitialManager.Instance.ShowInterstitial();
            };

            ensureLoadInterstitial.UIItem.OnClick += () =>
            {
                outputText.text = "Ensure Interstitial Is Loading";
                AVInterstitialManager.Instance.LoadInterstitialIfNotAlready();
            };

            cancelAutoShowInterstitial.UIItem.OnClick += () =>
            {
                outputText.text = "Cancel Interstitial Autoshow";
                AVInterstitialManager.Instance.CancelAutoShowInterstitial();
            };

            createVideos.UIItem.OnClick += () =>
            {
                outputText.text = "Create Video";
                AVInterstitialManager.Instance.CreateVideoInterstitial();
            };

            isVideoReady.UIItem.OnClick += () =>
            {
                outputText.text = "Is Video Ready? " + AVInterstitialManager.Instance.IsVideoInterstitialReady();
            };

            showVideo.UIItem.OnClick += () =>
            {
                outputText.text = "Show Video";
                AVInterstitialManager.Instance.ShowVideoInterstitial();
            };

            ensureLoadVideo.UIItem.OnClick += () =>
            {
                outputText.text = "Ensure Video Loading";
                AVInterstitialManager.Instance.LoadVideoInterstitialIfNotAlready();
            };

            cancelAutoShowVideo.UIItem.OnClick += () =>
            {
                outputText.text = "Cancel Auto Show Video";
                AVInterstitialManager.Instance.CancelAutoShowVideoInterstitial();
            };
        }
    }
}
