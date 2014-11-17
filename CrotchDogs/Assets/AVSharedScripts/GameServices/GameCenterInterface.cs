using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class GameCenterInterface : GameServicesInterface {


        public override bool IsAvailable()
        {
            #if UNITY_IPHONE
            return IPhoneInterface.GameCenter.IsAvailable();
            #endif
            return false;
        }

        public override bool IsSignedIn()
        {
            #if UNITY_IPHONE
            return IPhoneInterface.GameCenter.IsSignedIn();
            #endif
            return false;
        }

        public override void SignIn()
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.Authenticate();
            #endif
        }

        public override void SignOut()
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.SignOut();
            #endif
        }

        public override void ShowLeaderboards() 
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.ShowLeaderboards();
            #endif
        }

        public override void ShowLeaderboard(GSLeaderboard leaderboard)
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.ShowLeaderboard(leaderboard.iOSId);
            #endif
        }

        public override void ShowAchievements()
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.ShowAchievements();
            #endif
        }

        public override void UpdateAchievement(GSAchievement achievement, float progress, int steps)
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.PostAchievement(progress, achievement.iOSId);
            #endif
        }

        public override void UpdateScore(GSLeaderboard leaderboard, float score)
        {
            #if UNITY_IPHONE
            IPhoneInterface.GameCenter.PostScore(score, leaderboard.iOSId);
            #endif
        }

        // Invoked from native code
        void SignInComplete(string success) {
            if (DidSignIn != null)
                DidSignIn(success == "success");
        }
    }
}
