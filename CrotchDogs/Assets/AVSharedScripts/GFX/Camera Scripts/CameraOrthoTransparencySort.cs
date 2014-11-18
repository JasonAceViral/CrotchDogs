using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraOrthoTransparencySort : MonoBehaviour {
	void Start(){
		camera.transparencySortMode = TransparencySortMode.Orthographic;
	}
}
