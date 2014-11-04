

using UnityEngine;
using System.Collections;



public class Dog : MonoBehaviour 
{

	public CapsuleCollider DogTargetCollider;
	Collider CrotchObject;
	public GameObject Jaws;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (CrotchObject != null)
//		{
//				Vector3 difference =(DogTargetCollider.transform.position - CrotchObject.transform.position);
//				Debug.Log("Difference " + difference );
//		}	
	}
	
	//Trigger Bite It returns the difference in location for Biting of the Crotch
	public float Bite()
	{
				Vector3 difference = new Vector3 (CrotchDogConstants.NO_BITE,CrotchDogConstants.NO_BITE);
		if (CrotchObject != null)
		{
				 difference =(DogTargetCollider.transform.position - CrotchObject.transform.position);
				//Debug.Log("Difference " + difference );
		
		}
		
		return difference.z;
	}
	


	//returns the collider
	public CapsuleCollider getCollider()
	{
			return DogTargetCollider;
	}

	//returns what its currently colliding with
	public Collider getOtherCollider()
	{
			return CrotchObject;
	}

	void OnTriggerEnter ( Collider other)
	{
		//Debug.Log ("Enter " + other);
		CrotchObject = other;
				Vector3 difference =(DogTargetCollider.transform.position - CrotchObject.transform.position);
				Debug.Log("Enter " + difference );
	}

	void OnTriggerExit ( Collider other)
	{
				Vector3 difference =(DogTargetCollider.transform.position - CrotchObject.transform.position);
				Debug.Log("Exit " + difference );
		//Debug.Log ("Exit " + other);
		CrotchObject = null;
	}
}
