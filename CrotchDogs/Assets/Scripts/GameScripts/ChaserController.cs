using UnityEngine;
using System.Collections;

public class ChaserController : MonoBehaviour {

		private const int MAX_DOG_INDEX =40,MAX_CHASER_INDEX=10; 
		private const float MIN_CHASER_INTERVAL_TIME = 0.5f,REDUCE_CHASER_INTERVAL=10.0f,REDUCE_AMOUNT=0.1f;
		public int dogLocationIndex=0,chaserLocationIndex=0;


		private float timePast=0.0f,timeSinceChaserIncreased=0.0f;
		private float moveChaserInterval=6.0f;

		private const int ESCAPED_CHARACTER_INCREMENT = 4;

		public void Maul()
		{

				if (dogLocationIndex <= MAX_DOG_INDEX) {
						dogLocationIndex++;
				}
				else 
				{
						chaserLocationIndex--;
				}
		}

		public void Bite()
		{
				timeSinceChaserIncreased=0.0f;
		}

		public void Miss()
		{
				chaserLocationIndex++;
		}


		public void Escaped()
		{
				chaserLocationIndex += ESCAPED_CHARACTER_INCREMENT;
		}


		public int getDogIndex()
		{
				return dogLocationIndex;
		}


		public int getChaserIndex()
		{
				return chaserLocationIndex;
		}


		void Update()
		{
				timePast += Time.deltaTime;
				timeSinceChaserIncreased += Time.deltaTime;

				if (timeSinceChaserIncreased >= moveChaserInterval) 
				{
						chaserLocationIndex++;

						timeSinceChaserIncreased = 0.0f;

				}

//				if (timePast >= REDUCE_CHASER_INTERVAL) 
//				{
//						moveChaserInterval -= REDUCE_AMOUNT;
//
//						if (moveChaserInterval <= MIN_CHASER_INTERVAL_TIME) 
//						{
//								moveChaserInterval = MIN_CHASER_INTERVAL_TIME;
//						}
//				}
//

		}

		public void reset()
		{
				dogLocationIndex = 0;
				chaserLocationIndex=0;


				timePast = 0.0f;
				timeSinceChaserIncreased=0.0f;
				moveChaserInterval=6.0f;
		}

	
}
