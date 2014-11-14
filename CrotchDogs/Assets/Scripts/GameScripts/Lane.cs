using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lane : MonoBehaviour {
	
		public GameObject spawnPoint,resetPoint;
		public GameObject Ground;
		public float characterSpeed = CrotchDogConstants.CHARACTER_SPEED;
		public List<Character> Characters;

		public const float TIME_STEP = 0.016666667f;
		public void Awake()
		{
			if (Characters == null) 
			{
					Characters = new List<Character> ();
			}
		}


		public void Update()
		{

				if (GameController.Instance.getState() == GameController.GameState.PLAYING_GAME) {
						foreach (Character character in Characters) {
								if (character.isShowing) {
										if (character.gameObject.transform.position.z > resetPoint.transform.position.z) {
												character.gameObject.transform.position = new Vector3 (character.gameObject.transform.position.x, character.gameObject.transform.position.y, character.gameObject.transform.position.z + character.speed * TIME_STEP);
										} else {
												if (!character.crotchBitten) {
														GameController.Instance.crotchesEscapedLegList.Add (character.legsRandom);
														GameController.Instance.characterEscaped ();
												}
												character.resetCharacter ();
												character.gameObject.transform.position = spawnPoint.transform.position;
										}

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
				if (Characters.Count < CrotchDogConstants.MAX_CHARACTERS) 
				{	
						Character character = ((GameObject)Instantiate( Resources.Load ("Character"))).GetComponent<Character>();
						Debug.Log ("add new character in lane " + character);
						character.resetCharacter();
						character.gameObject.transform.position = spawnPoint.transform.position;
						character.spawn();
						character.SetSpeed (characterSpeed);
						//character.gameObject.transform.parent
						character.gameObject.transform.parent = this.gameObject.transform;
					
						Characters.Add (character);
				}
		}
		//reset is called on a new game
		public void reset()
		{
				foreach(Character character in Characters)
				{
						if(character.isShowing)
						{
								character.resetCharacter();
								character.gameObject.transform.position = spawnPoint.transform.position;
						}
				}

		}

		public void increaseSpeed(float newSpeed)
		{
				characterSpeed = newSpeed;
				foreach(Character character in Characters)
				{
						character.SetSpeed (characterSpeed);
				}
		}
}
