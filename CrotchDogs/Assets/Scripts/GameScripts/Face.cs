using UnityEngine;
using System.Collections;

public class Face : MonoBehaviour {


	public enum stateOfFaces
	{
			idle,maul,bite,miss
	}

	public tk2dSprite FaceSprite;
	float faceChangeTime;
	int faceState;
	bool Animating = false;
	float AnimationTime = 0.0f;
	private Vector3 startPosition;
	private stateOfFaces currentState =stateOfFaces.idle;

	public float SHAKE_DISTANCE = 0.5f,SHAKE_SPEED =150.0f;

	public const float SCALE_MAUL = 1.2f,SCALE_MAUL_RATE=0.1f,MISS_ROTATION = 0.3f,MISS_ROTATION_RATE = 0.05f;
	public bool rotateRight=true;

	// Use this for initialization
	void Awake () 
	{
		startPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
			if (faceChangeTime > 0.0f) 
			{
				faceChangeTime -= Time.deltaTime;
		
			}
			else if( faceChangeTime < 0.0f)
			{
				setFacetoState (stateOfFaces.idle);
			}

			animate ();
	}

	public void setFacetoState(stateOfFaces state)
	{
				if (!Animating) 
				{
						switch (state) 
						{
						case stateOfFaces.bite: 
								FaceSprite.SetSprite (getBite ());
								faceChangeTime = CrotchDogConstants.BITE_TIME;
								break;
						case stateOfFaces.maul: 
								FaceSprite.SetSprite (getMaul ());
								faceChangeTime = CrotchDogConstants.MAUL_TIME;
								break;
						case stateOfFaces.idle: 
								FaceSprite.SetSprite (getIdle ());
								faceChangeTime = CrotchDogConstants.IDLE_TIME;
								break;
						case stateOfFaces.miss: 
								FaceSprite.SetSprite (getMiss ());
								faceChangeTime = CrotchDogConstants.MISS_TIME;
								break;
						default:
								break;
						}
						currentState = state;
						AnimationTime = 0.0f;
				}

	}

		public void animate()
		{
				AnimationTime += Time.deltaTime;


				Quaternion rotationToZero = FaceSprite.transform.rotation;

				switch (currentState) 
				{
				case stateOfFaces.bite: //up down shake
					
						if (gameObject.transform.position.x == startPosition.x) 
						{
								gameObject.transform.position = new Vector3 (gameObject.transform.position.x + SHAKE_DISTANCE , gameObject.transform.position.y, gameObject.transform.position.z );
						}
						else 
						{
								gameObject.transform.position = new Vector3 (startPosition.x, gameObject.transform.position.y, gameObject.transform.position.z );
						}

						if (FaceSprite.scale.x > 1.0f) 
						{
								float newScale = FaceSprite.scale.x - SCALE_MAUL_RATE;

								FaceSprite.scale = new Vector3(newScale,newScale,newScale);
						}

						if (rotationToZero.z != 0.0f) {
								rotationToZero.z = Time.deltaTime * (-rotationToZero.z);

								FaceSprite.transform.rotation = rotationToZero;
						}
					
					
							break;
				case stateOfFaces.maul: //left right shake scale up and down

						if (gameObject.transform.position.y == startPosition.y) 
						{
								gameObject.transform.position = new Vector3 ( gameObject.transform.position.x, gameObject.transform.position.y + SHAKE_DISTANCE, gameObject.transform.position.z);
						}
						else 
						{
								gameObject.transform.position = new Vector3 ( gameObject.transform.position.x, startPosition.y, gameObject.transform.position.z);
						}

			

						if (FaceSprite.scale .x < SCALE_MAUL) 
						{
								float newScale = FaceSprite.scale .x + SCALE_MAUL_RATE;

								FaceSprite.scale  = new Vector3(newScale,newScale,newScale);
						}

						if (rotationToZero.z != 0.0f) {
								rotationToZero.z = Time.deltaTime * (-rotationToZero.z);

								FaceSprite.transform.rotation = rotationToZero;
						}

							break;
				case stateOfFaces.idle: 
						if (gameObject.transform.position != startPosition) {
								gameObject.transform.position = startPosition;
						}

						if (FaceSprite.scale.x > 1.0f) {
								float newScale = FaceSprite.scale.x - SCALE_MAUL_RATE;
								FaceSprite.scale = new Vector3 (newScale, newScale, newScale);
						}

						if (rotationToZero.z != 0.0f) {
								rotationToZero.z = Time.deltaTime * (-rotationToZero.z);

								FaceSprite.transform.rotation = rotationToZero;
						}
							break;
				case stateOfFaces.miss: //rotate
						if (gameObject.transform.position != startPosition) {
								gameObject.transform.position = startPosition;
						}

						if (FaceSprite.scale.x > 1.0f) {
								float newScale = FaceSprite.scale.x - SCALE_MAUL_RATE;
								FaceSprite.scale = new Vector3 (newScale, newScale, newScale);

						}

				
						{
								Quaternion rotation = FaceSprite.transform.rotation;

						

								if (rotateRight) 
								{

										if (rotation.z < MISS_ROTATION) 
										{
												rotation.z += MISS_ROTATION_RATE;
										}
										else 
										{
												rotateRight = false;
										}

										FaceSprite.transform.rotation = rotation;

								}
								else 
								{

										if (rotation.z > -MISS_ROTATION) 
										{
												rotation.z -= MISS_ROTATION_RATE;
										}
										else 
										{
												rotateRight = true;
										}

										FaceSprite.transform.rotation = rotation ;
								}
						}



							break;
					default:
							break;
				}

		}

		public string getBite()
		{
				int faceNum = Random.Range (1, 6);

				return "bite" + faceNum;
		}
		public string getMaul()
		{
				int faceNum = Random.Range (1, 6);

				return "maul" + faceNum;
		}
		public string getMiss()
		{
				int faceNum = Random.Range (1, 6);
				Debug.Log ("miss"+faceNum);
				return "miss" + faceNum;
		}
		public string getIdle()
		{
				return "idle";
		}
}
