using UnityEngine;
using System.Collections;

namespace AceViral {
    public class BillboardPlane : MonoBehaviour {
    	public Transform CameraTransform;
    	public Transform m_Transform;
    	public bool rotated = true;
    	
    	void Start() {
    		CameraTransform = Camera.main.transform;	
    		m_Transform = this.gameObject.transform;
    	}
    	
    	void Update() {
    		m_Transform.LookAt(CameraTransform.position);	
    		if(rotated){
    			m_Transform.Rotate(90,0,0);
    		} else {
    			m_Transform.Rotate(0,0,90);
    		}	
    		
    	}
    }
}
