using UnityEngine;
using System.Collections;
using AceViral;

namespace AVHiddenInterface {
    public class GameServicesEditorInterface : GameServicesInterface {

        public override bool IsAvailable()
        {
            Debug.Log ("GameServicesEditorInterface :: IsAvailable"); 
            return true;
        }

        public override bool IsSignedIn()
        {
            Debug.Log ("GameServicesEditorInterface :: IsSignedIn"); 
            return true;
        }

        public override void SignIn() 
        { 
            Debug.Log ("GameServicesEditorInterface :: SignIn"); 
        }

        public override void SignOut()
        {
            Debug.Log ("GameServicesEditorInterface :: SignOut"); 
        }

        public override void ShowLeaderboards() 
        { 
            Debug.Log ("GameServicesEditorInterface :: ShowLeaderboards"); 
        }

        public override void ShowLeaderboard(GSLeaderboard leaderboard) 
        { 
            Debug.Log ("GameServicesEditorInterface :: ShowLeaderboard. <LeaderboardId(iOS) : " + leaderboard.iOSId + ">"); 
        }

        public override void ShowAchievements() 
        { 
            Debug.Log ("GameServicesEditorInterface :: ShowAchievements"); 
        }

        public override void UpdateAchievement(GSAchievement achievement, float progress, int steps)
        { 
            Debug.Log ("GameServicesEditorInterface :: UpdateAchievement. <Achievement(iOS) : " + achievement.iOSId + "> <Progress : " + progress + "> <Steps : " + steps + " >"); 
        }

        public override void UpdateScore(GSLeaderboard leaderboard, float score) 
        { 
            Debug.Log ("GameServicesEditorInterface :: UpdateScore. <Score : " + score + "> <Leaderboard(iOS) : " + leaderboard.iOSId + ">"); 
        }
    }
}
