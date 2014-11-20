using UnityEngine;
using System.Collections;

namespace AceViral {

    public class GSAchievement {

        public string iOSId = "not set";
        public string AndroidId = "not set";
        public string AmazonId = "not set";
    }

    public class GSLeaderboard {

        public string iOSId = "not set";
        public string AndroidId = "not set";
        public string AmazonId = "not set";
    }

    public abstract class GameServicesInterface : MonoBehaviour {

		private static GameServicesInterface m_Instance;

		public static GameServicesInterface Instance {
			get {
				if (m_Instance == null) {
                    #if UNITY_EDITOR
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.GameServicesEditorInterface>();    
                    #elif UNITY_ANDROID
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.GooglePlayInterface>();	
					#elif UNITY_IPHONE
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.GameCenterInterface>();		
					#else
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.GameServicesEditorInterface>();      
					#endif
                    m_Instance.name = "AVGameServicesInterface";
				}
				return m_Instance;
			}
		}

        public System.Action<bool> DidSignIn;

        public abstract bool IsAvailable();

        public abstract bool IsSignedIn();

        public abstract void SignIn();

        public abstract void SignOut();

        public abstract void ShowLeaderboards();

        public abstract void ShowLeaderboard(GSLeaderboard leaderboard);

        public abstract void ShowAchievements();

        public abstract void UpdateAchievement(GSAchievement achievement, float progress, int steps);

        public abstract void UpdateScore(GSLeaderboard leaderboard, float score);
	}
}
