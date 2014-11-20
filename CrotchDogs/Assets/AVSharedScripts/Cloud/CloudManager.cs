using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AVHiddenInterface;

namespace AceViral {
		public class CloudManager : MonoBehaviour {


	    private bool m_IsAvailable = false;
	    public bool IsAvailable { get{ return m_IsAvailable;  } }

	    private bool cloudUpdateAvailable = false;
	    private bool CloudIsOutOfSync = false;

	    public System.Action OnUpdateFromCloud;

	    private static CloudManager _Instance;

	    public static CloudManager Instance {
			get {
				if (_Instance == null) {
	                _Instance = new GameObject().AddComponent<CloudManager> ();
                    _Instance.gameObject.name = "AVCloudManager";
	                DontDestroyOnLoad(_Instance.transform.gameObject);
	                _Instance.CheckAvailability();
				}

				return _Instance;
			}
		}

	    private string GetCloudSyncKey()
	    {
	        return "CloudManager.DeviceSync." + SystemInfo.deviceUniqueIdentifier;
	    }

	    public void FetchCloudData()
	    {
            // Only Android has to fetch cloud, as iOS does this in background even when the app is closed
	        #if UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.GameServices.Cloud.FetchData();
	        #endif
	    }

	    public bool CheckAvailability()
	    {
	        #if UNITY_IPHONE
            m_IsAvailable = IPhoneInterface.Cloud.IsAvailable();
	        #elif UNITY_ANDROID
            m_IsAvailable = AVHiddenInterface.AndroidInterface.GameServices.Cloud.IsAvailable();
	        #endif
	        return m_IsAvailable;
	    }

	    public Dictionary<string,string> LoadAllData()
	    {
	        #if UNITY_EDITOR
	        return null;
	        #endif

	        if (!m_IsAvailable)
	        {
	            return null;
	        }

	        cloudUpdateAvailable = false;
	        string allData = "";
	            
            #if UNITY_IPHONE
            allData = IPhoneInterface.Cloud.LoadAllData();
	        #elif UNITY_ANDROID
            allData = AVHiddenInterface.AndroidInterface.GameServices.Cloud.LoadAllData();
	        #endif


	        Dictionary<string,string> dict = new Dictionary<string,string>();

	        if (string.IsNullOrEmpty(allData))
	        {
	            return null;
	        }

	        string[] array = allData.Split(new string[] { "|_|" }, System.StringSplitOptions.None);

	        for (int i = 0; i < array.Length; i++)
	        {
	            string[] splitInfo = array[i].Split(new string[] { "|-|" }, System.StringSplitOptions.None);
	            if (splitInfo.Length == 2)
	            {
	                string key = splitInfo[0];
	                string data = splitInfo[1];

	                dict.Add(key, data);
	            }
	        }

	        // Load custom sync variable
	        if (dict.ContainsKey(GetCloudSyncKey()))
	        {
                //Debug.Log("Cloud sync contains key..");

	            string value = dict[GetCloudSyncKey()];
	            if (!string.IsNullOrEmpty(value))
	            {
                    //Debug.Log("Cloud sync contains key & value..");

	                System.DateTime cloudTime = System.DateTime.Parse(value);
	                string timeString = Prefs.GetString(GetCloudSyncKey(), "");

                    //Debug.Log("Cloud sync values. cloud " + value + " and local " + timeString);
	                if (!string.IsNullOrEmpty(timeString) && cloudTime < System.DateTime.Parse(timeString))
	                {
	                    CloudIsOutOfSync = true;
						MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_Cloud_Out_Of_Sync), 
							Localization.Instance.GetString(Localization.eLocalKeys.AV_Cloud_Would_You_Like_To_Use_Cloud), MsgBoxType.YES_NO, (MsgBoxResponse response)=> {

	                        if(response == MsgBoxResponse.YES)
	                        {
	                            CloudIsOutOfSync = false;
	                            Prefs.SetString(GetCloudSyncKey(), cloudTime.ToString());
	                            Prefs.Save();

	                            if (OnUpdateFromCloud != null)
	                            {
	                                OnUpdateFromCloud();
	                            }
	                        }
	                    });
	                    return null;
	                }
	            }
	        }
	            
