using UnityEngine;
using System.Collections;

namespace AceViral {
    public delegate void InAppBoughtFunction();
    public class InAppID {
    	
    	
    	public string Name;
    	public string iOSID;
    	public string MacID;
    	public string AndroidID;
    	public string AmazonID;
        public string WindowsPhoneID;
    	public bool Managed;
    	public InAppBoughtFunction OnBought;
    	public string Price = string.Empty;
    	public string Display;
        public bool isConsumable = false;
    }
}
