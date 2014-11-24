using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	//=-=-=-=-=-=-=
	// spawn the faces for the victims
	//=-=-=-=-=-=-=
	public GameObject ShowBitePos,SpawnBiteLocation,ResetBiteLocation;
	public GameObject ShowMissPos,SpawnMissLocation,ResetMissLocation;
	public GameObject ShowMaulPos,SpawnMaulLocation,ResetMaulLocation;
	public MovingText Rank;
	

	public GameObject ShowLegsPos,SpawnLegLocation,ResetLegLocation;

	public tk2dSprite biteFace,missFace,maulFace,Legs01,crotchNemesis;

	public int biteIndex=0,missIndex=0,maulIndex=0; 

	public bool movingBite=false,movingMiss=false,movingMaul=false,movingLegs01=false,showingNemesis=false,showingRank=false;

	public int showVictimIndex=0,showEscapedIndex;
	bool showVictims = false,skip=false,updatedTotalScore = false,showingSlander= false;
	private const float SHOW_ALL_TIME_LIMIT = 5.0f;

	public int TotalScore =0;

	public float move_speed,totalScoreUpdateTime= 0.0f;
	// =-=-=-=-=-=-=-=
	// Game Over Labels
	// =-=-=-=-=-=-=-=
	public tk2dTextMesh CrotchesBittenLabel,CrotchesMissedLabel,CrotchesMauledLabel, ComboScoreLabel, MissesLabel,CharactersEscapedLabel ,TotalScoreLabel, SlanderLabel;


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
								if (movingBite) {

										movingBite = UpdateFacesOfTheVictims (biteFace, ShowBitePos.transform.position, ResetBiteLocation.transform.position, SpawnBiteLocation.transform.position, 1);
								} else if (movingMiss) {
										movingMiss = UpdateFacesOfTheVictims (missFace, ShowMissPos.transform.position, ResetMissLocation.transform.position, SpawnMissLocation.transform.position, 0);
								} else if (movingMaul) {
										Debug.Log ("update maul ");
										movingMaul = UpdateFacesOfTheVictims (maulFace, ShowMaulPos.transform.position, ResetMaulLocation.transform.position, SpawnMaulLocation.transform.position, 2);
								}
