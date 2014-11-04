using UnityEngine;
using System.Collections.Generic;

public class MyBezier : MonoBehaviour
{
		private List<Bezier> myBezier;
		private float t = 0f,timeInterval =0.05f;

		static Material lineMaterial;

		private Vector3 startPoint, EndPoint;
		public GameObject Marker;

		private int pointIndex =0;

		public List<GameObject> points;

		public bool PointsChanged = true;

		void Start()
		{
				startPoint = points[0].transform.position;
				EndPoint = points[points.Count-3].transform.position;
				myBezier = new List<Bezier> ();

				//lines come in 4 parts always have the first point at the same position as the last one
				for (int i = 0; i < points.Count-1; i+=3) 
				{
						if((i + 3) < points.Count)
						{
								Debug.Log ("points " + (i) + "," + (i + 1) + "," + (i + 2) + "," + (i + 3) + ",");
								myBezier.Add (new Bezier (points [i].transform.position, points [i + 1].transform.position, points [i + 2].transform.position, points [i + 3].transform.position));
						}
						//myBezier.Add( new Bezier (points[i].transform.position, Random.insideUnitSphere * 3.1f, Random.insideUnitSphere * 3.1f, points[i+1].transform.position));
				}

		}

		void OnDrawGizmos()
		{
				updatePoints ();

				if (myBezier != null) 
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
		
				//moves the marker along the line
				Vector3 vec = myBezier[pointIndex].Get2DPointAtTime( t );
				Marker.transform.position = vec + transform.position;

				t += 0.01f;//Time.deltaTime;
				//Debug.Log ("point Index incremented" + pointIndex + " beizer count " + myBezier.Count + " time "+ t);
				if (t > 1.0f) 
				{
					t = 0f;
					pointIndex++;
	
					if (pointIndex >= myBezier.Count) 
					{
						pointIndex = 0;
					}
				}
		}

		void updatePoints()
		{
				//if points have moved update bezier
				//if (startPoint !=points[0].transform.position || EndPoint !=  points[points.Count-1].transform.position) 
				{
						myBezier = new List<Bezier> ();
						PointsChanged = true;
						startPoint = points[0].transform.position;
						EndPoint = points[points.Count-1].transform.position;




						for (int i = 0; i < points.Count-3; i+=3) 
						{

								if((i + 3) < points.Count)
								{
									//	Debug.Log ("points " + (i) + "," + (i + 1) + "," + (i + 2) + "," + (i + 3) + ",");
										myBezier.Add (new Bezier (points [i].transform.position, points [i + 1].transform.position, points [i + 2].transform.position, points [i + 3].transform.position));
								}
								//myBezier.Add( new Bezier (points[i].transform.position,  points[i+1].transform.position,  points[i+2].transform.position, points[i+3].transform.position));

								//myBezier.Add( new Bezier (points[i].transform.position, Random.insideUnitSphere * 3.1f, Random.insideUnitSphere * 3.1f, points[i+1].transform.position));
						}				
				}
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
				drawBezierQuadLines (0.025f);
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
				if (myBezier != null) 
				{
						//PointsChanged = false;
						Vector3 start = points[0].transform.position;
						float time = 0.0f;
						int bezierCount = 0;
						GL.PushMatrix ();
						GL.MultMatrix (transform.localToWorldMatrix);
						//GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
						//GL.MultMatrix(Camera.main.worldToCameraMatrix);

						Color firstColor = new  Color(1.0f,0.0f,0.0f,0.5f) ;

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
				if (myBezier != null) 
				{
						//PointsChanged = false;
						Vector3 start = points[0].transform.position;
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
						GL.Color( firstColor  );

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

}