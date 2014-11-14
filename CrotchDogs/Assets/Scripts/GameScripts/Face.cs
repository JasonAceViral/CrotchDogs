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
	// Use this for initialization
	void Start () {
	
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

	
	}

	public void setFacetoState(stateOfFaces state)
	{


				switch (state) 
				{
					case stateOfFaces.bite: 
							FaceSprite.SetSprite ("faceBITE");
							faceChangeTime = CrotchDogConstants.BITE_TIME;
							break;
					case stateOfFaces.maul: 
							FaceSprite.SetSprite ("faceMAUL");
							faceChangeTime = CrotchDogConstants.MAUL_TIME;
							break;
					case stateOfFaces.idle: 
							FaceSprite.SetSprite ("faceIDLE");
							faceChangeTime = CrotchDogConstants.IDLE_TIME;
							break;
					case stateOfFaces.miss: 
							FaceSprite.SetSprite ("faceMISS");
							faceChangeTime = CrotchDogConstants.MISS_TIME;
							break;

					default:
							break;
				}

	}

}
