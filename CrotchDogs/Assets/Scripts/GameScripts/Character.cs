using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float speed = -10.0f;
	public GameObject head,body,legs,ground;
	public bool crotchBitten = false,isShowing = false;
	public Collider crotchCollider;
	public Vector2[] legAnchors;
	public const int TOP_ORDER_IN_LAYER = 100;
		public bool setCharacterType = false;
		public int characterType = 0;
	 
	public const string BODY_SPRITE = "torso",HEAD_SPRITE = "nut",LEGS_SPRITE = "pins";
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

				if (!setCharacterType) {
						//Debug.Log ("Character Start");
						int headRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
						int bodyRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;
						int legsRandom = Random.Range (0, CrotchDogConstants.NUM_OF_CHARACTERS) + 1;

						head.GetComponent<tk2dSprite> ().SetSprite (HEAD_SPRITE + headRandom);
						body.GetComponent<tk2dSprite> ().SetSprite (BODY_SPRITE + bodyRandom);
						legs.GetComponent<tk2dSprite> ().SetSprite (LEGS_SPRITE + legsRandom);

						legs.transform.localPosition = legAnchors [legsRandom - 1];
				} else {
						head.GetComponent<tk2dSprite> ().SetSprite (HEAD_SPRITE + characterType);
						body.GetComponent<tk2dSprite> ().SetSprite (BODY_SPRITE + characterType);
						legs.GetComponent<tk2dSprite> ().SetSprite (LEGS_SPRITE + characterType);

						legs.transform.localPosition = legAnchors [characterType - 1];
				}

			//Debug.Log ("character awake " +legsRandom );
	}
	// Update is called once per frame
	void Update () 
	{
	
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
		
	}

	public Collider getCollider()
	{
			return crotchCollider;
	}
}
