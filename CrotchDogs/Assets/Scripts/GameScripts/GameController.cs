using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AceViral;

public class GameController : MonoBehaviour {

	public enum GameState
	{
		START_MENU,PLAYING_GAME,GAME_OVER,PAUSED,LOADING,RESTART
	}
   /*
	* POWER UPS
	*/
	public enum PowerUp
	{
			NONE,MAUL_MANIAC
	}
	public PowerUp ActivePower;
	public float powerUpTime=0.0f;

	public ChaserController chaseController;
	public SpawnController spawnController;
	public GameOverUI gameOverUI;
	public GameObject PauseMenu;

	//score information
	public int combo=0, crotchesBitten=0, bitestaken=0,charactersEscaped=0,bitesMissed=0,m_FirstTouchIndex,crotchesMauled =0,bestCombo=0;
	public float survivalTime = 0;

	//the lanes characters spawn in
	public List<Lane> lanes;
	//spawns houses at hte side of lanes
	public List<SceneObjectSpawner> objectSpawner;
	
	public int NumOfLanes = 1;
	public Dog dog;
	public Face face;
	public MovingText InfoLabel01,InfoLabel02;
	public int totalScore=0;

	public float spawnCharacterInterval = 3.0f;
	private float changeSceneTime=0.0f;
	
	public GameObject LoadingScene,MainMenuScene,GameScene,GameOverScene;
	public List<int> crotchesBittenFaceList,crotchesEscapedLegList; 
	List<Collider> CrotchBitten;
	public GameState state;

	public float speed;

	// =-=-=-=-=-=-=-=
	// Singleton
	// =-=-=-=-=-=-=-=
	public static GameController Instance;

	public MyBezier chaseRadar;

	// =-=-=-=-=-=-=-=-
	// Score Label
	// =-=-=-=-=-=-=-=-
	public tk2dTextMesh ScoreLabel;
	private int showingScoreValue=0, newScoreValue=0;

	public void Initialize()
	{
		for(int i = 0; i < NumOfLanes;i++)
		{

		}
	}

	void Awake()
	{
		if (Instance == null) 
		{	
			Instance = this;
			crotchesBittenFaceList = new List<int> ();
			crotchesEscapedLegList = new List<int> ();

			if (chaseController == null) 
			{
					chaseController = new ChaserController ();
			}
		}
		ScoreLabel.text = "" + showingScoreValue;

		changeState (GameState.LOADING);
	}

	// Use this for initialization
	void Start () 
	{
		if (Instance == null) 
		{
				Instance = this;
				if (chaseController == null) 
				{
						chaseController = new ChaserController ();
				}
		}
	}
	void Update () 
	{
		switch (state) 
		{
			case GameState.START_MENU: 
				break;
			case GameState.PLAYING_GAME:
				GameUpdate (); 
				UpdateScore ();
				break;
			case GameState.PAUSED: 
				break;
				case GameState.GAME_OVER: 
				GameOverUpdate ();
				break;
			case GameState.LOADING: 
				LoadingUpdate ();
				break;
		}

			
	}
		void UpdateScore()
		{
				int newTotalScore = bitesMissed * CrotchDogConstants.SCORE_MISSES +
									  charactersEscaped * CrotchDogConstants.SCORE_ESCAPED +
									  crotchesBitten * CrotchDogConstants.SCORE_BITE +
									  crotchesMauled * CrotchDogConstants.SCORE_MAUL +
									  bestCombo * CrotchDogConstants.SCORE_COMBO;


				if (totalScore != newTotalScore) 
				{
						totalScore = newTotalScore;
						newScoreValue = totalScore;
				}

				if (newScoreValue != showingScoreValue) 
				{
						if (newScoreValue > showingScoreValue) 
						{
								showingScoreValue++;
						}
						else 
						{
								showingScoreValue--;
						}

						ScoreLabel.text = ""+showingScoreValue;
				}

		}

