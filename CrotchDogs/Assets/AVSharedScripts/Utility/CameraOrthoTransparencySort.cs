using UnityEngine;
using System.Collections;

namespace AceViral {
    [RequireComponent(typeof(Camera))]
    public class CameraOrthoTransparencySort : MonoBehaviour {
    	void Start(){
    		camera.transparencySortMode = TransparencySortMode.Orthographic;
    	}
    }
}
