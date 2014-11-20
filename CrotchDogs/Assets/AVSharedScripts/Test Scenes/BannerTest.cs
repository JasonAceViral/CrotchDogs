using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class BannerTest : MonoBehaviour {

        public AVtk2dButton createBanner, loadAdvert, showBanner, hideAdvert, getHeight;
        public AVtk2dButton setConfigTopLeft, setConfigTop, setConfigTopRight, setConfigBottomLeft, setConfigBottom, setConfigBottomRight;
        public GUIText outputText;

        // Use this for initialization
        void Start () {
        
            createBanner.UIItem.OnClick += () =>
            {
                outputText.text = "Create Banner";
                AVAdMobManager.Instance.Init();
            };

            loadAdvert.UIItem.OnClick += () =>
            {
                outputText.text = "Load Banner";
                AVAdMobManager.Instance.LoadNewBanner();
            };

            showBanner.UIItem.OnClick += () =>
            {
                outputText.text = "Show Banner";
                AVAdMobManager.Instance.ShowBanner();
            };

            hideAdvert.UIItem.OnClick += () =>
            {
                outputText.text = "Hide Banner";
                AVAdMobManager.Instance.HideBanner();
            };

            getHeight.UIItem.OnClick += () =>
            {
                outputText.text = "Get Height";
                outputText.text = "Advert Height: " + AVAdMobManager.Instance.GetAdvertHeight();
            };

            setConfigTopLeft.UIItem.OnClick += () =>
            {
                outputText.text = "Config Top Left";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigTopLeft);
            };

            setConfigTop.UIItem.OnClick += () =>
            {
                outputText.text = "Config Top";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigTopCenter);
            };

            setConfigTopRight.UIItem.OnClick += () =>
            {
                outputText.text = "Config Top Right";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigTopRight);
            };

            setConfigBottomLeft.UIItem.OnClick += () =>
            {
                outputText.text = "Config Bottom Left";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigBottomLeft);
            };

            setConfigBottom.UIItem.OnClick += () =>
            {
                outputText.text = "Config Bottom";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigBottomCenter);
            };

            setConfigBottomRight.UIItem.OnClick += () =>
            {
                outputText.text = "Config Bottom Right";
                AVAdMobManager.Instance.SetBannerConfiguration(AVAdPositionConfiguration.eAdConfigBottomRight);
            };
        }
    }
}