		//Loading Udate
		void LoadingUpdate()
		{
				changeSceneTime += Time.deltaTime;

				if (changeSceneTime >= CrotchDogConstants.CHANGE_SCENE_TIME) 
				{
						changeState (GameState.START_MENU);
				}
		}
		void GameOverUpdate()
		{
				//checks if a button was pressed and should skip
				if(Input.GetMouseButtonDown(0))
				{

						gameOverUI.skipAnimations ();

				}

		}
		// main game Update is called once per frame when in gameMode
		void GameUpdate () 
		{

				spawnCharacter ();

				//update Power Up
				if (ActivePower != PowerUp.NONE) 
				{
						powerUpTime += Time.deltaTime;

						if (powerUpTime >= CrotchDogConstants.POWER_UP_LASTS) 
						{
								ActivatePowerUp (PowerUp.NONE);
						}
				}



				//check for a touch input
				#if UNITY_EDITOR || UNITY_EDITOR_OSX

				if(Input.GetMouseButtonDown(0))
				{
						biteCrotch();
				}

				#else




		
				if (Input.touchCount > 0) 
				{
					m_FirstTouchIndex = Input.GetTouch(0).fingerId;

					foreach (Touch touchData in Input.touches) 
					{
						// Don't care about multi-touch. Just handle first finger
						if (touchData.fingerId == m_FirstTouchIndex) 
						{
								//Debug.Log(" crotch Bite with began touch!!! -=-=-=-=-=-= " + Input.touchCount  + " Phase " + touchData.phase);
								if(touchData.phase == TouchPhase.Began)
								{
									biteCrotch();
								}
						}
					}
				}

				#endif


			
//				if (distanceToNearestCrotch <= CrotchDogConstants.CROTCH_MAX_DISTANCE) 
//				{
//						float newJawsScale = distanceToNearestCrotch / CrotchDogConstants.CROTCH_MAX_DISTANCE;
//						//Debug.Log ("nearest crotch: " + distanceToNearestCrotch + " new scale " + newJawsScale);
//
//						dog.Jaws.transform.localScale = new Vector3 (newJawsScale,newJawsScale,1.0f);
//
//				}
//				else if(dog.Jaws.transform.localScale.x != 1.0f)
//				{
//						float newScale = dog.Jaws.transform.localScale.x + Time.deltaTime;
//
//						if (newScale > 1.0f) 
//						{
//								newScale = 1.0f;
//						}
//						dog.Jaws.transform.localScale = new Vector3 (newScale,newScale,1.0f);
//				}
		}

		// called when user touches or presses mouse button
		public void biteCrotch()
		{
				float bite = CrotchDogConstants.NO_BITE;

				bite  =  dog.Bite();
				CrotchBitten = dog.getOtherCollider();


				if(CrotchBitten.Count == 0)
				{
						missedCrotch();
				}

				//press bite checks and finds which crotch was bitten
				if (bite != CrotchDogConstants.NO_BITE && CrotchBitten != null) 
				{
						//Debug.Log(@"Bite Crotch");
						for (int laneIndex = 0; laneIndex < lanes.Count; laneIndex++) 
						{
								for (int charIndex = 0; charIndex < lanes [laneIndex].Characters.Count; charIndex++) 
								{
										//Debug.Log ("index " + laneIndex);
										//Debug.Log ("character try bite " + lanes [laneIndex].Characters [charIndex].getCollider());
										if (lanes [laneIndex].Characters [charIndex].isShowing) 
										{

												List<Collider> removeCrotch = new List<Collider>();

												foreach (Collider crotch in CrotchBitten) {
														if (lanes [laneIndex].Characters [charIndex].getCollider () == crotch) {

																crotchesBittenFaceList.Add (lanes [laneIndex].Characters [charIndex].GetComponent<Character> ().headRandom);
																if (!lanes [laneIndex].Characters [charIndex].GetComponent<Character> ().crotchBitten) {
																		biteCrotch (bite);
																		lanes [laneIndex].Characters [charIndex].GetComponent<Character> ().setCrotchBitten (true);
																		//Activate PowerUp
																		if (lanes [laneIndex].Characters [charIndex].GetComponent<Character> ().hasPowerUp)
																		{
																				ActivatePowerUp (PowerUp.MAUL_MANIAC);
																		}
																}
																removeCrotch.Add (crotch);

														}
												}

												foreach (Collider crotch in removeCrotch) 
												{
														CrotchBitten.Remove (crotch);
												}
										}
								}
						}
				}


				float distanceToNearestCrotch= CrotchDogConstants.CROTCH_MAX_DISTANCE;

				foreach (Lane lane in lanes) 
				{
						foreach (Character character in lane.Characters) 
						{
								if (character != null) 
								{
										float diff = character.gameObject.transform.position.z - dog.getCollider().transform.position.z;

										diff = -dog.DifferenceToMidpointOfLine (dog.getCollider ().transform.position, dog.EndPoint.transform.position, character.getCollider ().transform.position);

										if (distanceToNearestCrotch > Mathf.Abs(diff) ) 
										{
												distanceToNearestCrotch = Mathf.Abs(diff);
												//Debug.Log ("distance  " + distanceToNearestCrotch);
										}
								}
						}

				}

		}


