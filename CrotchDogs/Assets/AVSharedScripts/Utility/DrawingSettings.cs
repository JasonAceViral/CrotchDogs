using UnityEngine;
using System.Collections;

namespace AceViral {
    public class DrawingSettings : MonoBehaviour {
    	
    	public static ScaleType ScaleType = ScaleType.Height;
    	
    	private const float TargetWidth = 640.0f;
    	private const float TargetHeight = 960.0f;
    	
    	private static float _ScaleMultiplier = 1.0f;
    	public static float ScaleMultiplier {
    //#if UNITY_ANDROID || UNITY_IPHONE
    		//get {return _WidthMultiplier;}
    //#else
    		get {return _ScaleMultiplier;}	
    //#endif
    	}
    	
    	private static float _WidthMultiplier = 1.0f;
    	public static float WidthMultiplier {
    		get {return _WidthMultiplier;}	
    	} 
    	
    	private static float _HeightMultiplier = 1.0f;
    	public static float HeightMultiplier {
    		get {return _HeightMultiplier;}	
    	}
    	
    	public static void DetectDimensions()
    	{
    		_WidthMultiplier = Screen.width / TargetWidth;
    		_HeightMultiplier = Screen.height / TargetHeight;
    		
    		_ScaleMultiplier = (_WidthMultiplier < _HeightMultiplier) ? _WidthMultiplier : _HeightMultiplier;
    	}
    	
    	public static float _FadeTextureAlpha = 0.65f;
    	private static Texture2D _FadeTexture = null;
    	public static Texture2D FadeTexture{
    		get {CreateFadeTexture(); return _FadeTexture;}
    	}
    	private static void CreateFadeTexture()
    	{
    		if(_FadeTexture == null){
    		_FadeTexture = new Texture2D(1,1);
    		_FadeTexture.SetPixel(0,0, new Color(0,0,0,_FadeTextureAlpha));
    		_FadeTexture.Apply();
    		}
    	}
    	
    	public static bool IsSD()
    	{
    		//if(Application.platform == RuntimePlatform.IPhonePlayer){
    			//if(Screen.width < 640){
    				//return true;	
    			//}
    		//}
    		return false;
    	}
    }
}