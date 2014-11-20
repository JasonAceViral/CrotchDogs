using UnityEngine;
using System.Collections;

namespace AceViral {

    public class Localization {

        public enum eLanguages
        {
            Non_Set = 0,
            English,
            psuedo
        }

        public enum eLocalKeys
        {
    		// Create Keys Here

    		AV_Cloud_Out_Of_Sync,
    		AV_Cloud_Would_You_Like_To_Use_Cloud,
    		AV_Cloud_Restore_From,
    		AV_Cloud_Save_Available,
            AV_In_App_Purchases,
            AV_Purchase_Failed_For_x,
			AV_Purchase_Failed_Already_Owned,
			AV_Purchase_Would_You_Like_Restore_Purchases,
			AV_Congratulations_Purchase_Of_x_Successful,
			AV_Rate_This_App,
			AV_Are_You_Enjoying_This_App,
			AV_Would_You_Like_To_Rate,
			AV_Video_Advert,
			AV_No_Content_Available_At_This_Time,
			AV_An_Error_Occurred_Try_Later,

            strings_COUNT
        }

        private static Localization _Instance;

        public static Localization Instance {
            get {
                if (_Instance == null) {
                    _Instance = new Localization ();
    				_Instance.SetLanguage(eLanguages.English);
                }

                return _Instance;
            }
        }

        public event System.Action LanguageHasBeenLoaded;

        void HasLoadedNewLanguage()
        {
            Debug.Log("Localization.HasLoadedNewLanguage");

            if (LanguageHasBeenLoaded != null)
            {
                LanguageHasBeenLoaded();
            }
        }

        private string[] LocalizedStrings;
        private bool languageHasBeenLoaded = false;
        private eLanguages currentlySetLanguage;

        public void SetLanguage(eLanguages setLanguage)
        {
            if (setLanguage == currentlySetLanguage)
                return;

            currentlySetLanguage = setLanguage;

            string fileToLoad = "English";

            switch (setLanguage)
            {
                case eLanguages.English:
                    fileToLoad = "English";
                    break;

                default: 
                    Debug.LogError("Localization.SetLanguage :: Unhandled language: " + setLanguage);
                    return;
            }

            LanguageLoader loader = new LanguageLoader();
    		string[] languageStrings = loader.LoadLanguageXMLFile(fileToLoad);


            if (languageStrings != null)
            {
                LocalizedStrings = languageStrings;
                languageHasBeenLoaded = true;
            }
        }

        public void EnsureHasBeenLoaded()
        {
            if (!languageHasBeenLoaded)
                SetLanguage(eLanguages.English);
        }

        public string GetString(eLocalKeys key)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)key >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }
            return LocalizedStrings[(int)key];
        }

        public string GetStringWithInput(eLocalKeys key, eLocalKeys input1)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)input1 >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            return GetStringWithInput(key, LocalizedStrings[(int)input1]);
        }

        public string GetStringWithInput(eLocalKeys key, string input1)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)key >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            // Split the string by wildcard
            string[] substrings = LocalizedStrings[(int)key].Split (new string [] { "@1" }, System.StringSplitOptions.None);

            if(substrings.Length <= 1)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be formatted.");
                return "String Missing.";
            }

            string result = substrings[0] + input1 + substrings[1];
            return result;
        }

        public string GetStringWithInput(eLocalKeys key, eLocalKeys input1, string input2)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)input1 >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            return GetStringWithInput(key, LocalizedStrings[(int)input1], input2);
        }

        public string GetStringWithInput(eLocalKeys key, string input1, eLocalKeys input2)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)input2 >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            return GetStringWithInput(key, input1, LocalizedStrings[(int)input2]);
        }

        public string GetStringWithInput(eLocalKeys key, eLocalKeys input1, eLocalKeys input2)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)input1 >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            if ((int)input2 >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            return GetStringWithInput(key, LocalizedStrings[(int)input1], LocalizedStrings[(int)input2]);
        }

        public string GetStringWithInput(eLocalKeys key, string input1, string input2)
        {
            //return " ";
    		if (LocalizedStrings == null) {
    			Debug.LogError("Localization.GetLocalizedStringWithInput :: LocalizedStrings was null.");
    			return "String Missing.";
    		}
            if ((int)key >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            string nextStep = GetStringWithInput(key, input1);

            // Split the string by wildcard
            string[] substrings = nextStep.Split (new string [] { "@2" }, System.StringSplitOptions.None);

            if(substrings.Length <= 1)
            {
                Debug.LogError("Localization.GetLocalizedStringWithInput :: Localized string could not be formatted.");
                return "String Missing.";
            }

            string result = substrings[0] + input2 + substrings[1];
            return result;
        }

        public string GetStringWithInput(eLocalKeys key, string input1, string input2, string input3)
        {
            //return " ";
            if (LocalizedStrings == null) {
                Debug.LogError("Localization.GetStringWithInput :: LocalizedStrings was null.");
                return "String Missing.";
            }
            if ((int)key >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            string nextStep = GetStringWithInput(key, input1, input2);

            // Split the string by wildcard
            string[] substrings = nextStep.Split (new string [] { "@3" }, System.StringSplitOptions.None);

            if(substrings.Length <= 1)
            {
                Debug.LogError("Localization.GetStringWithInput :: Localized string could not be formatted.");
                return "String Missing.";
            }

            string result = substrings[0] + input3 + substrings[1];
            return result;
        }

        public string GetStringFormatted(eLocalKeys key)
        {
            //return " ";
            if (LocalizedStrings == null) {
                Debug.LogError("Localization.GetStringWithInput :: LocalizedStrings was null.");
                return "String Missing.";
            }
            if ((int)key >= LocalizedStrings.Length)
            {
                Debug.LogError("Localization.GetStringWithInput :: Localized string could not be found.");
                return "String Missing.";
            }

            string result = LocalizedStrings[(int)key];

            // Check for and symbol
            if (result.Contains("@and"))
            {
                // Split the string by formatting
                string[] substrings =result.Split(new string [] { "@and" }, System.StringSplitOptions.None);

                if (substrings.Length <= 1)
                {
                    Debug.LogError("Localization.GetStringWithInput :: Localized string could not be formatted.");
                    return "String Missing.";
                }

                result = substrings[0];

                for (int i = 1; i < substrings.Length; i++)
                {
                    result = "&" + substrings[i];
                }
            }
            return result;
        }
    }
}
