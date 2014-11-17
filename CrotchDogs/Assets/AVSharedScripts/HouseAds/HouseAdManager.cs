using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace AceViral {

    public delegate void EventAdData ();
    public delegate void EventAdImage (string slotId);

    public class HouseAdManager : MonoBehaviour
    {
        public event EventAdData advertDataUpdated;
        public event EventAdImage advertImageDownloaded;

        protected virtual void AdvertDataUpdated()
        {
            if (advertDataUpdated != null)
            {
                advertDataUpdated();
            }
        }

        protected virtual void AdvertImageDownloaded(string slotId)
        {
            if (advertImageDownloaded != null)
            {
                advertImageDownloaded(slotId);
            }
        }

        public class HouseAdUrls
        {
            public const string
                iOS = AppConstants.IOS.HouseAdLink,
                Android = AppConstants.Android.HouseAdLink,
                Amazon = AppConstants.Amazon.HouseAdLink,
                Mac = "",
                Windows8 = AppConstants.Metro.HouseAdLink,
                WindowsPhone8 = AppConstants.WP8.HouseAdLink,
                WebPlayer = "",
                Linux = "";

            public static string GetUrlForCurrentPlatform()
            {
    #if UNITY_IPHONE
    			return iOS;
    #elif UNITY_ANDROID
                return Android;
                //return (GameConstants.CompileForAmazonAppStore) ? Amazon : Android;
    #elif UNITY_STANDALONE_OSX
    			return Mac;
    #elif UNITY_WEBPLAYER
    			return WebPlayer;
    #elif UNITY_STANDALONE_WIN
    			return Windows8;
    #elif UNITY_STANDALONE_LINUX
    			return Linux;
    #elif UNITY_METRO
                return Windows8;
    #elif UNITY_WP8
                return WindowsPhone8;
    #endif
            }
        }

        #region Singleton

        private static HouseAdManager m_Manager;
        // Always use the ad manager through AVHouseAdManager.Manager
        public static HouseAdManager Manager
        {
            get
            {
                if (m_Manager == null)
                {
                    // Creates a new gameobject in the scene and adds the ad manager component
                    m_Manager = new GameObject().AddComponent<HouseAdManager>();
                    m_Manager.gameObject.name = "AVHouseAdManager";

                    // This keeps the object alive between scenes
                    GameObject.DontDestroyOnLoad(m_Manager.gameObject);
                }
                return m_Manager;
            }
        }

        #endregion

        private const int MINUTES_BEFORE_XML_REFRESH = 60;
        private List<HouseAd> m_AdvertList;
        private string m_XmlSrc = string.Empty;
        private bool m_IsDownloadingNewXML = false;
        private bool m_IsDownloadingImage = false;
        private string cachePath = null;
        private HouseAd m_CurrentAdvert;
        private HouseAd m_CurrentFullscreenAdvert;
        private List<HouseAdSlot> m_AdSlots;
        private string xmlCacheFilePath = null;

        #region Interface

        public void GetXMLFromServer(bool force)
        {
            LoadXmlFromCache();
    		
            if (force || RequiresXmlUpdate() && HouseAdUrls.GetUrlForCurrentPlatform() != "")
            {
                StartCoroutine(DownloadXmlFromServer());
            }
        }

        private void LoadXmlFromCache()
        {
            //Load XML from cache is exists
    #if UNITY_ANDROID
            if (cachePath == null)
                cachePath = System.IO.Path.Combine(Application.persistentDataPath, "HouseAds/");
            xmlCacheFilePath = Path.Combine(cachePath, "advertCache.xml");
            if (File.Exists(xmlCacheFilePath))
                m_XmlSrc = File.ReadAllText(xmlCacheFilePath);
    #elif UNITY_METRO || UNITY_WP8 || UNITY_WEBPLAYER
            // Unity webplayer / WinStore can't access local stuff so we can't load any saved ads
    #else
    		if (cachePath == null)
    			cachePath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "HouseAds/");
    		xmlCacheFilePath = Path.Combine (cachePath, "advertCache.xml");
    		if(File.Exists(xmlCacheFilePath))
    			m_XmlSrc = File.ReadAllText(xmlCacheFilePath);
    #endif
            if (m_XmlSrc == string.Empty)
                Debug.Log("AVHouseAdManager: XML Cache Not Found.");
            else
            {
                Debug.Log("AVHouseAdManager: XML Cache Loaded.");
                ParseXmlFile();
                AdvertDataUpdated();
    			
                // Load images from cache
                if (cachePath == null)
                    cachePath = System.IO.Path.Combine(Application.persistentDataPath, "HouseAds/");
    			
                for (int i = 0; i < m_AdvertList.Count; i++)
                {
                    HouseAd advert = m_AdvertList[i];
    				
                    // Construct image name
                    string name = advert.slotId + advert.updateTime + ".png";
                    string path = string.Empty;
    #if UNITY_ANDROID
                    path = Path.Combine(cachePath, name);
                    advert.image = GetLocalImage(path);
                    AdvertImageDownloaded(advert.slotId);
    #elif UNITY_METRO || UNITY_WP8 || UNITY_WEBPLAYER
            		// Unity webplayer / WinStore can't access local stuff so we can't load any saved ads
    #else
    				if (cachePath == null)
    					cachePath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "HouseAds/");
    				path = Path.Combine (cachePath, name);
    				advert.image = GetLocalImage (path);
                    AdvertImageDownloaded(advert.slotId);
    #endif
                }
            }
        }

        private bool RequiresXmlUpdate()
        {
    #if UNITY_WP8 || UNITY_METRO    
            return true;
    #endif

            string strTimeOfLastUpdate = PlayerPrefs.GetString("TimeOfLastAdvertUpdate");
            if (strTimeOfLastUpdate == string.Empty)
            {
                return true;
            }

            if (m_XmlSrc == null)
            {
                return true;
            }

            DateTime timeOfLastUpdate = DateTime.Parse(strTimeOfLastUpdate);
            TimeSpan span = DateTime.Now - timeOfLastUpdate;

            if (span.TotalMinutes >= MINUTES_BEFORE_XML_REFRESH)
            {
                return true;
            }
            return false;
        }

        private void Update()
        {
            // Download images in the background
            if (!m_IsDownloadingNewXML && !m_IsDownloadingImage)
            {
                // Cycle ads and download if needed
                if (m_AdvertList != null)
                {
                    for (int i = 0; i < m_AdvertList.Count; i++)
                    {
                        if (m_AdvertList[i].active && m_AdvertList[i].image == null)
                        {
                            StartCoroutine(DownloadImageForAdvert(m_AdvertList[i]));
                            break;
                        }
                    }
                }
            }
        }

        public HouseAd GetAdFromSlot(string slot)
        {
            return GetAdsWithSlotID(slot).GetNextAdvert();
        }

        #endregion

        private HouseAdSlot GetAdsWithSlotID(string slot)
        {
            HouseAdSlot newSlot = new HouseAdSlot();
            newSlot.Name = slot;
            if (m_AdSlots != null)
            {
                foreach (HouseAdSlot adSlot in m_AdSlots)
                {
                    if (adSlot.Name == slot)
                        return adSlot;
                }

                if (m_AdvertList != null)
                {
                    foreach (HouseAd advert in m_AdvertList)
                    {
                        if (advert.slotId.Contains(slot))
                        {
                            if (advert.defaultAd)
                            {
                                newSlot.DefaultAdvert = advert;
                            }
                            else
                            {
                                newSlot.AddAdvert(advert);
                            }
                        }
                    }
                }
                newSlot.Reverse();
                m_AdSlots.Add(newSlot);
            }
            else
            {
                Debug.LogError("AVHouseAdManager: GetAdsWithSlotID() - Ad slots list was null!!");
                m_AdSlots = new List<HouseAdSlot>();
            }
            return newSlot;
        }

        public HouseAd GetAdvertAtSlot(string slot)
        {
            return GetAdvertAtSlot(slot, false);
        }

        public HouseAd GetAdvertAtSlot(string slot, bool allowNullImage)
        {
            if (m_AdvertList != null)
            {
                foreach (HouseAd ad in m_AdvertList)
                {
                    if (ad.slotId.Contains(slot))
                    {
                        if (!allowNullImage && ad.image == null)
                        {
                            Debug.Log("AVHouseAdManager: AVHouseAd slot found for id '" + slot + "' but no image has downloaded yet.");
                            return null;
                        }
                        return ad;
                    }
                }
                Debug.Log("AVHouseManager: Failed to find a matching AVHouseAd slot for Id '" + slot + "'.");
            }
            return null;
        }

        private IEnumerator DownloadImageForAdvert(HouseAd advert)
        {
            if (m_IsDownloadingImage || !advert.active || advert.image != null)
            {
                yield break;
            }

            m_IsDownloadingImage = true;

            // Construct image name
            string name = advert.slotId + advert.updateTime + ".png";
            string path = string.Empty;
    #if UNITY_ANDROID
            if (cachePath == null)
                cachePath = System.IO.Path.Combine(Application.persistentDataPath, "HouseAds/");
            path = Path.Combine(cachePath, name);
            advert.image = GetLocalImage(path);
    #elif UNITY_METRO || UNITY_WP8 || UNITY_WEBPLAYER
            // Unity webplayer / WinStore can't access local stuff so we can't load any saved ads
    #else
    		if (cachePath == null)
    			cachePath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "HouseAds/");
    		path = Path.Combine (cachePath, name);
    		advert.image = GetLocalImage (path);
    #endif

            if (advert.image == null)
            {
                yield return StartCoroutine(DownloadImageToAdvert(advert, path));
            }

            m_IsDownloadingImage = false;
            yield break;
        }

        private Texture2D GetLocalImage(string path)
        {
    #if UNITY_WEBPLAYER || UNITY_METRO || UNITY_WP8
            // Unity webplayer / WinStore can't access local stuff so we can't load any saved ads
    #else
            if (File.Exists(path))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                Texture2D imageTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                imageTex.LoadImage(imageBytes);
    			
                // Sometimes our ad images come in corrupted so check for this
                if (imageTex != null && imageTex.width > 20 && imageTex.height > 20)
                {
                    return imageTex;
                }
                Debug.LogError("AVHouseAdManager.GetLocalImage() - Error loading image: " + path);
            }
    #endif
            return null;
        }

        private IEnumerator DownloadImageToAdvert(HouseAd advert, string savePath)
        {
            WWW imageDownload = new WWW(advert.imageURL);
            yield return imageDownload;

            if (imageDownload.error != null)
            {
                Debug.LogError("Image Download error: " + imageDownload.error);
            }
            else
            {
                Texture2D texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                imageDownload.LoadImageIntoTexture(texture);

                advert.image = texture;
    #if UNITY_WEBPLAYER || UNITY_METRO || UNITY_WP8
                // Unity webplayer / WinStore can't access local stuff so we can't write our ads
    #else
                // Store if downloaded
                Directory.CreateDirectory(cachePath);
                FileStream cache = new System.IO.FileStream(savePath, System.IO.FileMode.Create);
                BinaryWriter w = new BinaryWriter(cache);
                w.Write(advert.image.EncodeToPNG());
                w.Close();
                cache.Close();
    #endif
                //Notify observers of image download
                AdvertImageDownloaded(advert.slotId);
            }

            imageDownload.Dispose();
            imageDownload = null;
            GC.Collect();

            yield break;
        }

        private IEnumerator DownloadXmlFromServer()
        {
            if (m_IsDownloadingNewXML)
            {
                yield break;
            }

            m_IsDownloadingNewXML = true;

            // Wait for the game loading to settle down first
            yield return new WaitForSeconds(2.0f);
    		
            // Download the xml
            yield return StartCoroutine(DownloadXML(HouseAdUrls.GetUrlForCurrentPlatform() + "/true"));

            // Verify xml
            if (m_XmlSrc != string.Empty)
            {
                ParseXmlFile();
            }
            else
            {
                Debug.LogError("Advert XML source was empty! From link: " + HouseAdUrls.GetUrlForCurrentPlatform() + "/true");
            } 
            if (m_AdSlots != null)
            {
                m_AdSlots.Clear();
            }
            else
            {
                m_AdSlots = new List<HouseAdSlot>();
            }
    		
            m_IsDownloadingNewXML = false;
            yield break;
        }

        private void ParseXmlFile()
        {
            // Parse xml
            StringReader textString = new StringReader(m_XmlSrc);
            XmlReader xmlReader = XmlReader.Create(textString);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name == "container")
                    {
                        string slotID = xmlReader.GetAttribute("slotid");
                        if (slotID == "showingHouseAds")
                        {
                        }
                    }
                    if (xmlReader.Name == "ad")
                    {
                        HouseAd newAd = GetAdFromCurrentNode(xmlReader);
                        bool hasReplaced = false;
                        if (m_AdvertList == null)
                        {
                            m_AdvertList = new List<HouseAd>();
                        }
                        for (int i = 0; i < m_AdvertList.Count; i++)
                        {
                            if (m_AdvertList[i].slotId == newAd.slotId)
                            {
                                hasReplaced = true;
                                if (m_AdvertList[i].updateTime != newAd.updateTime)
                                {
                                    m_AdvertList[i] = newAd;
                                    i = m_AdvertList.Count;
                                }
                            }
                        }
                        if (!hasReplaced)
                        {
                            m_AdvertList.Add(newAd);
                        }
                    }
                }
            }
        }

        private HouseAd GetAdFromCurrentNode(XmlReader reader)
        {

            HouseAd newAd = new HouseAd();

            string attribute = reader.GetAttribute("updatetime");
            if (attribute != string.Empty)
                newAd.updateTime = Int32.Parse(attribute);
            newAd.slotId = reader.GetAttribute("slotid");
            newAd.adURL = reader.GetAttribute("adurl");
            newAd.imageURL = reader.GetAttribute("imgurl");
            attribute = reader.GetAttribute("hd");
            if (attribute == "true")
                newAd.HD = true;
            attribute = reader.GetAttribute("x");
            if (attribute != string.Empty)
                newAd.x = float.Parse(attribute);
            attribute = reader.GetAttribute("y");
            if (attribute != string.Empty)
                newAd.y = float.Parse(attribute);
            attribute = reader.GetAttribute("active");
            if (attribute == "1")
                newAd.active = true;

            return newAd;
        }

        private IEnumerator DownloadXML(string xmlURL)
        {
            if (xmlURL != string.Empty && xmlURL != "/true")
            {
                WWW xmlDownload = new WWW(xmlURL);
                yield return xmlDownload;

                if (xmlDownload.error != null)
                {
                    Debug.LogError("Xml Download Error(" + xmlURL + "): " + xmlDownload.error);
                }
                else
                {
                    // Directly encode as UTF8 (cheaper than xmlDownload.text)
                    m_XmlSrc = System.Text.Encoding.UTF8.GetString(xmlDownload.bytes, 0, xmlDownload.bytes.Length);

    #if UNITY_WEBPLAYER || UNITY_METRO || UNITY_WP8
                	// Unity webplayer / WinStore can't access local stuff so we can't write our ads
                    Debug.Log("XML File downloaded.");
    #else
                    // Save XML locally
                    PlayerPrefs.SetString("TimeOfLastAdvertUpdate", DateTime.Now.ToString());
                    PlayerPrefs.Save();

                    // Store if downloaded
                    Directory.CreateDirectory(cachePath);
                    System.IO.File.WriteAllText(xmlCacheFilePath, m_XmlSrc);
                    Debug.Log("XML File saved locally to " + xmlCacheFilePath);

    #endif
                    // Notify observers
                    AdvertDataUpdated();
                }
            }
            else
            {
                Debug.LogError("AVHouseAdManager: Xml download string was empty");
            }
            yield break;
        }
    }
}