	        return dict;
	    }

        public string LoadDataFromKey(string key)
	    {
	        if (!m_IsAvailable)
	        {
	            return null;
	        }

	        #if UNITY_IPHONE
            return IPhoneInterface.Cloud.LoadDataForKey(key);
	        #elif UNITY_ANDROID
	        Debug.LogError("Android platform does not implement this function LoadDataFromKey. Use LoadAllData()");
	        #endif

	        return "";
	    }

	    public void SaveDictionary(Dictionary<string,string> dict)
	    {
	        if (!m_IsAvailable || !Prefs.GetBool("CloudManager.UserWantsCloud", true))
	        {
	            return;
	        }

	        string parseData = "";

	        foreach (KeyValuePair<string,string> kv in dict)
	        {
	            if (parseData.Length > 0)
	            {
	                parseData += "|_|";
	            }

	            parseData += kv.Key + "|-|" + kv.Value;
	        }

	        // Save custom sync variable
	        string timeNow = System.DateTime.Now.ToString();
	        parseData += "|_|" + GetCloudSyncKey() + "|-|" + timeNow;
	        Prefs.SetString(GetCloudSyncKey(), timeNow);
	        Prefs.Save();

	        #if UNITY_IPHONE
	        if (!CloudIsOutOfSync)
                IPhoneInterface.Cloud.SaveDictionaryData(parseData);

	        #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.GameServices.Cloud.SaveDictionaryData(parseData);
	        #endif
	    }

        public void SaveDataToKey(string key, string data)
	    {
	        if (!m_IsAvailable || !Prefs.GetBool("CloudManager.UserWantsCloud", true))
	        {
	            return;
	        }

	        #if UNITY_IPHONE
	        if (!CloudIsOutOfSync)
                IPhoneInterface.Cloud.SaveDataForKey(key, data);
	        #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.GameServices.Cloud.SaveKey(key, data);
	        #endif
	    }

	    public void Synchronize()
	    {
	        if (CloudIsOutOfSync)
	            return;

	        #if UNITY_IPHONE
            IPhoneInterface.Cloud.Synchronize();
	        #elif UNITY_ANDROID
            AVHiddenInterface.AndroidInterface.GameServices.Cloud.Synchronize();
	        #endif
	    }

	    public void CheckForCloudRestore()
	    {
	        Debug.LogWarning("CloudManager :: CheckForCloudRestore. Update available? " + cloudUpdateAvailable + " Questioned already? " + Prefs.GetBool("CloudManager.UserAnsweredRestore", false));

	        if (cloudUpdateAvailable)
	        {
	            if (!Prefs.GetBool("CloudManager.UserAnsweredRestore", false))
	            {
						MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_Cloud_Restore_From), 
							Localization.Instance.GetString(Localization.eLocalKeys.AV_Cloud_Save_Available), MsgBoxType.YES_NO, (MsgBoxResponse response) =>
	                {
	                    if (response == MsgBoxResponse.YES)
	                    {
	                        Prefs.SetBool("CloudManager.UserAnsweredRestore", true);
	                        Prefs.SetBool("CloudManager.UserWantsCloud", true);
	                        if (OnUpdateFromCloud != null)
	                        {
	                            OnUpdateFromCloud();
	                        }
	                    }
	                    else
	                    {
	                        Prefs.SetBool("CloudManager.UserWantsCloud", false);
	                    }
	                    Prefs.Save();
	                }); 
	            }
	            else if (OnUpdateFromCloud != null)
	            {
	                OnUpdateFromCloud();
	            }
	        }
	    }

	    // Invoked by native code
	    void CloudUpdateAvailable(string data)
	    {
	        Debug.LogWarning("CloudManager :: CloudUpdateAvailable");
	        cloudUpdateAvailable = true;

	        if (Prefs.GetBool("CloudManager.UserWantsCloud", true))
	        {
	            if(OnUpdateFromCloud != null)
	            {
	                OnUpdateFromCloud();
	            }
	        }
	        else Debug.Log("Cloud has not used the update as user does not want cloud!");
	    }
	}
}
