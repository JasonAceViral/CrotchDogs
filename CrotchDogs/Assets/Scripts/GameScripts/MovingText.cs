using UnityEngine;
using System.Collections;

public class MovingText : MonoBehaviour {

	public const float MAX_SHOW_TIME = 0.5f,BEFORE_HIDE_TIME=0.5f;

	public enum InfoType
	{
		BITE,MAUL,MISS,DOMESTICATED,BAD_DOG,DANGEROUS_BREED,WILD_ANIMAL,MAN_EATER,CROTCH_DOG,READY
	}

	public Vector3 showLocation,hideLocation;
	public float offsetLocation,showTime=0.0f,timeBeforeHide=0.0f;
	public bool showSprite = false,isShowing= false,hideSprite = true;
	public GameObject startLocation;
	// Use this for initialization
	void Start () 
	{

		showLocation = gameObject.transform.position;
		hideLocation = showLocation;
		hideLocation.x += offsetLocation;
			
		gameObject.transform.position = startLocation.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () 
	{

				if (GameController.Instance.getState () == GameController.GameState.PLAYING_GAME) {
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
						} else if (isShowing) { // hide the sprite from screen
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
						case InfoType.DOMESTICATED:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank1");
								break;
						case InfoType.BAD_DOG:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank2");
								break;
						case InfoType.DANGEROUS_BREED:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank3");
								break;
						case InfoType.WILD_ANIMAL:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank4");
								break;
						case InfoType.MAN_EATER:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank5");
								break;
						case InfoType.CROTCH_DOG:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("rank6");
								break;
						case InfoType.READY:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("READY");
								break;
						}

						showSprite = true;
						isShowing = false;
						showTime = 0.0f;
						timeBeforeHide = 0.0f;
				}
	}
}
