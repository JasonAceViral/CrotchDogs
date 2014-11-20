using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class GooglePlayInterface : GameServicesInterface {

        public override bool IsAvailable()
        {
            return AndroidInterface.GameServices.IsAvailable();
        }

        public override bool IsSignedIn()
        {
            return AndroidInterface.GameServices.IsSignedIn();
        }

        public override void SignIn()
        {
            AndroidInterface.GameServices.SignIn();
        }

        public override void SignOut()
        {
            AndroidInterface.GameServices.SignOut();
        }

        public override void ShowLeaderboards()
        {
            AndroidInterface.GameServices.ShowLeaderboards();
        }

        public override void ShowLeaderboard(GSLeaderboard leaderboardId)
        {
            AndroidInterface.GameServices.ShowLeaderboard(leaderboardId.AndroidId);
        }

        public override void ShowAchievements()
        {
            AndroidInterface.GameServices.ShowAchievements();
        }

        public override void UpdateAchievement(GSAchievement achievement, float progress, int steps)
        {
            AndroidInterface.GameServices.UnlockAchievement(achievement.AndroidId, progress, steps);
        }

        public override void UpdateScore(GSLeaderboard leaderboard, float score)
        {
            AndroidInterface.GameServices.UpdateLeaderboard(leaderboard.AndroidId, score);
        }

        // Invoked from native code
        void SignInComplete(string success) {
            if (DidSignIn != null)
                DidSignIn(success == "success");
        }
    }
}