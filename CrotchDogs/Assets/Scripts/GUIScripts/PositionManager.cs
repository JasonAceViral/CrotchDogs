using UnityEngine;
using System.Collections;

public class PositionManager : MonoBehaviour {

	public GameObject Road,PauseButton,SFXMute,Mute,Face,ScoreBoard;

	public tk2dCamera cam;
	public float screenWidth,screenHeight;


	public float moveWidth,moveheight;
	// Use this for initialization
	void Start () {
				//if Camera has not been set
				if (cam == null) 
				{
						cam = (tk2dCamera.Instance);
				}

				screenWidth = cam.ScreenExtents.width;
				screenHeight = cam.ScreenExtents.height;


				Road.transform.position = new Vector3 (Road.transform.position.x, -screenHeight*0.5f + Road.GetComponent<tk2dSprite>().GetBounds().size.y*0.5f,Road.transform.position.z);
	
				MoveGameObject(Face,-0.38f,0.42f);
				MoveGameObject(PauseButton,0.4f,0.45f);
				MoveGameObject(SFXMute,0.25f,0.45f);
				MoveGameObject(Mute,0.1f,0.45f);
				MoveGameObject (ScoreBoard, -0.1f, 0.45f);
	}

		void Update()
		{

				//MoveGameObject (PauseButton, moveWidth, moveheight);
		}
	
		public void MoveGameObject(GameObject theGameObject,float xAmount, float yAmount)
		{

				theGameObject.transform.position = new Vector3 (cam.transform.position.x  + screenWidth*xAmount ,cam.transform.position.y + screenHeight*yAmount ,theGameObject.transform.position.z);

		}
}
