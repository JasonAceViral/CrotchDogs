using UnityEngine;
using System.Collections;

namespace AceViral {
    public class RotateForever : MonoBehaviour {

    	public float xSpeed = 0.0f;
    	public float ySpeed = 0.0f;
        public float zSpeed = 0.0f;

        private float m_xSpeed = 0.0f;
        private float m_ySpeed = 0.0f;
        private float m_zSpeed = 0.0f;

        private Vector3 updateVector = Vector3.zero;
    	
    	// Update is called once per frame
    	void Update () {
    	
            if (m_xSpeed != xSpeed)
            {
                m_xSpeed = xSpeed;
                UpdateRotationVector();
            }
            if (m_ySpeed != ySpeed)
            {
                m_ySpeed = ySpeed;
                UpdateRotationVector();
            }
            if (m_zSpeed != zSpeed)
            {
                m_zSpeed = zSpeed;
                UpdateRotationVector();
            }
    		transform.Rotate(updateVector);
    	}

        void UpdateRotationVector()
        {
            updateVector = new Vector3 (xSpeed, ySpeed, zSpeed);
        }
    }
}