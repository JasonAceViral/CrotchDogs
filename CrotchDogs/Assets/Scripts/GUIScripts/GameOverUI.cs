using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	//=-=-=-=-=-=-=
	// spawn the faces for the victims
	//=-=-=-=-=-=-=
	public GameObject ShowFacePos,SpawnFaceLocation,ResetFaceLocation;
	public GameObject ShowLegsPos,SpawnLegLocation,ResetLegLocation;
	public tk2dSprite face01,Legs01,crotchNemesis;
	bool movingFace01=false,movingLegs01=false,showingNemesis=false;

	public int showVictimIndex=0,showEscapedIndex;
	bool showVictims = false,skip=false,updatedTotalScore = false,showingSlander= false;
	private const float SHOW_ALL_TIME_LIMIT = 5.0f;

	public int TotalScore =0;

	public float move_speed,totalScoreUpdateTime= 0.0f;
	// =-=-=-=-=-=-=-=
	// Game Over Labels
	// =-=-=-=-=-=-=-=
	public tk2dTextMesh CrotchesBittenLabel, ComboScoreLabel, MissesLabel,CharactersEscapedLabel ,TotalScoreLabel, SlanderLabel;


	private string[] minusSlander = {"That the best you can do?",
									"Why don't you try to get more than 0",
									"Didn't even know we had a minus score" };
	private string[] zeroSlander = { "Zero!,could have atleast tried to score points" };
	private string[] okSlander = {   "They might have felt one of those bites",
				                    "Your score isn't even 3 digits!" };
	private string[] GoodSlander = { "Wow I think one of them are limping", 
				                    "Your a ravenous dog feared by crotches" };


	// Use this for initialization
	void Awake () {
				//gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{

				if (!skip) {
						//if (showVictims) 
						{
								if (movingFace01) 
								{
										UpdateFacesOfTheVictims ();
								} else if (movingLegs01) {	
										UpdateCrotchesMissed ();
								} else if (!showingNemesis) {
										UpdateNemesis ();
								} else if (!updatedTotalScore) {
										UpdateTotalScore ();
								}
								else if (!showingSlander) {
										UpdateSlanderMessage ();
								}
						}


				} else 
				{
						//if (showVictims) 
						{
								if (movingFace01)
								{
										UpdateFacesOfTheVictims ();
								} 

								if (movingLegs01)
								{	
										UpdateCrotchesMissed ();
								} 

								if (!showingNemesis) 
								{
										UpdateNemesis ();
								}
								if (!updatedTotalScore) 
								{
										UpdateTotalScore ();
								}
								if (!showingSlander) {
										UpdateSlanderMessage ();
								}
						}

				}
	}
	//called from gameController when the game ends
	public void GameOver()
	{	
			//sets the total Score
			GameController.Instance.totalScore = GameController.Instance.bitesMissed * CrotchDogConstants.SCORE_MISSES +
			GameController.Instance.charactersEscaped * CrotchDogConstants.SCORE_ESCAPED +
			GameController.Instance.crotchesBitten * CrotchDogConstants.SCORE_BITE +
			GameController.Instance.crotchesMauled * CrotchDogConstants.SCORE_MAUL +
			GameController.Instance.bestCombo* CrotchDogConstants.SCORE_COMBO;


			setSlanderMessage ();

			setGameOverInfo ();

	}

	//sets all labels
	public void setGameOverInfo()
	{
		
		ComboScoreLabel.text = "Best Combo: " + GameController.Instance.bestCombo;
		MissesLabel.text = "Misses: " + GameController.Instance.bitesMissed;
		CrotchesBittenLabel.text = "Crotches Bitten: " + 0;
		CharactersEscapedLabel.text = "Escaped: " + 0;
		TotalScoreLabel.text = "Total Score: " + 0;
		face01.transform.position = SpawnFaceLocation.transform.position;
		Legs01.transform.position = SpawnLegLocation.transform.position;
		showVictims = true;


		if (GameController.Instance.crotchesEscapedLegList.Count == 0) {
				movingLegs01 = false;

				//turn off crotch nemesis is no crotches escaped
				showingNemesis = true;
				crotchNemesis.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		} else {
				movingLegs01 = true;

				// if no legs escaped can't have a nemesis crotch
				setNemesisCrotch ();
		}

		if (GameController.Instance.crotchesBittenFaceList.Count == 0) {
				movingFace01 = false;
		} else {
				movingFace01 = true;
		}
	

		updatedTotalScore = false;
		showingSlander = false;
		totalScoreUpdateTime = 0.0f;
		
		SlanderLabel.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

	}

	public void reset()
	{
			showVictims = false;
			showVictimIndex = 0;
			movingFace01 = false;
			movingLegs01=false;
			skip = false;
	}
		//updates the victims
	public void UpdateFacesOfTheVictims()
	{
				float deltaTime = Time.deltaTime;
				//moves sprite by speed
				move_speed = -15.0f * (float)((float)GameController.Instance.crotchesBittenFaceList.Count/(float)(showVictimIndex+1))*deltaTime;
			
				if (movingFace01 ) 
				{
						if (face01.transform.position.y > ShowFacePos.transform.position.y) // move face to show popsition
						{
								if ((face01.gameObject.transform.position.y + move_speed) < ResetFaceLocation.transform.position.y) {
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ResetFaceLocation.transform.position.y,	face01.gameObject.transform.position.z);
								} else {
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	face01.gameObject.transform.position.y + move_speed,	face01.gameObject.transform.position.z);
								}
						}
						else if (face01.transform.position.y > ResetFaceLocation.transform.position.y && showVictimIndex < GameController.Instance.crotchesBittenFaceList.Count-1) //move face to reset position
						{
								if((face01.gameObject.transform.position.y +  move_speed) < ResetFaceLocation.transform.position.y)
								{
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ResetFaceLocation.transform.position.y,	face01.gameObject.transform.position.z);
								}
								else
								{
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	face01.gameObject.transform.position.y +  move_speed,	face01.gameObject.transform.position.z);
								}
						}
						else  // reset face
						{
								//movingFace01 = false;
								showVictimIndex++;
								SoundManager.PlayBite();
								CrotchesBittenLabel.text = "Crotches Bitten: " + showVictimIndex;
								//if ran out of victims to show
								if (showVictimIndex >= GameController.Instance.crotchesBittenFaceList.Count) {
										showVictimIndex = 0;
										movingFace01 = false;
										face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ShowFacePos.transform.position.y,	face01.gameObject.transform.position.z);

								}
								else// move face back to reset position so it moves down with new face
								{
										face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	SpawnFaceLocation.transform.position.y,	face01.gameObject.transform.position.z);
										face01.SetSprite (Character.HEAD_SPRITE + GameController.Instance.crotchesBittenFaceList[showVictimIndex] );
								}
						}
				}


	}

		//Updates the crotches that escaped
		public void UpdateCrotchesMissed()
		{

				float deltaTime = Time.deltaTime;
				//moves sprite by speed
				move_speed = -10.0f * (float)((float)GameController.Instance.crotchesEscapedLegList.Count/(float)(showEscapedIndex+1))*deltaTime;


				if (movingLegs01 ) 
				{
						if (Legs01.transform.position.y > ShowLegsPos.transform.position.y) // move face to show popsition
						{
								Legs01.gameObject.transform.position = new Vector3 (Legs01.gameObject.transform.position.x,	Legs01.gameObject.transform.position.y +  move_speed,	Legs01.gameObject.transform.position.z);
						}
						else if (Legs01.transform.position.y > ResetLegLocation.transform.position.y && showEscapedIndex < GameController.Instance.crotchesEscapedLegList.Count-1) //move face to reset position
						{
								Legs01.gameObject.transform.position = new Vector3 (Legs01.gameObject.transform.position.x,	Legs01.gameObject.transform.position.y +  move_speed,	Legs01.gameObject.transform.position.z);

						}
						else  // reset legs 
						{
								showEscapedIndex++;
								SoundManager.PlaySwoosh();
								CharactersEscapedLabel.text = "Crotches Escaped: " + showEscapedIndex;
								//if ran out of victims to show
								if (showEscapedIndex >= GameController.Instance.crotchesEscapedLegList.Count) 
								{
										showEscapedIndex = 0;
										movingLegs01 = false;
										showVictims = false;
										Legs01.gameObject.transform.position = new Vector3 (Legs01.gameObject.transform.position.x,	ShowLegsPos.transform.position.y,	Legs01.gameObject.transform.position.z);
								}
								else// move face back to reset position so it moves down with new face
								{
										Legs01.gameObject.transform.position = new Vector3 (Legs01.gameObject.transform.position.x,	SpawnLegLocation.transform.position.y,	Legs01.gameObject.transform.position.z);
										Legs01.SetSprite (Character.LEGS_SPRITE + GameController.Instance.crotchesEscapedLegList[showEscapedIndex] );
								}
						}
				}
		}

		//updates the nemesis
		void UpdateNemesis()
		{
				float opacity = crotchNemesis.color.a;
				Debug.Log ("nemisis " + opacity);
				if (opacity < 1.0f) 
				{
						opacity += Time.deltaTime;
						crotchNemesis.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				} 
				else 
				{
						crotchNemesis.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
						showingNemesis = true;
				}


		}

		void setNemesisCrotch()
		{
				showingNemesis = false;
				crotchNemesis.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

				int crotchType1 = 0, crotchType2 = 0, crotchType3 = 0, crotchType4 = 0;
				 
			
				for (int i = 0; i < GameController.Instance.crotchesBittenFaceList.Count; i++) 
				{

						switch( GameController.Instance.crotchesBittenFaceList[i])
						{
							case 1: crotchType1++; break;
							case 2: crotchType2++; break;
							case 3: crotchType3++; break;
							case 4: crotchType4++; break;
							default:Debug.Log ("STRANGE CROTCH TYPE");break;
						}
				}


				int greaterCrotch = 0;
				//crotchtype 1 is greater
				if (crotchType1 > crotchType2) 
				{
						//crotch 3 greater
						if (crotchType3 > crotchType4) 
						{
								if (crotchType3 > crotchType1) {
										greaterCrotch = 3;
								} else {
										greaterCrotch = 1;
								}
						}
						else 
						{
								if (crotchType4 > crotchType1) {
										greaterCrotch = 4;
								} else 
								{
										greaterCrotch = 1;
								}
						
						}
				}
				else  // crotch 2 is greater
				{
						//crotch 3 greater
						if (crotchType3 > crotchType4) 
						{
								if (crotchType3 > crotchType2) {
										greaterCrotch = 3;
								} else {
										greaterCrotch = 2;
								}
						}
						else 
						{
								if (crotchType4 > crotchType2) 
								{
										greaterCrotch = 4;
								}else {
										greaterCrotch = 2;
								}

						}
				}


				crotchNemesis.SetSprite (Character.LEGS_SPRITE + greaterCrotch);
		}

		void UpdateTotalScore()
		{
				totalScoreUpdateTime += Time.deltaTime;


				if (totalScoreUpdateTime <= 1.0f) {
						int score = (int)(GameController.Instance.totalScore * totalScoreUpdateTime);
						TotalScoreLabel.text = "Total Score: " + score;
				}
				else 
				{
						TotalScoreLabel.text = "Total Score: " + GameController.Instance.totalScore;
						updatedTotalScore = true;
				}
		}

		void UpdateSlanderMessage()
		{
			
				float opacity = SlanderLabel.color.a;
		
				if (opacity < 1.0f) 
				{
						opacity += Time.deltaTime;
						SlanderLabel.color = new Color (1.0f, 1.0f, 1.0f, opacity);
				} 
				else 
				{
						SlanderLabel.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
						showingSlander = true;
				}

		}
		// sets the message at the end based on points
		void setSlanderMessage()
		{
				if (GameController.Instance.totalScore < 0) 
				{
						int showMessage = Random.Range (0,minusSlander.Length);
						SlanderLabel.text = "" + minusSlander[showMessage];

				}
				else if(GameController.Instance.totalScore == 0)
				{
						int showMessage = Random.Range (0,zeroSlander.Length);
						SlanderLabel.text = "" + zeroSlander[showMessage];
				}
				else if (GameController.Instance.totalScore < 100) 
				{
						int showMessage = Random.Range (0,okSlander.Length);
						SlanderLabel.text = "" + okSlander[showMessage];
				}
				else 
				{
						int showMessage = Random.Range (0,GoodSlander.Length);
						SlanderLabel.text = "" + GoodSlander[showMessage];
				}

		}

		public void skipAnimations()
		{
				skip = true;
		}

}
