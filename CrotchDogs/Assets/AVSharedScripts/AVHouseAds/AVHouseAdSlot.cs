using System;
using System.Collections.Generic;
using UnityEngine;

public class AVHouseAdSlot
{
	public string Name;
	public AVHouseAd DefaultAdvert = null;
	private int m_CurrentIndex = 0;
	private List<AVHouseAd> m_AdList = new List<AVHouseAd>();
	public void AddAdvert(AVHouseAd ad){
		m_AdList.Add(ad);
	}
	
	public AVHouseAd GetNextAdvert()
	{
		if(m_AdList.Count == 0){
			Debug.LogError("AVHouseAdSlot: Advert list was empty");
			return null;
		}
		
		if(m_AdList.Count <= m_CurrentIndex){
			m_CurrentIndex = 0;
		}
		
		return m_AdList[m_CurrentIndex++];
	}
	
	public void Reverse()
	{
		m_AdList.Reverse();
	}
}

