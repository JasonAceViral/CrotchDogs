using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObjectSpawner : MonoBehaviour 
{
	public List<GameObject> spawnedList;
	public float speed;
	public GameObject SpawnPoint,ResetPoint;
	public Vector3 changeScale = new Vector3(1.0f,1.0f,1.0f);
	public float SPAWN_INTERVAL = 1.0f,timeSinceSpawn=0.0f;

	// Use this for initialization
	void Awake () 
	{
		if (spawnedList == null) 
		{
				spawnedList = new List<GameObject> ();
		}
	}
	
	// Update is called once per frame
	// Will spawn new objects and move current objects
	void Update () 
	{

				if (GameController.Instance.getState () == GameController.GameState.PLAYING_GAME) 
				{
						timeSinceSpawn += Time.deltaTime;

						if (timeSinceSpawn >= SPAWN_INTERVAL) {
								timeSinceSpawn = 0.0f;
								SpawnNewObject ();
						}
						//check and reset scene objects
						for (int i = 0; i < spawnedList.Count; i++) {
								if (spawnedList [i].activeSelf) {
										spawnedList [i].transform.position = new Vector3 (spawnedList [i].transform.position.x, spawnedList [i].transform.position.y, spawnedList [i].transform.position.z + speed * Time.deltaTime);

										if (spawnedList [i].transform.position.z <= ResetPoint.transform.position.z) 
										{
												spawnedList [i].transform.position = SpawnPoint.transform.position;
												spawnedList [i].SetActive (false);
										}
								}
						}	
				}
	}
				
	public void SpawnNewObject()
	{
				bool foundSceneObject= false;

				foreach (GameObject sceneObject in spawnedList) 
				{
						if (!sceneObject.activeSelf) 
						{
								foundSceneObject = true;
								sceneObject.gameObject.transform.position = SpawnPoint.transform.position;
								sceneObject.GetComponent<SceneObject> ().Spawn ();
								sceneObject.SetActive (true);
								return;
						}
				}

				//spawns a brand new character if there aren't enough in the character list 
				if (spawnedList.Count < CrotchDogConstants.MAX_SCENE_OBJECTS) 
				{	
						GameObject sceneObject = ((GameObject)Instantiate( Resources.Load ("House")));

						sceneObject.gameObject.transform.position = SpawnPoint.transform.position;
						sceneObject.gameObject.transform.localScale = new Vector3 (sceneObject.gameObject.transform.localScale.x*changeScale.x, sceneObject.gameObject.transform.localScale.y*changeScale.y, sceneObject.gameObject.transform.localScale.z * changeScale.z);
						//character.gameObject.transform.parent
						sceneObject.gameObject.transform.parent = this.gameObject.transform;

						spawnedList.Add (sceneObject);
				}
		
	}


	public void SetSpeed(float newSpeed)
	{
			speed = newSpeed;
	}
}
