using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace AceViral {
    public delegate void PurchaseFailedHandler (InAppID id);
    public abstract class InAppUnity : MonoBehaviour
    {

    	private static InAppUnity m_Instance;

    	public static InAppUnity Instance {
    		get {
    			if (m_Instance == null) {
    				#if UNITY_EDITOR
                    m_Instance = new GameObject ().AddComponent<AVHiddenInterface.InAppUnityEditor> ();
    #elif UNITY_ANDROID
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityAndoid>();	
    #elif UNITY_IPHONE
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityIOS>();		
    #elif UNITY_STANDALONE_OSX
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityMac>();		
    #elif UNITY_WEBPLAYER
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityWeb>();
    #elif UNITY_METRO
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityWin>();
    #elif UNITY_WP8
                    m_Instance = new GameObject().AddComponent<AVHiddenInterface.InAppUnityWindowsPhone>();
    #endif
                    m_Instance.Initialize();
                    m_Instance.gameObject.name = "AVInAppUnity";
    			}
    			return m_Instance;
    		}
    	}

    	public static List<InAppID> m_PurchaseList = new List<InAppID>();

        private void Initialize()
        {
            // Add purchase information
            for (int i = 0; i < AppConstants.IAP.Purchases.Length; i++)
            {
                AddPurchaseID(AppConstants.IAP.Purchases[i]);
            }

            OnStart();
            RequestPurchasePrices();
        }

        protected virtual void OnStart() { }

        protected virtual void AddPurchaseID(InAppID id)
        {
            for (int i = 0; i < m_PurchaseList.Count; i++)
            {
                if (((InAppID)m_PurchaseList[i]).Name == id.Name)
                {
                    Debug.Log("AVInAppUnity: Purchase List already contains purchase information on an id with name '" + id.Name + "'");
                    return;
                }
            }
            m_PurchaseList.Add(id);
            PostProcessPurchaseInformation(id);
        }


        // =-=-=-=-=-=-=-=
        // Events
        // =-=-=-=-=-=-=-=

        public System.Action<InAppID> PurchaseFailedHandle;
        public System.Action<string> OnPurchaseSuccess;
        public System.Action OnPurchaseDataUpdated;


        // =-=-=-=-=-=-=-=
        // Interface
        // =-=-=-=-=-=-=-=

        public InAppID GetInAppWithReferenceName(string refName)
        {
            for (int i = 0; i < m_PurchaseList.Count; i++)
            {
                if (((InAppID)m_PurchaseList[i]).Name == refName)
                {
                    return m_PurchaseList[i];
                }
            }
            return null;
        }

    	public abstract void RequestPurchase (InAppID id);

    	public abstract void RestorePurchases ();

    	public abstract void PurchaseFailed (string message);

    	public abstract void OnPurchaseDataReceived (string data);

    	public abstract void RequestPurchasePrices ();

        public virtual void PostProcessPurchaseInformation(InAppID id) { }


        // =-=-=-=-=-=-=-=
        // Event Utility
        // =-=-=-=-=-=-=-=

        protected virtual void OnPurchaseFailed(InAppID id){
            MsgBox.Show(Localization.Instance.GetString(Localization.eLocalKeys.AV_In_App_Purchases),
                Localization.Instance.GetStringWithInput(Localization.eLocalKeys.AV_Purchase_Failed_For_x, id.Name));
            if(PurchaseFailedHandle != null){
                PurchaseFailedHandle(id);
            }
        }

        protected void SendOnPurchaseSuccessEvent(string iapID){
            if (OnPurchaseSuccess != null) {
                OnPurchaseSuccess (iapID);
            }
        }

        protected void SendOnPurchaseDataUpdatedEvent(){
            if (OnPurchaseDataUpdated != null) {
                OnPurchaseDataUpdated ();
            }
        }

        // =-=-=-=-=-=-=-=
        // Utility
        // =-=-=-=-=-=-=-=

        public static decimal GetDecimalPriceFromPriceString(string input) {
            if(string.IsNullOrEmpty(input)){
                return 0.0m;
            }
            try{
                return decimal.Parse(Regex.Match(input, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
            } catch (System.Exception e){
                AVDebug.DumpException (e);
            }
            return 0.0m;
        }
    }
}