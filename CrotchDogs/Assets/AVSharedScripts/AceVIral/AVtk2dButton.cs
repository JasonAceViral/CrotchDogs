using UnityEngine;
using System.Collections;
using System;

namespace AceViral {
    [ExecuteInEditMode]
    [RequireComponent (typeof(tk2dSprite), typeof(tk2dUIItem), typeof(BoxCollider))]
    public class AVtk2dButton : tk2dUIBaseItemControl {
    	private tk2dSprite m_Sprite;
    	private tk2dUIItem m_UIItem;

    	public string NormalSprite;
    	public string PressedSprite;

    	public Action<int> OnEnumeratedClick;
    	public int EnumeratedValue;

    	public Action<object> OnClickWithObjectData;
    	public object ObjectData;

    	void Awake () {
    		m_Sprite = GetComponent<tk2dSprite> ();
    		m_UIItem = GetComponent<tk2dUIItem> ();
    		m_UIItem.OnDown += HandleOnDown;
    		m_UIItem.OnUp += HandleOnUp;
    		m_UIItem.OnClick += HandleOnClick;
    	}

    	void HandleOnUp ()
    	{
    		if (!string.IsNullOrEmpty (NormalSprite)) {
    			m_Sprite.SetSprite (NormalSprite);
    		}
    	}

    	void HandleOnDown ()
    	{
    		if (!string.IsNullOrEmpty (PressedSprite)) {
    			m_Sprite.SetSprite (PressedSprite);
    		}
    	}

    	private void HandleOnClick(){
    		DebugLog ();
    		if (OnEnumeratedClick != null) {
    			OnEnumeratedClick (EnumeratedValue);
    		}
    		if (OnClickWithObjectData != null) {
    			OnClickWithObjectData (ObjectData);
    		}
    	}

    	public tk2dSprite Sprite {
    		get {
    			if (m_Sprite == null) {
    				m_Sprite = GetComponent<tk2dSprite> ();	
    			}			
    			return m_Sprite;
    		}
    	}

    	public tk2dUIItem UIItem {
    		get {
    			if (m_UIItem == null) {
    				m_UIItem = GetComponent<tk2dUIItem> ();	
    			}			
    			return m_UIItem;
    		}
    	}

    	public void DebugLog(){
    		AVDebug.LogUIAction (gameObject.name + "_OnClick");
    	}

    	public void SetSprites(string sprName){
    		SetSprites (sprName,sprName);
    	}

    	public void SetSprites(string up, string down){
    		Sprite.SetSprite(up);
    		NormalSprite = up;
    		PressedSprite = down;
    	}
    }
}
