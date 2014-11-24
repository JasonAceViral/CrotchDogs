using UnityEngine;
using System.Collections.Generic;

public class MyBezier : MonoBehaviour
{
		private const int MAX_POINTS = 25, MAX_DRAW_POINTS = 16;

		public  List<Bezier> myBezier;
		private float timeInterval =0.05f;

		static Material lineMaterial;

		public GameObject pointEnd, pointStart;
		public GameObject DogMarker,DogCatcherMarker;

		public List<GameObject> points;

		private bool createdAllPoints = false;

		public float dogMarkerLocation,catcherMarkerLocation;
		private Vector3 dogMarkerStart, catcherMarkerStart;

		void Start()
		{
				myBezier = new List<Bezier> ();
				points = new List<GameObject> ();

				float diff = (pointStart.transform.position.x - pointEnd.transform.position.x) / (MAX_DRAW_POINTS);

				dogMarkerLocation = DogMarker.transform.position.x;
				catcherMarkerLocation = DogCatcherMarker.transform.position.x;

				dogMarkerStart = DogMarker.transform.position;
				catcherMarkerStart = DogCatcherMarker.transform.position;

				for (int i = 0; i < MAX_POINTS; i++) 
				{
						float rand = Random.Range (-0.8f, 0.8f);
						GameObject newPoint = new GameObject ("bezier point");
					//	Debug.Log ("rand" + rand);
						newPoint.transform.position =  new Vector3 (pointStart.transform.position.x - diff*i,pointStart.transform.position.y + rand,pointStart.transform.position.z);
						newPoint.transform.parent = gameObject.transform;
						points.Add (newPoint);
				}

				createdAllPoints = true;

				changeBezier ();
		}
		// adds a new point to points list
		void addNewPoint()
		{
				float diff = (pointStart.transform.position.x - pointEnd.transform.position.x) / (MAX_DRAW_POINTS);
				float rand = Random.Range (-0.8f, 0.8f);
				GameObject newPoint = new GameObject("new point");
				newPoint.transform.parent = points [0].transform.parent;
				newPoint.transform.position =	new Vector3 (points [points.Count-1].transform.position.x - diff,pointStart.transform.position.y + rand,pointStart.transform.position.z);
				points.Add (newPoint);

		}

		public void changeBezier()
		{
				//lines come in 4 parts always have the first point at the same position as the last one
				for (int i = 0; i < points.Count-1; i+=3) 
				{
						if((i + 3) < points.Count)
						{
								//Debug.Log ("points " + (i) + "," + (i + 1) + "," + (i + 2) + "," + (i + 3) + ",");
								myBezier.Add (new Bezier (points [i].transform.localPosition, points [i + 1].transform.localPosition, points [i + 2].transform.localPosition, points [i + 3].transform.localPosition));
						}
						//myBezier.Add( new Bezier (points[i].transform.position, Random.insideUnitSphere * 3.1f, Random.insideUnitSphere * 3.1f, points[i+1].transform.position));
				}
		}
		/*
		 *UPDATES the points locatiosn and spawn any new points if needed
		 */
		void updatePointLocations()
		{
			float time = Time.deltaTime;
			//pointAtTime = time;

			gameObject.transform.transform.position = new Vector3 (gameObject.transform.position.x - CrotchDogConstants.POINTS_MOVE_SPEED*time ,gameObject.transform.position.y,gameObject.transform.position.z);

//			for (int i = 0; i < points.Count; i++) 
//			{
//				
//					points [i].transform.position = new Vector3 (points [i].transform.position.x - CrotchDogConstants.POINTS_MOVE_SPEED*time ,points [i].transform.position.y,points [i].transform.position.z);
//			}
//

			//check if the 2nd last point is less than the startpoint then add a new point and remove hte old one
			if(points[4].transform.position.x < pointStart.transform.position.x)
			{


					//Debug.Log ("remove point 0");


					GameObject remove1 = points [0];
					GameObject remove2 = points [1];
					GameObject remove3 = points [2];

					points.Remove (remove1);
					points.Remove (remove2);
					points.Remove (remove3);

					Destroy (remove1);
					Destroy (remove2);
					Destroy (remove3);


					addNewPoint ();
					addNewPoint ();
					addNewPoint ();

					Bezier removeBez = myBezier [0];

					myBezier.Remove (removeBez);


					myBezier.Add (new Bezier (points[points.Count-4].transform.localPosition, points[points.Count-3].transform.localPosition, points[points.Count-2].transform.localPosition, points[points.Count-1].transform.localPosition));

			
			}



		}

		void OnDrawGizmos()
		{
				updatePoints ();

				if (myBezier != null && createdAllPoints ) 
				{
						//PointsChanged = false;
						Vector3 start = points[0].transform.position +transform.position;
						float time = 0.0f;
						int bezierCount = 0;
					//	Debug.Log (" draw gizmo");
						while (bezierCount < myBezier.Count) 
						{
								Vector3 pointAtTime = myBezier [bezierCount].Get2DPointAtTime (time) +transform.position;

								Gizmos.DrawLine (start, pointAtTime);
								Debug.DrawLine (start, pointAtTime);

								start = pointAtTime;

								time += timeInterval;

								if (time >= 1.0f) 
								{
										time = 0.0f;
										bezierCount++;
								}
						}
				}
		}

