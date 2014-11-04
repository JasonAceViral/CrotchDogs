using UnityEngine;
using System.Collections;

public class MeshFilterTest : MonoBehaviour {
	
	public GameObject m_baseGameObject;
	
	public Mesh[] m_MeshArray;
	private int m_MeshArrayIndex = 0;
	
	float m_Timer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		m_Timer += Time.deltaTime;
		if(m_Timer >= 1.0f){
			m_Timer = 0.0f;
			m_baseGameObject.GetComponent<MeshFilter>().mesh = m_MeshArray[m_MeshArrayIndex];
			m_MeshArrayIndex++;
			if(m_MeshArrayIndex >= m_MeshArray.Length){
				m_MeshArrayIndex = 0;	
			}
		}
	}
	
	
}
