using UnityEngine;
using System.Collections;

public class HouseController : MonoBehaviour 
{
	public float SpawnInterval = 1.0f,speed = -1.0f;
	public GameObject SpawnPoint, ResetPoint;
	public GameObject[] Houses;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < Houses.Length; i++) 
		{
			Houses [i].transform.position = new Vector3 (Houses [i].transform.position.x,Houses [i].transform.position.y,Houses [i].transform.position.z + speed*Time.deltaTime);

			if (Houses [i].transform.position.z <= ResetPoint.transform.position.z) 
			{
					Houses [i].transform.position = SpawnPoint.transform.position;
			}
		}
	}
}
