using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	enum GameState
	{
		START_MENU,PLAYING_GAME,GAME_OVER,PAUSED,LOADING
	}
				
	public int combo=0, crotchesBitten=0, bitestaken=0,charactersEscaped=0,bitesMissed=0,m_FirstTouchIndex;
	public List<Lane> lanes;
	public int NumOfLanes = 1;
	public Dog dog;
	public Face face;
	public MovingText InfoLabel;
	public int totalScore=0;
	private float spawnCharacterInterval= 3.0f,spawnTime=0.0f,changeSceneTime=0.0f;
	
	public GameObject LoadingScene,MainMenuScene,GameScene,GameOverScene;

	Collider CrotchBitten;
	GameState state;

	public float speed;
	// =-=-=-=-=-=-=-=
	// Game Over Labels
	// =-=-=-=-=-=-=-=
	public tk2dTextMesh CrotchesBittenLabel, ComboScoreLabel, MissesLabel, TotalScoreLabel;


	// =-=-=-=-=-=-=-=
	// Singleton
	// =-=-=-=-=-=-=-=
	public static GameController Instance;

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
		}
		changeState (GameState.LOADING);
	}

	// Use this for initialization
	void Start () 
	{
		if (Instance == null) 
		{
				Instance = this;
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
				break;
			case GameState.PAUSED: 
				break;
			case GameState.GAME_OVER: 
				break;
			case GameState.LOADING: 
				LoadingUpdate ();
				break;
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

		// main game Update is called once per frame when in gameMode
		void GameUpdate () 
		{
				float bite = -1;
				spawnCharacter ();
				//check for a touch input
				#if UNITY_EDITOR || UNITY_EDITOR_OSX

				if(Input.GetMouseButtonDown(0))
				{
						bitestaken++;
						bite  =  dog.Bite();
						CrotchBitten = dog.getOtherCollider();


						if(CrotchBitten == null)
						{
								missedCrotch();
								bitesMissed++;
						}

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
										Debug.Log(" crotch Bite with began touch!!! -=-=-=-=-=-= " + Input.touchCount  + " Phase " + touchData.phase);
								if(touchData.phase == TouchPhase.Began)
								{
									
									bitestaken++;
									bite  =  dog.Bite();
									CrotchBitten = dog.getOtherCollider();


									if(CrotchBitten == null)
									{
											missedCrotch();
											bitesMissed++;
									}
								}
						}
					}
				}

				#endif


				//press bite checks and finds which crotch was bitten
				if (bite != CrotchDogConstants.NO_BITE && CrotchBitten != null) 
				{
						Debug.Log(@"Bite Crotch");
						for (int laneIndex = 0; laneIndex < lanes.Count; laneIndex++) 
						{
								for (int charIndex = 0; charIndex < lanes [laneIndex].Characters.Count; charIndex++) 
								{
										Debug.Log ("index " + laneIndex);
										Debug.Log ("character try bite " + lanes [laneIndex].Characters [charIndex].getCollider());
										if (lanes [laneIndex].Characters [charIndex].isShowing) {
												if (lanes [laneIndex].Characters [charIndex].getCollider() == CrotchBitten) {
														lanes [laneIndex].Characters [charIndex].GetComponent<Character> ().setCrotchBitten (true);
														biteCrotch (bite);
														CrotchBitten = null;
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
										float diff = character.gameObject.transform.position.z - dog.gameObject.transform.position.z;

										if (distanceToNearestCrotch > diff && diff >= 0.0f) {
												distanceToNearestCrotch = diff;
										}
								}
						}

				}

				if (distanceToNearestCrotch <= CrotchDogConstants.CROTCH_MAX_DISTANCE) 
				{
						float newJawsScale = distanceToNearestCrotch / CrotchDogConstants.CROTCH_MAX_DISTANCE;
						//Debug.Log ("nearest crotch: " + distanceToNearestCrotch + " new scale " + newJawsScale);

						dog.Jaws.transform.localScale = new Vector3 (newJawsScale,newJawsScale,1.0f);

				}
				else if(dog.Jaws.transform.localScale.x != 1.0f)
				{
						float newScale = dog.Jaws.transform.localScale.x + Time.deltaTime;

						if (newScale > 1.0f) 
						{
								newScale = 1.0f;
						}
						dog.Jaws.transform.localScale = new Vector3 (newScale,newScale,1.0f);
				}

		}


	void changeState(GameState newState)
	{
		
		// leave the old state
		switch (state) 
		{
			case GameState.START_MENU: 
					LeaveMainMenu (newState);
			break;
			case GameState.PLAYING_GAME:
					LeaveGame (newState); 
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

		// get ready for the new state
		switch (newState) 
		{
				case GameState.START_MENU: 
					goToMainMenu ();
				break;
				case GameState.PLAYING_GAME:
					goToGame (); 
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

		state = newState;

	}

	void LeaveMainMenu(GameState newState)
	{
		MainMenuScene.SetActive (false);
	}


	void LeaveGame(GameState newState)
	{
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

	}

	void goToLoading()
	{
		LoadingScene.SetActive (true);
	}

	void goToMainMenu()
	{
		MainMenuScene.SetActive (true);
	}

	void goToGame()
	{
		GameScene.SetActive (true);
		resetGame ();
	}

	void pause()
	{

	}

	void goToGameOver()
	{
		GameOverScene.SetActive (true);
		setGameOverInfo ();
	}

	


	//bite the crotch that has been successfully bitten
	void biteCrotch(float distanceOnCrotch)
	{
		Debug.Log ("bite crotch " + distanceOnCrotch);
		combo++;

		if(Mathf.Abs(distanceOnCrotch) < CrotchDogConstants.MAUL_DISTANCE)
		{
			crotchesBitten++;
			face.setFacetoState ( Face.stateOfFaces.maul);
			InfoLabel.showFlavourText (MovingText.InfoType.MAUL);
		}
		else
		{
			crotchesBitten++;
			face.setFacetoState ( Face.stateOfFaces.bite);
			InfoLabel.showFlavourText (MovingText.InfoType.BITE);
		}

	}

	void missedCrotch()
	{
		face.setFacetoState ( Face.stateOfFaces.miss);
		InfoLabel.showFlavourText (MovingText.InfoType.MISS);
	}

	public void characterEscaped ()
	{
		Debug.Log ("character escaped");

		combo = 0;
		charactersEscaped++;

		if (charactersEscaped > CrotchDogConstants.NUM_OF_ESCAPES) 
		{
			changeState (GameState.GAME_OVER);
		}
	}

	public void spawnCharacter()
	{
			spawnTime += Time.deltaTime;

			if (spawnTime >= spawnCharacterInterval) 
			{
				Debug.Log ("spawn character");
				spawnInLane (0);
				spawnTime = 0.0f;
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
			CrotchesBittenLabel.text = "Crotches Bitten: " + crotchesBitten;
			ComboScoreLabel.text = "Combo Score: " + combo;
			MissesLabel.text = "Misses: " + bitesMissed;
			TotalScoreLabel.text = "Total Score: " + totalScore;
		}

		public void Replay()
		{
				changeState(GameState.START_MENU);
		}

		public void Share()
		{

		}

		public void resetGame()
		{
				crotchesBitten = 0;
				combo = 0;
				bitesMissed = 0;
				totalScore = 0;
				charactersEscaped = 0;
		}

}
