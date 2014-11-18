using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AVDebug : MonoBehaviour {
	private const int MAX_LOGS = 50;
	private static AVDebug m_Instance;
	private static AVDebug Instance{
		get{
			if(m_Instance == null){
				m_Instance = new GameObject().AddComponent<AVDebug>();
				m_Instance.m_TextComponent = m_Instance.gameObject.AddComponent<GUIText>();
				m_Instance.m_TextComponent.transform.position = new Vector3(0,1,0);
				//m_Instance.m_TextComponent.font = AVFontManager.Manager.GetFont("", 20);
				//m_Instance.m_TextComponent.material = m_Instance.m_TextComponent.font.material;
				//DontDestroyOnLoad(m_Instance.gameObject);
			}
			return m_Instance;
		}	
	}
	
	public static void Log(string log)
	{
		//Instance.AddLog(log);
	}
	
	private List<string> m_LogList = new List<string>();
	private GUIText m_TextComponent = null;
	//private string logText = string.Empty;
	private void AddLog(string log)
	{
		m_LogList.Add(log);
		if(m_LogList.Count > MAX_LOGS){
			m_LogList.RemoveAt(0);
		}
		UpdateLogText();
	}
	
	private void UpdateLogText()
	{
		m_TextComponent.text = string.Empty;
		foreach(string log in m_LogList){
			m_TextComponent.text += log + "\n";	
		}
		//logText = m_TextComponent.text;
		m_TextComponent.text = string.Empty;
	}
	
	//void OnGUI()
	//{
		//GUI.Label(new Rect(0,0,Screen.width, Screen.height), logText);
	//}
}