		void Update()
		{
		
				if (createdAllPoints) 
				{
						//gameObject.transform.position =new Vector3(-Marker.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
						updatePointLocations ();

						if (GameController.Instance != null) {
								updateMarkerLocation ();
						}
				}
		}

		void updateMarkerLocation()
		{
				//finds the time interval for the x value of the marker
				float findTime = 0;
				bool foundTime = false;
				Vector3 vec;

				// there is a possibility that if a player si good enough that it can go minus
				int chaseValue = GameController.Instance.chaseController.getDogIndex ();

				if (chaseValue < 0) 
				{
						chaseValue = 0;
				}
				dogMarkerLocation = dogMarkerStart.x + chaseValue*CrotchDogConstants.DOG_INCREMENT;


				chaseValue = GameController.Instance.chaseController.getChaserIndex ();
				if (chaseValue < 0) 
				{
						chaseValue = 0;
				}

				catcherMarkerLocation = catcherMarkerStart.x + chaseValue*CrotchDogConstants.CHASER_INCREMENT;

				//move the dog marker towards the new location
				if (DogMarker.transform.position.x != dogMarkerLocation) 
				{

						DogMarker.transform.position = new Vector3 (DogMarker.transform.position.x + ( dogMarkerLocation - DogMarker.transform.position.x)*Time.deltaTime,DogMarker.transform.position.y,DogMarker.transform.position.z);
				}

				//move the dog Catcher marker towards the new location
				if (DogCatcherMarker.transform.position.x != catcherMarkerLocation) 
				{

						DogCatcherMarker.transform.position = new Vector3 (DogCatcherMarker.transform.position.x + (catcherMarkerLocation - DogCatcherMarker.transform.position.x)*Time.deltaTime,DogCatcherMarker.transform.position.y,DogCatcherMarker.transform.position.z);
				}

				//Game Over
				if (DogMarker.transform.position.x <= DogCatcherMarker.transform.position.x) {
						GameController.Instance.callGameOver ();
				}

				// find the Dog markers Y location for its X
				for (int i = 0; i < myBezier.Count && !foundTime; i++) 
				{
						findTime = 0.0f;
						foundTime = false;
						while (findTime < 1.0f && !foundTime)
						{
								findTime += 0.01f;
								vec = myBezier [i].Get2DPointAtTime (findTime);

								if ((vec.x + gameObject.transform.position.x) > DogMarker.transform.position.x) 
								{
										DogMarker.transform.position = new Vector3 (DogMarker.transform.position.x, vec.y + gameObject.transform.position.y, DogMarker.transform.position.z);
										foundTime = true;
								}
						}
				}

				findTime = 0.0f;
				foundTime = false;

				//find the Chasers Y location from its X
				for (int i = 0; i < myBezier.Count && !foundTime; i++) 
				{
						findTime = 0.0f;
						foundTime = false;
						while (findTime < 1.0f && !foundTime) 
						{
								findTime += 0.01f;
								vec = myBezier [i].Get2DPointAtTime (findTime);

								if ((vec.x + gameObject.transform.position.x) > DogCatcherMarker.transform.position.x) 
								{
										DogCatcherMarker.transform.position = new Vector3 (DogCatcherMarker.transform.position.x, vec.y + gameObject.transform.position.y, DogCatcherMarker.transform.position.z);
										foundTime = true;
								}
						}
				}
		}

		void updatePoints()
		{
				//if points have moved update bezier
				//if (points != null && createdAllPoints) 
				//{
						//myBezier = new List<Bezier> ();
//
//						PointsChanged = true;
//
////						for (int i = 0; i < points.Count-3; i+=3) 
////						{
////
////								if((i + 3) < points.Count)
////								{
////									//	Debug.Log ("points " + (i) + "," + (i + 1) + "," + (i + 2) + "," + (i + 3) + ",");
////										myBezier.Add (new Bezier (points [i].transform.position, points [i + 1].transform.position, points [i + 2].transform.position, points [i + 3].transform.position));
////								}
////								//myBezier.Add( new Bezier (points[i].transform.position,  points[i+1].transform.position,  points[i+2].transform.position, points[i+3].transform.position));
////
////								//myBezier.Add( new Bezier (points[i].transform.position, Random.insideUnitSphere * 3.1f, Random.insideUnitSphere * 3.1f, points[i+1].transform.position));
////						}	
//
						//changeBezier ();


				//}
		}

