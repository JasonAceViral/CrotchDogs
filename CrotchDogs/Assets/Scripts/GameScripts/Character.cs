﻿using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float speed = -10.0f;
	public GameObject head,body,legs,ground,power1,power2;
	public bool crotchBitten = false,isShowing = false;
	public Collider crotchCollider;
	public Vector2[] legAnchors;
	public const int TOP_ORDER_IN_LAYER = 100;
	public bool setCharacterType = false,hasPowerUp=false;
	public int characterType = 0;
	public float opacity =0.0f;

	public const string BODY_SPRITE = "body",HEAD_SPRITE = "head",LEGS_SPRITE = "leg";
	public int headRandom,bodyRandom,legsRandom;
	
	

	// Use this for initialization
	void Awake () 
	{
		setCharacterlook ();
//		int headRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
//		int bodyRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
//		int legsRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
//
//		head.GetComponent<tk2dSprite> ().SetSprite (HEAD_SPRITE + headRandom);
//		body.GetComponent<tk2dSprite> ().SetSprite (BODY_SPRITE + bodyRandom);
//		legs.GetComponent<tk2dSprite> ().SetSprite (LEGS_SPRITE + legsRandom);
//
//		legs.transform.localPosition = legAnchors[legsRandom -1];
//
//		Debug.Log ("character awake " + legsRandom );
	}
	void Start()
	{
		setCharacterlook ();
	}

	public void setDrawOrder(int headOrder)
	{
		head.GetComponent<tk2dSprite> ().SortingOrder = headOrder;
		body.GetComponent<tk2dSprite> ().SortingOrder = headOrder-1;
		legs.GetComponent<tk2dSprite> ().SortingOrder = headOrder-2;
	}

	public void setCharacterlook()
	{
				opacity = 0.0f;
				if (!setCharacterType) 
				{
						//Debug.Log ("Character Start");
						headRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
						bodyRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
						legsRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;

						head.GetComponent<tk2dSprite> ().SetSprite (HEAD_SPRITE + headRandom);
						body.GetComponent<tk2dSprite> ().SetSprite (BODY_SPRITE + bodyRandom);
						legs.GetComponent<tk2dSprite> ().SetSprite (LEGS_SPRITE + legsRandom);

						legs.transform.localPosition = legAnchors [legsRandom - 1];
				}
				else
				{

						headRandom = characterType;
						bodyRandom = characterType;
						legsRandom = characterType;
						head.GetComponent<tk2dSprite> ().SetSprite (HEAD_SPRITE + characterType);
						body.GetComponent<tk2dSprite> ().SetSprite (BODY_SPRITE + characterType);
						legs.GetComponent<tk2dSprite> ().SetSprite (LEGS_SPRITE + characterType);

						legs.transform.localPosition = legAnchors [characterType - 1];
				}

				setOpacity (opacity);
			//Debug.Log ("character awake " +legsRandom );
	}

	public void setOpacity(float opacity)
	{
				Color newColor = new Color (1.0f, 1.0f, 1.0f, opacity);

				head.GetComponent<tk2dSprite> ().color = newColor;
				body.GetComponent<tk2dSprite> ().color = newColor;
				legs.GetComponent<tk2dSprite> ().color = newColor;

				if (hasPowerUp) 
				{
					power1.GetComponent<tk2dSprite> ().color = newColor;
					power2.GetComponent<tk2dSprite> ().color = newColor;
				}
	}
	// Update is called once per frame
	void Update () 
	{
				if (opacity < 1.0f) 
				{
						opacity += Time.deltaTime;
						setOpacity (opacity);
				}

				if (hasPowerUp) 
				{
						Vector3 rotation01 = power1.transform.eulerAngles;
						Vector3 rotation02 = power2.transform.eulerAngles;

						rotation01.z++;
						rotation02.z--;


						power1.transform.eulerAngles = rotation01;
						power2.transform.eulerAngles = rotation02;


				}
	}

	void OnTriggerEnter ( Collider other)
	{
		//	Debug.Log ("character Collided with " + other);
	}

	// change animation if you must here
	public void setCrotchBitten(bool bite)
	{
		Debug.Log ("Bitten the crotch!");
		crotchBitten = bite;
	}

	public void resetCharacter()
	{
			crotchBitten = false;
			isShowing = false;
			setCharacterlook ();
			gameObject.SetActive (false);
	}
	public void spawn()
	{
			crotchBitten = false;
			isShowing = true;
			setCharacterlook ();
			gameObject.SetActive (true);

			int checkPowerUp = (int)Random.Range (0,25.0f);
			
				if (checkPowerUp == 0) {
						hasPowerUp = true;
						power1.SetActive (true);
						power2.SetActive (true);

				}
				else 
				{
						hasPowerUp = false;
						power1.SetActive (false);
						power2.SetActive (false);
				}
		
	}
				
	public Collider getCollider()
	{
			return crotchCollider;
	}

	public void SetSpeed(float newSpeed)
	{
			speed = newSpeed;
	}
}
