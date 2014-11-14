using UnityEngine;
using System.Collections;

public class CameraPostRender : MonoBehaviour {

		public MyBezier bez;


		void OnPostRender()
		{

				if (bez != null) {
						bez.OnPostRender ();
				}
		}
}
