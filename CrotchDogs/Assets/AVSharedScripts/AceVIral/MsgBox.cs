using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AceViral {
    public delegate void MsgBoxHandler (MsgBoxResponse response);
    public delegate void Interaction ();

    public enum MsgBoxType
    {
    	YES_NO,
    	OK
    }

    public enum MsgBoxResponse
    {
    	YES,
    	NO,
    	OK
    }

    public class MsgBox : MonoBehaviour
    {
    	public class MsgBoxMessage
    	{
    		public string Title, Message;
    		public MsgBoxType MsgType;
    		public MsgBoxHandler CallBack;
    	}
    	// Static instance
    	private static MsgBox _Instance;
    	// Internal message queue
    	private Queue<MsgBoxMessage> m_MessageBoxQueue = new Queue<MsgBoxMessage> ();
    	private MsgBoxMessage m_CurrentMessage = null;
    	// NGUI Widgets
    	public GameObject PanelParent;
    	public GameObject ScaleParent;
    	public tk2dTextMesh TitleLabel, MainTextLabel;
    	public tk2dUIItem ButtonYes, ButtonNo, ButtonOk;
    	// Unity Init/Update
    	private void Awake ()
    	{
    		_Instance = this;
    	}

    	private void OnDestroy ()
    	{
    		_Instance = null;	
    	}

    	public static event Interaction show;
    	public static event Interaction hide;
    	private void Show() {
    		if (show != null) {
    			show ();
    		}
    	}
    	private void Hide() {
    		if (hide != null) {
    			hide ();
    		}
    	}

    	private void Start ()
    	{
    		if (CanMsgBoxShow ()) {
    			ButtonYes.OnClick += ButtonYes_OnClick;
    			ButtonNo.OnClick += ButtonNo_OnClick;
    			ButtonOk.OnClick += ButtonOk_OnClick;	
    		}		
    	}

    	private void Update ()
    	{
    		if (m_CurrentMessage == null) {
    			if (m_MessageBoxQueue.Count > 0) {
    				ShowMsgDialog (m_MessageBoxQueue.Dequeue ());
    			} else {
    				// Hiding only when unneeded fixes any quick visual artifacts (turned off/turned on)
    				// in-between creating a new msgbox directly after hiding another
    				if (PanelParent != null) {
    					if (PanelParent.activeSelf) {
    						//TODO NGUITools.SetActive (PanelParent, false);
    					}
    				}
    			}
    		} else {
    			// If there's a msgbox showing - handle back/escape button appropriately
    			if (Input.GetKeyDown (KeyCode.Escape)) {
    				switch (m_CurrentMessage.MsgType) {
    				case MsgBoxType.OK:
    					ButtonOk_OnClick (null, null);
    					break;
    				case MsgBoxType.YES_NO:
    					ButtonNo_OnClick (null, null);
    					break;
    				default:
    					ButtonOk_OnClick (null, null);
    					break;
    				}
    			}
    		}
    	}
    	// Message Box Construction
    	private void ShowMsgDialog (MsgBoxMessage message)
    	{
    		if (CanMsgBoxShow ()) {
    			m_CurrentMessage = message;
    			//TODO NGUITools.SetActive (PanelParent, true);
    		
    			TitleLabel.text = message.Title;
    			MainTextLabel.text = message.Message;
    		
    			switch (message.MsgType) {
    			case MsgBoxType.OK:
    				SetMsgBoxAsTypeOK ();
    				break;
    			case MsgBoxType.YES_NO:
    				SetMsgBoxAsTypeYesNo ();
    				break;
    			}
    		
    			Show ();

    			StartCoroutine (AnimateMsgBox ());
    		}
    	}

    	private void SetMsgBoxAsTypeOK ()
    	{
    		//TODO NGUITools.SetActive (ButtonYes.gameObject, false);
    		//TODO NGUITools.SetActive (ButtonNo.gameObject, false);
    		//TODO NGUITools.SetActive (ButtonOk.gameObject, true);
    	}

    	private void SetMsgBoxAsTypeYesNo ()
    	{
    		//TODO NGUITools.SetActive (ButtonOk.gameObject, false);
    		//TODO NGUITools.SetActive (ButtonYes.gameObject, true);
    		//TODO NGUITools.SetActive (ButtonNo.gameObject, true);
    	}

    	private IEnumerator AnimateMsgBox ()
    	{
    		ScaleParent.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);

    		//TODO TweenScale scaleTween = TweenScale.Begin (ScaleParent, 0.3f, Vector3.one);
    		//TODO scaleTween.method = UITweener.Method.EaseInOut;
    		yield break;
    	}
    	// Message Box Finishing/Hiding
    	private void FinishMsgDialog (MsgBoxResponse response)
    	{
    		//AudioManager.PlayMenuForward ();
    		if (m_CurrentMessage.CallBack != null) {
    			m_CurrentMessage.CallBack (response);
    		}
    		
    		
    		if (m_MessageBoxQueue.Count > 0) {
    			ShowMsgDialog (m_MessageBoxQueue.Dequeue ());
    		} else {
    			// Hiding only when unneeded fixes any quick visual artifacts (turned off/turned on)
    			// in-between creating a new msgbox directly after hiding another
    			m_CurrentMessage = null;
    			if (PanelParent.activeSelf) {
    				//TODO NGUITools.SetActive (PanelParent, false);
    			}

    			Hide ();
    		}		
    	}

    	// Button Wrapper Events
    	private void ButtonYes_OnClick ()
    	{
    		ButtonYes_OnClick (this, null);
    	}

    	private void ButtonNo_OnClick ()
    	{
    		ButtonNo_OnClick (this, null);
    	}

    	private void ButtonOk_OnClick ()
    	{
    		ButtonOk_OnClick (this, null);
    	}

    	// Button Events
    	private void ButtonYes_OnClick (object sender, System.EventArgs e)
    	{
    		FinishMsgDialog (MsgBoxResponse.YES);
    	}

    	private void ButtonNo_OnClick (object sender, System.EventArgs e)
    	{
    		FinishMsgDialog (MsgBoxResponse.NO);
    	}

    	private void ButtonOk_OnClick (object sender, System.EventArgs e)
    	{
    		FinishMsgDialog (MsgBoxResponse.OK);
    	}
    	// External
    	public void ShowOkDialogFromExternalRequest (string message)
    	{
    		string[] msgSplit = message.Split ('#');
    		if (msgSplit.Length > 1) {
    			Show (msgSplit [0], msgSplit [1]);
    		}
    	}
    	// Static instantiators
    	public static void Show (string title, string message)
    	{
    		Show (title, message, MsgBoxType.OK);
    	}

    	public static void Show (string title, string message, MsgBoxType type)
    	{
    		Show (title, message, type, null);
    	}

    	public static void Show (string title, string message, MsgBoxType type, MsgBoxHandler callbackHandle)
    	{
    		if (_Instance != null) {
    			if (_Instance.CanMsgBoxShow ()) {
    				_Instance.m_MessageBoxQueue.Enqueue (new MsgBox.MsgBoxMessage () {
    					Title = title.ToUpper (),
    					Message = message,
    					MsgType = type,
    					CallBack = callbackHandle
    				});
    			} else {
    				if (callbackHandle != null) {
    					Debug.LogWarning ("MsgBox: Show() - Could not show message box so invoking function");
    					if (type == MsgBoxType.OK) {
    						callbackHandle (MsgBoxResponse.OK);
    					} else {
    						callbackHandle (MsgBoxResponse.YES);
    					}					
    				}
    			}
    		} else {
    			Debug.LogWarning ("MsgBox: Show() - _Instance is null!!!");	
    		}
    	}

    	public static bool IsShowingDialog ()
    	{
    		if (_Instance != null) {
    			return _Instance.m_CurrentMessage != null;	
    		}
    		return false;
    	}
    	// Menu Verification (bit overkill - but mainly for setting up new projects)
    	private bool CanMsgBoxShow ()
    	{
    		if (PanelParent == null) {
    			Debug.LogWarning ("MsgBox: PanelParent is null");
    			return false;	
    		}
    		if (ScaleParent == null) {
    			Debug.LogWarning ("MsgBox: ScaleParent is null");
    			return false;	
    		}
    		if (TitleLabel == null) {
    			Debug.LogWarning ("MsgBox: TitleLabel is null");
    			return false;	
    		}
    		if (MainTextLabel == null) {
    			Debug.LogWarning ("MsgBox: MainTextLabel is null");
    			return false;
    		}
    		if (ButtonYes == null) {
    			Debug.LogWarning ("MsgBox: ButtonYes is null");
    			return false;	
    		}
    		if (ButtonNo == null) {
    			Debug.LogWarning ("MsgBox: ButtonNo is null");
    			return false;	
    		}
    		if (ButtonOk == null) {
    			Debug.LogWarning ("MsgBox: ButtonOk is null");
    			return false;	
    		}
    		return true;
    	}
    }
}
