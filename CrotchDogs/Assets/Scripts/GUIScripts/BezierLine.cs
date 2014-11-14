using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class BezierLine : MonoBehaviour 
{

	public List<GameObject> points;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	
	}
		#if UNITY_EDITOR
		void OnSceneGUI()
		{
				Debug.Log ("Draw Bezier Curve");

				if (points.Count >= 2) {
					
						float width = HandleUtility.GetHandleSize (Vector3.zero) * 0.1f;
						Debug.Log ("make Bezier Curve " + points [0].transform.position + " width " + width);


						Handles.DrawBezier (points [0].transform.position, 
								Vector3.zero, 
								Vector3.up, 
								-Vector3.up,
								Color.red, 
								null,
								width);
				}

		}

		public Vector3 startPosition = Vector3.zero;
		public Vector3 endPosition = Vector3.left;
		public Vector3 startTangent = Vector3.up;
		public Vector3 endTangent = Vector3.back;
		public int numberOfSubdivisions = 10;

		void OnDrawGizmos(){
				numberOfSubdivisions = Mathf.Max(1,numberOfSubdivisions);
				Vector3[] array = new Vector3[numberOfSubdivisions+1];

				// B(t) = (1-t)^3 * startPosition + 3 * (1-t)^2 * t * startTangent + 3 * (1-t) * t^2 * endTangent + t^3 * endPosition, t=[0,1]
				for (int i=0;i<=numberOfSubdivisions;i++)
				{
						float t = i/(float)numberOfSubdivisions;
						float omt = 1.0f-t; // One minus t = omt
						array[i] = startPosition*(omt*omt*omt) +
								startTangent*(3*omt*omt*t) + 
								endTangent*(3*omt*t*t)+
								endPosition*(t*t*t);
						if (i>0)
						{

								Gizmos.DrawLine(array[i-1],array[i]);
						}
				}
		}
		#endif
}
