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
							FaceSprite.SetSprite (getBite());
							faceChangeTime = CrotchDogConstants.BITE_TIME;
							break;
					case stateOfFaces.maul: 
							FaceSprite.SetSprite (getMaul());
							faceChangeTime = CrotchDogConstants.MAUL_TIME;
							break;
					case stateOfFaces.idle: 
							FaceSprite.SetSprite (getIdle());
							faceChangeTime = CrotchDogConstants.IDLE_TIME;
							break;
					case stateOfFaces.miss: 
							FaceSprite.SetSprite (getMiss());
							faceChangeTime = CrotchDogConstants.MISS_TIME;
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