		static void CreateLineMaterial()
		{
				if( lineMaterial == null) 
				{
						lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
								"SubShader { Pass { " +
								"    Blend SrcAlpha OneMinusSrcAlpha " +
								"    ZWrite Off Cull Off Fog { Mode Off } " +
								"    BindChannels {" +
								"      Bind \"vertex\", vertex Bind \"color\", color }" +
								"} } }" );
						lineMaterial.hideFlags = HideFlags.HideAndDontSave;
						lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
				}
		}

		public void OnPostRender()
		{

				if (GameController.Instance.getState() == GameController.GameState.PLAYING_GAME) 
				{
						drawBezierQuadLines (0.025f);
				}
		
				//Successfully draws a cube
//				GL.PushMatrix ();
//				GL.MultMatrix (transform.localToWorldMatrix);
//				//GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
//				//GL.MultMatrix(Camera.main.worldToCameraMatrix);
//
//				Color firstColor = new  Color(1.0f,1.0f,0.0f,0.5f) ;
//				Color secondColor = new  Color(1.0f,0.0f,1.0f,0.5f) ;
//				CreateLineMaterial();
//				lineMaterial.SetPass( 0 );
//
//				GL.Begin( GL.LINES );
//				GL.Color( firstColor  );
//				GL.Vertex3( 0, 0, 0 );
//				GL.Vertex3( 1, 0, 0 );
//				GL.Vertex3( 0, 1, 0 );
//				GL.Vertex3( 1, 1, 0 );
//				GL.Color(secondColor );
//				GL.Vertex3( 0, 0, 0 );
//				GL.Vertex3( 0, 1, 0 );
//				GL.Vertex3( 1, 0, 0 );
//				GL.Vertex3( 1, 1, 0 );
//				GL.End();
//
//				GL.PopMatrix ();
		}

		public void drawBezierLines()
		{
				if (myBezier != null && createdAllPoints) 
				{
						//PointsChanged = false;
						Vector3 start = points[0].transform.position;
						float time = 0.0f;
						int bezierCount = 0;
						GL.PushMatrix ();
						GL.MultMatrix (transform.localToWorldMatrix);
						//GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
						//GL.MultMatrix(Camera.main.worldToCameraMatrix);

						Color firstColor = new  Color(1.0f,0.0f,0.0f,0.5f);

						CreateLineMaterial();
						lineMaterial.SetPass( 0 );
						GL.Begin( GL.LINES );
						GL.Color( firstColor  );

						//	Debug.Log (" draw gizmo");
						while (bezierCount < myBezier.Count) 
						{
								Vector3 pointAtTime = myBezier [bezierCount].Get2DPointAtTime (time);

								//call GL function to draw lines

								GL.Vertex3( start.x,start.y,start.z);
								GL.Vertex3( pointAtTime.x,pointAtTime.y,pointAtTime.z );

								start = pointAtTime;

								time += timeInterval;

								if (time >= 1.0f) 
								{
										time = 0.0f;
										bezierCount++;
								}
						}
						//end gl calls
						GL.End();

						GL.PopMatrix ();
				}
		}

		public void drawBezierQuadLines( float width)
		{
				if (myBezier != null && points != null && myBezier.Count >0) 
				{
						//PointsChanged = false;
						Vector3 start = myBezier [0].Get2DPointAtTime (0.0f);

						//Debug.Log ("new Points");

						float time = 0.0f;
						int bezierCount = 0;
						GL.PushMatrix ();
						GL.MultMatrix (transform.localToWorldMatrix);
						//GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
						//GL.MultMatrix(Camera.main.worldToCameraMatrix);

						Color firstColor = new  Color(1.0f,0.0f,0.0f,1.0f) ;

						CreateLineMaterial();
						lineMaterial.SetPass( 0 );
						GL.Begin( GL.QUADS);
						GL.Color( firstColor );

						//	Debug.Log (" draw gizmo");
						while (bezierCount < myBezier.Count) 
						{
								Vector3 pointAtTime = myBezier [bezierCount].Get2DPointAtTime (time);

								//call GL function to draw lines

								//top left
								GL.Vertex3( start.x,start.y,start.z);
								//bottom left
								GL.Vertex3( start.x,start.y-width,start.z);

								//bottom right
								GL.Vertex3( pointAtTime.x,pointAtTime.y-width,pointAtTime.z );
								//top right
								GL.Vertex3( pointAtTime.x,pointAtTime.y,pointAtTime.z );
							
								//Debug.Log ("start Points" + start);

								start = pointAtTime;

								time += timeInterval;

								if (time >= 1.0f) 
								{
										time = 0.0f;
										bezierCount++;
								}
						}
						//end gl calls
						GL.End();

						GL.PopMatrix ();



				}
		}
				
		public void reset()
		{
				Debug.Log ("state point " + dogMarkerStart + " catcher " + catcherMarkerStart);

				if (dogMarkerStart.x != 0) {
						DogMarker.transform.position = dogMarkerStart;
						DogCatcherMarker.transform.position = catcherMarkerStart;
				}

				myBezier = new List<Bezier> ();
				changeBezier ();
		}

}