		void ActivatePowerUp(PowerUp newPower)
		{


				ActivePower = newPower;
				switch(newPower)
				{
					case PowerUp.NONE:
						powerUpTime = 0;
						setSpeedOnLanes (CrotchDogConstants.CHARACTER_SPEED);
						setSpeedOnObjects (CrotchDogConstants.CHARACTER_SPEED);
						break;
					case PowerUp.MAUL_MANIAC:
						powerUpTime = 0;
						setSpeedOnLanes (CrotchDogConstants.CHARACTER_POWER_UP_SPEED);
						setSpeedOnObjects (CrotchDogConstants.CHARACTER_POWER_UP_SPEED);
						break;
					default: break;

				}
		}


		void setSpeedOnLanes(float newSpeed)
		{
				foreach (Lane lane in lanes) 
				{
					lane.SetSpeed (newSpeed);
				}
		}

		void setSpeedOnObjects(float newSpeed)
		{
				foreach (SceneObjectSpawner spawner in objectSpawner) 
				{
						spawner.SetSpeed (newSpeed);
				}
		}
	void changeState(GameState newState)
	{
		GameState oldState = state;
		//Debug.Log ("new state  " + newState + " old state " + state );
		// leave the old state
		switch (state) 
		{
			case GameState.START_MENU: 
					LeaveMainMenu (newState);
			break;
			case GameState.PLAYING_GAME:
				if (newState != GameState.PAUSED) 
				{
					LeaveGame (newState); 
				}
			break;
				case GameState.RESTART:
		
				break;
			case GameState.PAUSED: 
					Unpause (newState);
			break;
			case GameState.GAME_OVER:
					LeaveGameOver (newState);
			break;
			case GameState.LOADING: 
					LeaveLoading(newState);
			break;
		}


				state = newState;
	
		// get ready for the new state
		switch (newState) 
		{
				case GameState.START_MENU: 
					goToMainMenu ();
				break;
				case GameState.PLAYING_GAME:
					AdController.Instance.HideBanner ();
					Debug.Log ("play game " + state);
					if (oldState != GameState.PAUSED) 
					{
						Debug.Log ("play game ");
						goToGame (); 
					}
				break;
				case GameState.RESTART:
					Debug.Log ("restart ");
					changeState (GameState.PLAYING_GAME);
					break;
				case GameState.PAUSED: 
					pause ();
				break;
				case GameState.GAME_OVER:
					goToGameOver ();
				break;
				case GameState.LOADING: 
					goToLoading();
				break;
		}
			


	}

	void LeaveMainMenu(GameState newState)
	{
		MainMenuScene.SetActive (false);
	}


	void LeaveGame(GameState newState)
	{
		dog.gameObject.SetActive (false);
		GameScene.SetActive (false);
	}

	void LeaveLoading(GameState newState)
	{
		LoadingScene.SetActive (false);
	}

	void LeaveGameOver(GameState newState)
	{
		GameOverScene.SetActive  (false);
	}

	void Unpause(GameState newState)
	{
		PauseMenu.SetActive (false);
	}

	void goToLoading()
	{
		LoadingScene.SetActive (true);
		GameOverScene.SetActive (false);
		GameScene.SetActive (false);
		dog.gameObject.SetActive (false);
		PauseMenu.SetActive (false);
		MainMenuScene.SetActive (false);
	}

	void goToMainMenu()
	{
		AdController.Instance.ShowBanner ();
		dog.gameObject.SetActive (false);
		GameScene.SetActive (false);
		MainMenuScene.SetActive (true);
	}

	void goToGame()
	{
		GameScene.SetActive (true);
		dog.gameObject.SetActive (true);
		resetGame ();
	}

	void pause()
	{
		PauseMenu.SetActive (true);
	}

	void goToGameOver()
	{

		AdController.Instance.ShowBanner ();
		GameOverScene.SetActive (true);
		setGameOverInfo ();
		
	}

		public void spawnCharacter()
		{
			if (spawnController.UpdateToSpawn(Time.deltaTime)) 
			{
				spawnInLane (0);		
			}
		}

		public void spawnInLane(int laneNum)
		{
			lanes [0].SpawnCharacter ();
		}

		public void StartGame()
		{
			changeState(GameState.PLAYING_GAME);
		}

		public void setGameOverInfo()
		{
			gameOverUI.GameOver ();
		}

		public void Replay()
		{
			changeState(GameState.START_MENU);
		}

