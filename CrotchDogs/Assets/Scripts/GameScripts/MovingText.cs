using UnityEngine;
using System.Collections;

public class MovingText : MonoBehaviour {

	public const float MAX_SHOW_TIME = 0.5f,BEFORE_HIDE_TIME=0.5f;

	public enum InfoType
	{
		BITE,MAUL,MISS,PLAYFUL,NAUGHTY,AGGRESSIVE,MONSTROUS,CROTCH_DOG,READY
	}

	public Vector3 showLocation,hideLocation;
	public float offsetLocation,showTime=0.0f,timeBeforeHide=0.0f;
	public bool showSprite = false,isShowing= false,hideSprite = true;
	public GameObject startLocation;
	
	// Use this for initialization
	void Awake () 
	{
		Debug.Log ("Awake");
		showLocation = gameObject.transform.position;
		hideLocation = showLocation;
		hideLocation.x += offsetLocation;
			
		gameObject.transform.position = startLocation.transform.position;
	
	}
	public void setToStartPosition()
	{
		gameObject.transform.position = startLocation.transform.position;
		showSprite = false;
		isShowing = false;
		showTime = 0.0f;
		timeBeforeHide = 0.0f;
	}
	// Update is called once per frame
	void Update () 
	{

				if (GameController.Instance.getState () != GameController.GameState.PAUSED) {
						if (showSprite) { // show the sprite on screen
								if (showTime < MAX_SHOW_TIME) {

										showTime += Time.deltaTime;

										float newX = Mathf.Lerp (gameObject.transform.position.x, showLocation.x, showTime / MAX_SHOW_TIME);
										float newY = Mathf.Lerp (gameObject.transform.position.y, showLocation.y, showTime / MAX_SHOW_TIME);
				
										gameObject.transform.position = new Vector3 (newX, newY, gameObject.transform.position.z);
								} else {
										if (!isShowing) {
												isShowing = true;
												gameObject.transform.position = showLocation;
										} else {
												timeBeforeHide += Time.deltaTime;

												if (timeBeforeHide >= BEFORE_HIDE_TIME) {
														showSprite = false;

														showTime = 0.0f;
												}
										}
								}
						} else if (isShowing && hideSprite) { // hide the sprite from screen
								if (showTime < MAX_SHOW_TIME) {
										showTime += Time.deltaTime;

										float newX = Mathf.Lerp (gameObject.transform.position.x, hideLocation.x, showTime / MAX_SHOW_TIME);
										float newY = Mathf.Lerp (gameObject.transform.position.y, hideLocation.y, showTime / MAX_SHOW_TIME);

										gameObject.transform.position = new Vector3 (newX, newY, gameObject.transform.position.z);
								} else {
										gameObject.transform.position = new Vector3 (startLocation.transform.position.x, startLocation.transform.position.y, gameObject.transform.position.z);
										isShowing = false;
										showTime = 0.0f;
								}
						}
				}
	}
		public void hide()
		{

				if (showSprite) 
				{
						showTime = 0.0f;
						showSprite = false;
						isShowing = true;
					
				}
			
		}

		public bool onScreen()
		{
				return isShowing || showSprite;
		}

	public void showFlavourText (InfoType showType)
	{

				//if (!isShowing) 
				{
						switch (showType) {
						case InfoType.BITE:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordbite");
								break;
						case InfoType.MAUL:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordmaul");
								break;
						case InfoType.MISS:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordmiss");
								break;
						case InfoType.PLAYFUL:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordplayful");
								break;
						case InfoType.NAUGHTY:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordnaughty");
								break;
						case InfoType.AGGRESSIVE:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordaggressive");
								break;
						case InfoType.MONSTROUS:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordmonstrous");
								break;
						case InfoType.CROTCH_DOG:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordcrotchdog");
								break;
						case InfoType.READY:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("wordready");
								break;
						}

						showSprite = true;
						isShowing = false;
						showTime = 0.0f;
						timeBeforeHide = 0.0f;
				}
	}


}