//										else if (movingLegs01) {	
//										UpdateCrotchesMissed ();
//								} 
//								else if (!showingNemesis) {
//										UpdateNemesis ();
//								} 
								else if (!updatedTotalScore) {
										UpdateTotalScore ();
								} else if (!showingRank) 
								{
										showRankLabel ();
								}		
								else if (!showingSlander) {
										UpdateSlanderMessage ();
								}
						}


				} else 
				{
						//if (showVictims) 
						{
								if (movingBite) 
								{

										movingBite = UpdateFacesOfTheVictims(biteFace,ShowBitePos.transform.position,ResetBiteLocation.transform.position,SpawnBiteLocation.transform.position,1);
								} 

								if (movingMiss) 
								{
										movingMiss = UpdateFacesOfTheVictims(missFace,ShowMissPos.transform.position,ResetMissLocation.transform.position,SpawnMissLocation.transform.position,0);
								}

								if (movingMaul) 
								{
										Debug.Log ("update maul ");
										movingMaul = UpdateFacesOfTheVictims(maulFace,ShowMaulPos.transform.position,ResetMaulLocation.transform.position,SpawnMaulLocation.transform.position,2);
								}

//								if (movingLegs01)
//								{	
//										UpdateCrotchesMissed ();
//								} 

								if (!showingRank) 
								{
										showRankLabel ();
								}

//								if (!showingNemesis) 
//								{
//										UpdateNemesis ();
//								}
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
			GameController.Instance.bestCombo * CrotchDogConstants.SCORE_COMBO;


			setSlanderMessage ();

			setGameOverInfo ();

	}

	//sets all labels and reseets all variables to show again
	public void setGameOverInfo()
	{
		skip = false;
		Rank.setToStartPosition ();

		ComboScoreLabel.text = "Best Combo: " + GameController.Instance.bestCombo;
		MissesLabel.text = "Misses: " + GameController.Instance.bitesMissed;
		CrotchesBittenLabel.text = "" + 0;

		CharactersEscapedLabel.text = "Escaped: " + 0;
		TotalScoreLabel.text = "Total Score: " + 0;
		
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
			
		//set up for bitten victims
		if (GameController.Instance.crotchesBitten == 0) {
				biteFace.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				movingBite = false;
		} else {
				biteFace.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				movingBite = true;
		}

		//set up for mauled victimes
		if (GameController.Instance.crotchesMauled == 0) {
				maulFace.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				movingMaul = false;
		} else {
				maulFace.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				movingMaul = true;
		}

		//set up for mauled victims
		if (GameController.Instance.bitesMissed == 0) {
			missFace.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
			movingMiss = false;
		} else {
			missFace.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			movingMiss = true;
		}


		updatedTotalScore = false;
		showingSlander = false;
		totalScoreUpdateTime = 0.0f;
		
		SlanderLabel.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

		biteIndex = 0;
		maulIndex = 0;
		missIndex = 0;

		showingRank = false;


		CrotchesBittenLabel.text = "0";
		CrotchesMauledLabel.text = "0";
		CrotchesMissedLabel.text = "0";
	}

	public void reset()
	{
			showVictims = false;
			showVictimIndex = 0;
			movingBite = false;
			movingMaul = false;
			movingMiss = false;
			movingLegs01=false;
			skip = false;
			biteIndex = 0;
			missIndex=0;
			maulIndex=0; 

			CrotchesBittenLabel.text = "0";
			CrotchesMauledLabel.text = "0";
			CrotchesMissedLabel.text = "0";

	}

	//updates the victims
	public bool UpdateFacesOfTheVictims(tk2dSprite face01,Vector3 ShowFacePos,Vector3 ResetFaceLocation,Vector3 SpawnFaceLocation,int Facetype)
	{
				float deltaTime = Time.deltaTime;
				bool movingFace01 = true;
				bool stopFace = false;
				//moves sprite by speed

				switch (Facetype) 
				{
					//miss
				case 0:
						if (missIndex > 0) {
								move_speed = -15.0f * (GameController.Instance.bitesMissed / missIndex) * deltaTime;
						} else {
								move_speed = -15.0f * deltaTime;
						}
						if (missIndex >= GameController.Instance.bitesMissed-1) 
						{
								stopFace = true;
						}

							break;
					//bite
				case 1:	
						if (biteIndex > 0) {
								move_speed = -15.0f * (GameController.Instance.crotchesBitten / biteIndex) * deltaTime;
						} else {
								move_speed = -15.0f *deltaTime;
						}

						if (biteIndex >= GameController.Instance.crotchesBitten-1) 
						{
								stopFace = true;
						}

							break;

					//maul
				case 2:	if(maulIndex >0)
						{
								move_speed = -15.0f*(GameController.Instance.crotchesMauled/maulIndex) *deltaTime;
						} else {
								move_speed = -15.0f *deltaTime;
						}

						if (maulIndex >= GameController.Instance.crotchesMauled-1) 
						{
								stopFace = true;
						}

							break;
					default:	move_speed = -15.0f *deltaTime;
							break;
				}
			
			
				if (movingFace01 ) 
				{
						if (face01.transform.position.y > ShowFacePos.y) // move face to show popsition
						{
								if ((face01.gameObject.transform.position.y + move_speed) < ResetFaceLocation.y) {
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ResetFaceLocation.y,	face01.gameObject.transform.position.z);
								} else {
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	face01.gameObject.transform.position.y + move_speed,	face01.gameObject.transform.position.z);
								}
						}
						else if (face01.transform.position.y > ResetFaceLocation.y  && !stopFace) //move face to reset position
						{
								if((face01.gameObject.transform.position.y +  move_speed) < ResetFaceLocation.y)
								{
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ResetFaceLocation.y,	face01.gameObject.transform.position.z);
								}
								else
								{
									face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	face01.gameObject.transform.position.y +  move_speed,	face01.gameObject.transform.position.z);
								}
						}
						else  // reset face
						{
								//movingFace01 = false;
							

						

								if (Facetype == 0) 
								{
										SoundManager.PlaySwoosh();
										missIndex++;
										//if ran out of victims to show
										if (missIndex >= GameController.Instance.bitesMissed ) 
										{
												movingFace01 = false;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ShowFacePos.y,	face01.gameObject.transform.position.z);

										}
										else// move face back to reset position so it moves down with new face
										{
												CrotchesMissedLabel.text = "" + missIndex;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	SpawnFaceLocation.y,	face01.gameObject.transform.position.z);

												face01.SetSprite ("miss" + Random.Range(1,6));
										}
								}
								else if (Facetype == 1) 
								{
										SoundManager.PlayBite();
										biteIndex++;
										//if ran out of victims to show
										if (biteIndex >= GameController.Instance.crotchesBitten ) 
										{
												movingFace01 = false;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ShowFacePos.y,	face01.gameObject.transform.position.z);

										}
										else// move face back to reset position so it moves down with new face
										{
												CrotchesBittenLabel.text = "" + biteIndex;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	SpawnFaceLocation.y,	face01.gameObject.transform.position.z);

												face01.SetSprite ("bite" + Random.Range(1,6));
										}
								}
								else if (Facetype == 2) 
								{
										SoundManager.PlayBite();
										Debug.Log ("maul " +  GameController.Instance.crotchesMauled);
										maulIndex++;
										//if ran out of victims to show
										if (maulIndex >= GameController.Instance.crotchesMauled ) 
										{
												movingFace01 = false;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	ShowFacePos.y,	face01.gameObject.transform.position.z);

										}
										else// move face back to reset position so it moves down with new face
										{
												CrotchesMauledLabel.text = "" + maulIndex;
												face01.gameObject.transform.position = new Vector3 (face01.gameObject.transform.position.x,	SpawnFaceLocation.y,	face01.gameObject.transform.position.z);

												face01.SetSprite ("maul" + Random.Range(1,6));
										}
								}


						}
				}
		return movingFace01;

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
								showEscapedIndex++;
						}
				}
		}

		//updates the nemesis
		void UpdateNemesis()
		{
				float opacity = crotchNemesis.color.a;

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

				int greaterCrotch = 0;
				int[] crotchTypeCount = new int[CrotchDogConstants.NUM_OF_CHARACTERS];
			
				for (int i = 0; i < GameController.Instance.crotchesEscapedLegList.Count; i++) 
				{
						crotchTypeCount [GameController.Instance.crotchesEscapedLegList [i]-1]++;
				}

				int crotchAmount = 0;
				for (int i = 0; i < crotchTypeCount.Length; i++) 
				{
						if (crotchTypeCount [i] > crotchAmount) 
						{
								crotchAmount = crotchTypeCount [i];
								greaterCrotch = i+1;
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


		public void showRankLabel()
		{
				showingRank = true;

				Rank.setToStartPosition ();


				if (GameController.Instance.totalScore <= CrotchDogConstants.HIGHSCORE_PLAYFUL) 
				{
						Rank.showFlavourText (MovingText.InfoType.PLAYFUL);
				}
				else if (GameController.Instance.totalScore <= CrotchDogConstants.HIGHSCORE_NAUGHTY) 
				{
						Rank.showFlavourText (MovingText.InfoType.NAUGHTY);
				}
				else if (GameController.Instance.totalScore <= CrotchDogConstants.HIGHSCORE_AGGRESIVE) 
				{
						Rank.showFlavourText (MovingText.InfoType.AGGRESSIVE);
				}
				else if (GameController.Instance.totalScore <= CrotchDogConstants.HIGHSCORE_MONSTROUS) 
				{
						Rank.showFlavourText (MovingText.InfoType.MONSTROUS);
				}
				else 
				{
						Rank.showFlavourText (MovingText.InfoType.CROTCH_DOG);
				}
	

	


		}
}
