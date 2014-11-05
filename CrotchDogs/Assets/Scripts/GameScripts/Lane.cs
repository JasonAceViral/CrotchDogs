using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lane : MonoBehaviour {
	
		public GameObject spawnPoint,resetPoint;
		public GameObject Ground;

		public List<Character> Characters;
		public const int MAX_CHARACTERS =10;

		public void Awake()
		{
			if (Characters == null) 
			{
					Characters = new List<Character> ();
			}
		}


		public void Update()
		{
			foreach (Character character in Characters) 
			{
						if (character.isShowing) {
								if (character.gameObject.transform.position.z > resetPoint.transform.position.z) {
										character.gameObject.transform.position = new Vector3 (character.gameObject.transform.position.x, character.gameObject.transform.position.y, character.gameObject.transform.position.z + character.speed * Time.deltaTime);
								} else {
										if (!character.crotchBitten) {
											
												GameController.Instance.characterEscaped ();
										}
										character.resetCharacter ();
										character.gameObject.transform.position = spawnPoint.transform.position;
								}

						}
			}
		}

		public void startACharacter()
		{
				foreach(Character character in Characters)
				{
						if(!character.isShowing)
						{
								character.resetCharacter();
								character.gameObject.transform.position = spawnPoint.transform.position;
								character.spawn();
								return;
						}
				}

		}

		public void SpawnCharacter()
		{
	
				foreach(Character character in Characters)
				{
						if(!character.isShowing)
						{
								character.resetCharacter();
								character.gameObject.transform.position = spawnPoint.transform.position;
								character.spawn();
								return;
						}
				}

				//spawns a brand new character if there aren't enough in the character list 
				if (Characters.Count < MAX_CHARACTERS) 
				{	
						Character character = ((GameObject)Instantiate( Resources.Load ("Character"))).GetComponent<Character>();
						Debug.Log ("add new character in lane " + character);
						character.resetCharacter();
						character.gameObject.transform.position = spawnPoint.transform.position;
						character.spawn();
						//character.gameObject.transform.parent
						character.gameObject.transform.parent = this.gameObject.transform;
					
						Characters.Add (character);
				}
		}
}
