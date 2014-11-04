using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public delegate void PurchaseFailedHandler (AVInAppID id);
public abstract class AVInAppUnity : MonoBehaviour
{

	private static AVInAppUnity m_Instance;

	public static AVInAppUnity Instance {
		get {
			if (m_Instance == null) {
				#if UNITY_EDITOR
				m_Instance = new GameObject ().AddComponent<AVInAppUnityEditor> ();
#elif UNITY_ANDROID
					m_Instance = new GameObject().AddComponent<AVInAppUnityAndoid>();	
#elif UNITY_IPHONE
				m_Instance = new GameObject().AddComponent<AVInAppUnityIOS>();		
#elif UNITY_STANDALONE_OSX
				m_Instance = new GameObject().AddComponent<AVInAppUnityMac>();		
#elif UNITY_WEBPLAYER
				m_Instance = new GameObject().AddComponent<AVInAppUnityWeb>();
#elif UNITY_METRO
				m_Instance = new GameObject().AddComponent<AVInAppUnityWin>();
#elif UNITY_WP8
				m_Instance = new GameObject().AddComponent<AVInAppUnityWindowsPhone>();
#endif
                m_Instance.Initialize();
			}
			return m_Instance;
		}
	}

	public static List<AVInAppID> m_PurchaseList = new List<AVInAppID>();

	//public static ReadOnlyCollection<AVInAppID> PurchaseList
	//{
	//    get{ return m_PurchaseList.AsReadOnly();}
	//}

    private void Initialize()
    {
        // Add purchase information
        for (int i = 0; i < AVAppConstants.InAppPurchases.Length; i++)
        {
            AddPurchaseID(AVAppConstants.InAppPurchases[i]);
        }

        OnStart();
    }

    protected virtual void OnStart() { }

    protected virtual void AddPurchaseID(AVInAppID id)
    {
        for (int i = 0; i < m_PurchaseList.Count; i++)
        {
            if (((AVInAppID)m_PurchaseList[i]).Name == id.Name)
            {
                Debug.Log("AVInAppUnity: Purchase List already contains purchase information on an id with name '" + id.Name + "'");
                return;
            }
        }
        m_PurchaseList.Add(id);
        PostProcessPurchaseInformation(id);
    }

    public virtual void PostProcessPurchaseInformation(AVInAppID id) { }

    public AVInAppID GetInAppWithReferenceName(string refName)
    {
        for (int i = 0; i < m_PurchaseList.Count; i++)
        {
            if (((AVInAppID)m_PurchaseList[i]).Name == refName)
            {
                return m_PurchaseList[i];
            }
        }
        return null;
    }

	public abstract void RequestPurchase (AVInAppID id);

	public abstract void RestorePurchases ();
	//public abstract void CheckForCompletedPurchases();	
	public abstract void PurchaseFailed (string message);

	public abstract void OnPurchaseDataReceived (string data);

	public abstract void RequestPurchasePrices ();

	public event PurchaseFailedHandler PurchaseFailedHandle;

	public virtual void OnPurchaseFailed (AVInAppID id)
	{
        AVMsgBox.Show("In-App Purchases", (id == null ? "Purchase failed" : "Purchase failed for: " + id.Name));

        HandleFailedPurchase(id);
	}

    public virtual void HandleFailedPurchase (AVInAppID id)
    {
        if (PurchaseFailedHandle != null) {
            PurchaseFailedHandle (id);
        }
    }
}