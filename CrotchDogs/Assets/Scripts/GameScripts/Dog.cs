

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dog : MonoBehaviour 
{

	public CapsuleCollider DogTargetCollider;
	public List<Collider> CrotchObjects;
	public GameObject Jaws;
	public GameObject EndPoint;
	public GameObject dogCam;

	public bool boundingDog;
	//dogCam movement test
	public bool MovingUp=false;

	public Vector3 dogCamStartPos;
	public float moveDistance = 10.0f;
	public float MOVE_SPEED = 10.0f;

	public float moveDiff;
	// Use this for initialization
	void Start () 
	{
		dogCamStartPos = dogCam.transform.position;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (CrotchObject != null)
//		{
//				Vector3 difference =(DogTargetCollider.transform.position - CrotchObject.transform.position);
//				Debug.Log("Difference " + difference );
//		}	
				if (boundingDog) 
				{
						//move hte dogCam Up
						if (MovingUp) {
								if (dogCam.transform.position.y <= (dogCamStartPos.y + moveDistance)) 
								{ 
										moveDiff = Mathf.Lerp (dogCam.transform.position.y, (dogCamStartPos.y + (moveDistance + 1)), Time.deltaTime * MOVE_SPEED);
										dogCam.transform.position = new Vector3 (dogCam.transform.position.x, moveDiff, dogCam.transform.position.z);
								}
								else 
								{
										MovingUp = false;
								}


						} else {
								if (dogCam.transform.position.y >= (dogCamStartPos.y - moveDistance)) 
								{
										moveDiff = Mathf.Lerp (dogCam.transform.position.y, (dogCamStartPos.y - (moveDistance + 1)), Time.deltaTime * MOVE_SPEED);
										dogCam.transform.position = new Vector3 (dogCam.transform.position.x, moveDiff, dogCam.transform.position.z);
								}
								else 
								{
										MovingUp = true;
								}
						}
				}

	}

	public void Reset()
	{
			CrotchObjects = new List<Collider> ();
	}
	//Trigger Bite It returns the difference in location for Biting of the Crotch
	public float Bite()
	{
		float difference = CrotchDogConstants.NO_BITE;

		tk2dSpriteAnimator jawsBite =	Jaws.GetComponent<tk2dSpriteAnimator> ();
		//if (!jawsBite.IsPlaying("Bite")) 
		{
			jawsBite.Play ();
		}
		
		if (CrotchObjects != null)
		{

				foreach(Collider Crotch in CrotchObjects)
				{
						difference = DifferenceToMidpointOfLine (getCollider ().transform.position, EndPoint.transform.position, Crotch.transform.position);
				}
				//Debug.Log("Difference " + difference )
		
		}
		
		return difference;
	}
	


	//returns the collider
	public CapsuleCollider getCollider()
	{
			return DogTargetCollider;
	}

	//returns what its currently colliding with
	public List<Collider> getOtherCollider()
	{
			return CrotchObjects;
	}

	public void OnTriggerEnter ( Collider other)
	{
				CrotchObjects.Add(other);

				if (GameController.Instance.ActivePower == GameController.PowerUp.MAUL_MANIAC) 
				{
						GameController.Instance.biteCrotch ();
				}
	}

	public void OnTriggerExit ( Collider other)
	{
		
				if (CrotchObjects.Count > 0) 
				{
						SoundManager.PlaySwoosh ();
						CrotchObjects.Remove(other);
				}
	}

	public float DifferenceToMidpointOfLine(Vector3 p1,Vector3 p2, Vector3 objectP3)
	{
				/*
				// get the slope of p1 to p2 m =(y1-y2)/(x1-x2)
				float x1 = p1.y;
				float y1 = p1.z;

				float x2 = p2.y;
				float y2 = p2.z;

				float x3 = objectP3.y; 
				float y3 = objectP3.z;


				float A = (y1 - y2);
				float B = (x1 - x2);
				float m1 = ((y1 - y2) / (x1 - x2));
				//float m2Perp = B / A;
				//Debug.Log ("slope " + m1);
				//create a perpendicular line with a slope -m

		
//					Equation of a Straight Line
//					Slope (or Gradient)	Y Intercept
//					 
//					y = how far up
//
//					x = how far along
//
//					m = Slope or Gradient (how steep the line is)
//
//					b = the Y Intercept (where the line crosses the Y axis)



//				Take Point1=( 2, 1 )
//						Take Point2=( 5, 7 )
//
//						Find the LINEAR EQUATION of the line that passes through the points (2,1) and (5,7). Your answer must be in the form of Ax + By + C = 0.
//
//						Using the equation:
//						(y1 – y2)x + (x2 – x1)y + (x1y2 – x2y1) = 0
//
//						We’ll just plug numbers in:
//						(1 – 7)x + (5 – 2)y + ( (2 x 7) – (5 x 1) ) = 0
//						-6x + 3y + (14 – 5) = 0
//						-6x + 3y + 9 = 0
//
//						Factoring a -3 out:
//						-3( 2x – y – 3 ) = 0
//
//						Dividing both sides by -3:
//						2x – y – 3 = 0
				
						//find the linear equation of the line

						//Using the equation:
						//(y1 – y2)x + (x2 – x1)y + (x1y2 – x2y1) = 0

						float partX = (y1 -y2);//(y1 – y2)x
						float partY = (x2 - x1);//(x2 – x1)y
						float partC = (x1 * y2 - x2 * y1);  //(x1y2 – x2y1)
					
						//d = |Am + Bn + C|/ sqrt( A*A +B*B)
					float distance = (A * x3 + B * y3 + partC) / Mathf.Sqrt (A * A + B * B);
					//Debug.Log ("distance "+ distance);

				//y = mx + 1 
			 	//y = mx + b
				*/
//				Vector3 point = objectP3;
//				Vector3 lineStart = p1;
//				Vector3 lineEnd = p2;
				float distance = DistancePointLine (objectP3, p1,p2);
//				Debug.Log ("calculated distance " + distance);
				return distance;
	}

		public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
		{
				//Debug.Log ("distance from points");
				return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
		}
		public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
		{
				Vector3 rhs = point - lineStart;
				Vector3 vector2 = lineEnd - lineStart;
				float magnitude = vector2.magnitude;
				Vector3 lhs = vector2;
				if (magnitude > 1E-06f)
				{
						lhs = (Vector3)(lhs / magnitude);
				}
				float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
				return (lineStart + ((Vector3)(lhs * num2)));
		}

}
