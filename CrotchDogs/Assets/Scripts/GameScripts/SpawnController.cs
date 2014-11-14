using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
	

	private float timeSinceDecrease= 0.0f,timeSinceSpawn=0.0f;
	public float spawnInterval;

	//Spawn Waves Information
	bool spawningWave=true;
	int waveNumber =0;
	int spawnsSinceLastWave = 0;
	int waveValue = 0;

	public float[] wave01 = new float[]{0.6f,0.6f,0.6f,0.6f,3.0f,3.0f,1.0f,2.0f,0.6f,0.6f,0.6f,0.6f,3.0f,0.2f}; 
	public float[] wave02 = new float[]{3.0f,2.0f,1.0f,0.9f,0.8f,0.7f,0.6f,0.6f,0.6f,0.6f,0.6f,0.6f,3.0f,3.2f}; 
	public float[] wave03 = new float[]{1.5f,0.6f,1.5f,0.6f,3.0f,2.0f,1.0f,2.3f,0.6f,0.6f,0.7f,0.7f,0.9f,0.9f}; 


	// Use this for initialization
	void Start () 
	{
		reset ();
	}

	void Awake()
	{
		reset();
	}
	
	// Update is called once per frame
	public bool UpdateToSpawn (float time) 
	{
		bool willSpawn = false;
				// if its not in spawning wave mode then spawn at a set interval
				if (!spawningWave) 
				{
						timeSinceDecrease += time;
						timeSinceSpawn += time;
		
						if (timeSinceSpawn >= spawnInterval) {
								timeSinceSpawn = 0.0f;
								willSpawn = true;
								spawnsSinceLastWave++;

								if (spawnsSinceLastWave >= CrotchDogConstants.SPAWNS_BEFORE_NEXT_WAVE) 
								{
										spawnsSinceLastWave = 0;
										spawningWave = true;
										waveValue = 0;
								}
						}
	
						if (timeSinceDecrease >= CrotchDogConstants.TIME_TILL_DECREASE) {
								timeSinceDecrease = 0.0f;
								spawnInterval -= CrotchDogConstants.DECREASE_SPAWN_INTERVAL;

								if (spawnInterval < CrotchDogConstants.MIN_SPAWN_INTERVAL) {
										spawnInterval = CrotchDogConstants.MIN_SPAWN_INTERVAL;
								}
			
						}
				}
				else 
				{
					
						float waveSpawnInterval = getWaveSpawnInterval();

						timeSinceSpawn += time;

						if (timeSinceSpawn >= waveSpawnInterval) 
						{
							
								timeSinceSpawn = 0.0f;
								waveValue++;
								willSpawn = true;

								if (waveFinished ()) 
								{
										waveValue = 0;
										waveNumber++;

										if (waveNumber >= CrotchDogConstants.AMOUNT_OF_WAVES) 
										{
												waveNumber = 0;
										}
										spawningWave = false;
								}
						}


				}
	

		return willSpawn;
	}
	
	public float getWaveSpawnInterval()
	{
				switch (waveNumber) 
				{
						case 0:

						if (waveValue < wave01.Length) 
						{
								return wave01 [waveValue];
						}
					
				
						break;
				case 1:

						if (waveValue < wave02.Length) 
						{
								return wave02 [waveValue];
						}


						break;
				case 2:

						if (waveValue < wave03.Length) 
						{
								return wave03 [waveValue];
						}


						break;
				}

				return 0.0f;
	}


		public bool waveFinished()
		{
				switch (waveNumber) 
				{
					case 0: return waveValue >= wave01.Length; break;
					case 1: return waveValue >= wave02.Length; break;
					case 2: return waveValue >= wave03.Length; break;

				}


				return true;
		}


	public void reset()
	{
			spawnInterval = CrotchDogConstants.MAX_SPAWN_INTERVAL;

			timeSinceDecrease = 0;
			timeSinceSpawn = 0;


			//Spawn Waves Information
			spawningWave=true;
			waveNumber =0;
			spawnsSinceLastWave = 0;
			waveValue = 0;
	}

}
