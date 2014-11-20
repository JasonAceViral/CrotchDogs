using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace AceViral {

    public class LanguageLoader
    {
        private string[] loadedLanguage;

        public string[] LoadLanguageXMLFile(string fileName)
        {
            TextAsset textAsset;
            string path = "Localization/" + fileName;

            Debug.Log("Attempting to load XML File: " + path);

            textAsset = Resources.Load(path) as TextAsset;

            if (textAsset == null)
            {
                Debug.LogError("LanguageLoader.LoadLanguageXMLFile :: Error loading in XML Language file '" + fileName + "'");
                return null;
            }

            MemoryStream assetStream = new MemoryStream(textAsset.bytes);

            loadedLanguage = new string[(int)Localization.eLocalKeys.strings_COUNT];

            int currentIndex = 0;
            bool skippedFirstElement = false;

    		try{
                using (XmlTextReader reader = new XmlTextReader(assetStream))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if(!skippedFirstElement && reader.Name == "LanguageData")
                            {
                                reader.Read();
                                reader.Read();
                                skippedFirstElement = true;
                            }

                            // Keys should be in order so first try next index
                            Localization.eLocalKeys enumVal = (Localization.eLocalKeys)currentIndex;
                            string key = enumVal.ToString();

                            if (reader.Name == key)
                            {
                                reader.Read();
                                loadedLanguage[currentIndex] = reader.Value;
                                currentIndex++;
                            }
                            else
                            {
                                Debug.Log("Langauge Loader: Unexpected missed key value. Key: " + enumVal);

                                // Iterate whole enum to try and find the key
                                for (int i = 0; i < (int)Localization.eLocalKeys.strings_COUNT; i++)
                                {
            						enumVal = (Localization.eLocalKeys)i;
            						key = enumVal.ToString();

                                    if (reader.Name == key)
                                    {
                                        reader.Read();
                                        loadedLanguage[i] = reader.Value;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
    		} catch (System.Exception e){
    			Debug.LogError ("LanguageLoader.LoadLanguageXMLFile: Exception: " + e.Message);
    		}
            return loadedLanguage;
        }
    }
}