		public void Share()
		{
				Debug.Log ("Logged in? " + AVFacebook.Instance.IsLoggedIn ());
			if (AVFacebook.Instance.IsLoggedIn ()) 
			{

						//AVFacebook.Instance.OpenPostDialog ("Crotch Dogs", "I've bitten " + crotchesBitten + " crotches and Mauled " + crotchesMauled + " yummy!");

						AVFacebook.Instance.Post ("I just bit some Crotches", "I've bitten " + crotchesBitten + " crotches and Mauled " + crotchesMauled + " yummy!", "");
			}
			else 
			{

						AVFacebook.Instance.Login ();
			}
		}

		public void resetGame()
		{
				bestCombo = 0;
				crotchesBitten = 0;
				crotchesMauled = 0;

				combo = 0;
				bitesMissed = 0;
				totalScore = 0;
				charactersEscaped = 0;
				bitestaken = 0;

				chaseController.reset ();
				chaseRadar.reset ();
				spawnController.reset ();

				//reset the crotches bitten List
				crotchesBittenFaceList = new List<int> ();
				crotchesEscapedLegList = new List<int> ();

				foreach (Lane lane in lanes) 
				{
					lane.reset ();
				}

				//InfoLabel01.showFlavourText(MovingText.InfoType.READY);


				//reset score label
				newScoreValue = 0;
				showingScoreValue = 0;

				ScoreLabel.text = "" + showingScoreValue;

		}



		public void characterEscaped ()
		{
				//Debug.Log ("character escaped");

				combo = 0;
				charactersEscaped++;
				chaseController.Escaped ();

		}

		//bite the crotch that has been successfully bitten
		void biteCrotch(float distanceOnCrotch)
		{
				bitestaken++;
				combo++;

				if (combo > bestCombo) 
				{
						bestCombo = combo;
				}

				if(Mathf.Abs(distanceOnCrotch) < CrotchDogConstants.MAUL_DISTANCE || ActivePower == PowerUp.MAUL_MANIAC)
				{
						SoundManager.PlayBite ();

						Debug.Log ("maul crotch " + distanceOnCrotch);
			
						crotchesMauled++;
						face.setFacetoState ( Face.stateOfFaces.maul);
			
						if (!InfoLabel01.onScreen()) 
						{	
							InfoLabel01.showFlavourText (MovingText.InfoType.MAUL);
							InfoLabel02.hide ();
						}
						else if (!InfoLabel02.onScreen()) 
						{
							InfoLabel01.hide ();
							InfoLabel02.showFlavourText (MovingText.InfoType.MAUL);
						}
						chaseController.Maul ();
				}
				else
				{
						SoundManager.PlayBite ();

						Debug.Log ("bite crotch " + distanceOnCrotch);
						crotchesBitten++;
						face.setFacetoState ( Face.stateOfFaces.bite);

						if (!InfoLabel01.onScreen()) {
								InfoLabel02.hide ();
								InfoLabel01.showFlavourText (MovingText.InfoType.BITE);
						}
						else if (!InfoLabel02.onScreen()) {
								InfoLabel01.hide ();
								InfoLabel02.showFlavourText (MovingText.InfoType.BITE);
						}


						chaseController.Bite ();
				}

		}

		void missedCrotch()
		{
				chaseController.Miss ();
				bitestaken++;
				bitesMissed++;
				face.setFacetoState ( Face.stateOfFaces.miss);
				if (!InfoLabel01.onScreen()) {
						InfoLabel02.hide ();
						InfoLabel01.showFlavourText (MovingText.InfoType.MISS);
				}
				else if (!InfoLabel02.onScreen()) {
						InfoLabel01.hide ();
						InfoLabel02.showFlavourText (MovingText.InfoType.MISS);
				}
		}


		public void callGameOver()
		{
			changeState (GameState.GAME_OVER);
		}

		public GameState getState()
		{
				return state;
		}

		//called From pause menu
		public void PauseGame()
		{

				if (state == GameState.PAUSED) 
				{
						changeState (GameState.PLAYING_GAME);
				} else {
						changeState (GameState.PAUSED);
				}
		}

		public void Exit()
		{
				changeState (GameState.START_MENU);
		}

		public void Restart()
		{

				changeState (GameState.RESTART);
		}

		public void MuteSFX()
		{
				SoundManager.Instance.SFX_Mute = !SoundManager.Instance.SFX_Mute;
		}

		public void MuteMusic()
		{
				SoundManager.Instance.Mute = !SoundManager.Instance.Mute;
		}

		public void saveHighScores()
		{

		}

}
