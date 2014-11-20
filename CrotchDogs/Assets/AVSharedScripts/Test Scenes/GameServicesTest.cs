using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class GameServicesTest : MonoBehaviour {

        public AVtk2dButton signIn, signOut, isSignedIn, isAvailable;
        public AVtk2dButton showLeaderboards, showLeaderboard, showAchievements, updateAchievement, updateLeaderboard;
        public GUIText outputText;

    	// Use this for initialization
    	void Start () {

            GameServicesInterface.Instance.DidSignIn += (bool success) =>
            {
                outputText.text = "Did Sign In: " + success;
            };

            signIn.UIItem.OnClick += () =>
            {
                outputText.text = "Sign In";
                GameServicesInterface.Instance.SignIn();
            };

            signOut.UIItem.OnClick += () =>
            {
                outputText.text = "Sign Out";
                GameServicesInterface.Instance.SignOut();
            };

            isSignedIn.UIItem.OnClick += () =>
            {
                outputText.text = "Is Signed In: " + GameServicesInterface.Instance.IsSignedIn();
            };

            isAvailable.UIItem.OnClick += () =>
            {
                outputText.text = "Is Available: " + GameServicesInterface.Instance.IsAvailable();
            };

            showLeaderboards.UIItem.OnClick += () =>
            {
                outputText.text = "Show leaderboards";
                GameServicesInterface.Instance.ShowLeaderboards();
            };

            showLeaderboard.UIItem.OnClick += () =>
            {
                outputText.text = "Show leaderboard";
                GameServicesInterface.Instance.ShowLeaderboard(AceViral.AppConstants.Leaderboards[0]);
            };

            showAchievements.UIItem.OnClick += () =>
            {
                outputText.text = "Show Achievements";
                GameServicesInterface.Instance.ShowAchievements();
            };

            updateAchievement.UIItem.OnClick += () =>
            {
                outputText.text = "Update Achievement";
                GameServicesInterface.Instance.UpdateAchievement(AceViral.AppConstants.Achievements[0], 0.5f, 5);
            };

            updateLeaderboard.UIItem.OnClick += () =>
            {
                outputText.text = "Soft Sign In";
                GameServicesInterface.Instance.UpdateScore(AceViral.AppConstants.Leaderboards[0], 500f);
            };
    	}
    }
}
