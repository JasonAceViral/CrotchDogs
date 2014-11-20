using System;
using System.Collections.Generic;
using UnityEngine;

namespace AceViral {

    public class HouseAdSlot
    {
    	public string Name;
    	public HouseAd DefaultAdvert = null;
    	private int m_CurrentIndex = 0;
    	private List<HouseAd> m_AdList = new List<HouseAd>();
    	public void AddAdvert(HouseAd ad){
    		m_AdList.Add(ad);
    	}
    	
    	public HouseAd GetNextAdvert()
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
}
