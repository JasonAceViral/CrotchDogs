using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AceViral {
    [ExecuteInEditMode]
    public class LocalizedStringSetter : MonoBehaviour {

        public tk2dTextMesh TextToLocalize;
        public Localization.eLocalKeys StringKey;
        public bool UseFormattedString = false;

        void Start () {

            #if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            #endif
            {
                SetLocalizedString();
                Localization.Instance.LanguageHasBeenLoaded += SetLocalizedString;
            }
        }

        void SetLocalizedString()
        {
            if (TextToLocalize == null)
            {
                Debug.LogError("TextMesh object is null. GameObject: " + gameObject.name);
                return;
            }

            if (TextToLocalize != null)
            {
                if(UseFormattedString)
                    TextToLocalize.text = Localization.Instance.GetStringFormatted(StringKey);
                else
                    TextToLocalize.text = Localization.Instance.GetString(StringKey);
            }
        }

        #if UNITY_EDITOR
        void Update() 
        {
            if (!EditorApplication.isPlaying)
            {
                if (TextToLocalize == null)
                {
                    TextToLocalize = GetComponent<tk2dTextMesh>();
                }
            }
        }
        #endif
    }
}
