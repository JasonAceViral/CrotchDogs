using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace AceViral {
    public class Prefs {

    	public const string ENCRYPTION_KEY = "aceviral";
    	private static int m_KeysSaved = 0;
    	private static StringBuilder sb = new StringBuilder();

    	public static bool HasKey(string key){
    		return PlayerPrefs.HasKey (key);
    	}

    	public static void DeleteAll(){
    		PlayerPrefs.DeleteAll ();
    	}

    	public static void DeleteKey(string key){
    		PlayerPrefs.DeleteKey (key);
    	}

    	// Bool Stuff
    	public static bool GetBool(string key, bool defaultValue){
    		return GetBoolSecure (key, defaultValue);
    	}

    	public static void SetBool(string key, bool val){
    		m_KeysSaved++;
    		#if UNITY_EDITOR
    		sb.Append("BOOL: "); sb.Append(key); sb.Append(" / "); sb.Append(val); sb.AppendLine();
    		#endif
    		SetBoolSecure (key, val);
    	}

    	// Int Stuff
    	public static void SetInt(string key, int value){
    		m_KeysSaved++;
    		#if UNITY_EDITOR
    		sb.Append("INT: "); sb.Append(key); sb.Append(" / "); sb.Append(value); sb.AppendLine();
    		#endif
    		SetIntSecure (key, value);
    	}

    	public static int GetInt(string key, int defaultValue){
    		return GetIntSecure (key, defaultValue);
    	}

    	public static int GetInt(string key){
    		return GetInt (key, 0);
    	}

    	// Float Stuff
    	public static void SetFloat(string key, float value){
    		m_KeysSaved++;
    		#if UNITY_EDITOR
    		sb.Append("FLOAT: ");
    		sb.Append(key);
    		sb.Append(" / ");
    		sb.Append(value);
    		sb.AppendLine();
    		#endif
    		PlayerPrefs.SetFloat (key, value);
    	}

    	public static float GetFloat(string key, float defaultValue){
    		return PlayerPrefs.GetFloat (key, defaultValue);
    	}

    	// String Stuff
    	public static void SetString(string key, string value){
    		m_KeysSaved++;
    		#if UNITY_EDITOR
    		sb.Append("STRING: ");
    		sb.Append(key);
    		sb.Append(" / ");
    		sb.Append(value);
    		sb.AppendLine();
    		#endif
    		SetStringSecure (key, value);
    		//	PlayerPrefs.SetString (key, value);
    	}

    	public static string GetString(string key, string defaultValue){
    		return GetStringSecure (key, defaultValue);
    		//return PlayerPrefs.GetString (key, defaultValue);
    	}

    	public static string GetString(string key){
    		return GetString (key, string.Empty);
    	}

    	// =-=-=-=-=
    	// Dates
    	// =-=-=-=-=
    	public static DateTime GetDateTimeSecure(string key){
    		return GetDateTimeSecure (key, new DateTime());
    	}

    	public static DateTime GetDateTimeSecure(string key, DateTime defaultValue){
    		string load = GetStringSecure (key, string.Empty);
    		if (!string.IsNullOrEmpty (load)) {
    			DateTime ret;
    			if (DateTime.TryParse (load, out ret)) {
    				return ret;
    			}
    		}
    		return defaultValue;
    	}

    	public static void SetDateTimeSecure(string key, DateTime value){
    		SetStringSecure (key, value.ToString ());
    	}


    	// Int Array Stuff
    	public static int[] LoadIntArray(string key){
    		string strArray = PlayerPrefs.GetString (key, string.Empty);
    		List<int> arr = new List<int> ();
    		string[] splitVals = strArray.Split (',');
    		for (int i = 0; i < splitVals.Length; i++) {
    			splitVals[i].Replace(",","");
    			if (splitVals [i].CompareTo(string.Empty) != 0) {
    				arr.Add (int.Parse(splitVals [i]));
    			}
    		}
    		return arr.ToArray ();
    	}

    	public static void SaveIntArray(string key, int[] array){
    		if (array == null) {
    			return;
    		}
    		string strArray = string.Empty;
    		for (int i = 0; i < array.Length; i++) {
    			strArray += array [i].ToString () + ",";
    		}
    		PlayerPrefs.SetString (key, strArray);
    	}

    	public static void Save(){
    		#if UNITY_EDITOR
    		//Debug.Log (" ******************* Saving to prefs(" + m_KeysSaved + ") *******************" + System.Environment.NewLine + sb.ToString());
    		#endif
    		sb.Length = 0;
    		m_KeysSaved = 0;
    		PlayerPrefs.Save ();
    	}


    	// ****************
    	// SECURE STUFF
    	// ****************

    	public static bool GetBoolSecure (string val)
    	{
    		return (PlayerPrefs.GetInt (Utility.ScrambleStringFlip (val)) == 1);	
    	}

    	public static bool GetBoolSecure (string val, bool defaultVal)
    	{
    		return (PlayerPrefs.GetInt (Utility.ScrambleStringFlip (val), (defaultVal) ? 1 : 0) == 1);	
    	}

    	public static void SetBoolSecure (string val, bool flag)
    	{
    		PlayerPrefs.SetInt (Utility.ScrambleStringFlip (val), (flag) ? 1 : 0);
    	}

    	public static int GetIntSecure (string val, int defaultValue)
    	{
    		int pickedval = PlayerPrefs.GetInt (Utility.ScrambleStringFlip (val));
    		if (Utility.ScrambleStringFlip (pickedval.ToString ()) == PlayerPrefs.GetString (val)) {
    			return pickedval;	
    		} else {
    			return defaultValue;	
    		}
    	}

    	public static void SetIntSecure (string val, int num)
    	{
    		PlayerPrefs.SetInt (Utility.ScrambleStringFlip (val), num);
    		PlayerPrefs.SetString (val, Utility.ScrambleStringFlip (num.ToString ()));
    	}

    	public static void SetStringSecure(string val, string data){
    		//PlayerPrefs.SetString(AVUtility.ScrambleStringFlip (val), StringEncrypt.Encrypt (data, ENCRYPTION_KEY));
    		PlayerPrefs.SetString (Utility.ScrambleStringFlip (val), data);
    		PlayerPrefs.SetString (val, Utility.ScrambleStringFlip (data));
    	}

    	public static string GetStringSecure (string val, string defaultValue)
    	{
    //		return StringEncrypt.Decrypt(PlayerPrefs.GetString(AVUtility.ScrambleStringFlip (val)), ENCRYPTION_KEY);
    		string pickedval = PlayerPrefs.GetString (Utility.ScrambleStringFlip (val));
    		if (Utility.ScrambleStringFlip (pickedval.ToString ()) == PlayerPrefs.GetString (val)) {
    			return pickedval;	
    		} else {
    			return defaultValue;	
    		}
    	}
    }
}