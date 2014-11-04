using UnityEngine;
using System.Collections;

public class MovingText : MonoBehaviour {

		public const float MAX_SHOW_TIME = 1.0f,BEFORE_HIDE_TIME=1.0f;

	public enum InfoType
	{
				BITE,MAUL,MISS,DOMESTICATED,BAD_DOG,DANGEROUS_BREED,WILD_ANIMAL,MAN_EATER,CROTCH_DOG,READY
	}

	public Vector3 showLocation,hideLocation;
	public float offsetLocation,showTime=0.0f,timeBeforeHide=0.0f;
	public bool showSprite = false,isShowing= false,hideSprite = true;
	
	// Use this for initialization
	void Start () 
	{
		showLocation = gameObject.transform.position;
		hideLocation = showLocation;
		hideLocation.x += offsetLocation;
			
		gameObject.transform.position = hideLocation;
	
	}
	
	// Update is called once per frame
	void Update () 
	{



				if (showSprite) // show the sprite on screen
				{
						if (showTime < MAX_SHOW_TIME) 
						{
								showTime += Time.deltaTime;

								float newX = Mathf.Lerp(  gameObject.transform.position.x,showLocation.x,showTime / MAX_SHOW_TIME) ;
								float newY = Mathf.Lerp(  gameObject.transform.position.y,showLocation.y,showTime / MAX_SHOW_TIME) ;

				
								gameObject.transform.position = new Vector3 (newX, newY, gameObject.transform.position.z);
						}
						else 
						{

								if (!isShowing) 
								{
										isShowing = true;
										gameObject.transform.position = showLocation;
								}
								else 
								{
										timeBeforeHide += Time.deltaTime;

										if (timeBeforeHide >= BEFORE_HIDE_TIME) 
										{
												showSprite = false;

												showTime = 0.0f;
										}
								}
						}
				}
				else // hide the sprite from screen
				{
						if (showTime < MAX_SHOW_TIME) {
								showTime += Time.deltaTime;

								float newX = Mathf.Lerp (hideLocation.x, gameObject.transform.position.x, showTime / MAX_SHOW_TIME);
								float newY = Mathf.Lerp (hideLocation.y, gameObject.transform.position.y, showTime / MAX_SHOW_TIME);


								gameObject.transform.position = new Vector3 (newX, newY, gameObject.transform.position.z);
						} 
						else 
						{
								isShowing = false;
						}
				}
	}

	public void showFlavourText (InfoType showType)
	{

				//if (!isShowing) 
				{
						switch (showType) {
						case InfoType.BITE:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("bitewordBITE");
								break;
						case InfoType.MAUL:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("bitewordMAUL");
								break;
						case InfoType.MISS:
								gameObject.GetComponent<tk2dSprite> ().SetSprite ("bitewordMISS");
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
