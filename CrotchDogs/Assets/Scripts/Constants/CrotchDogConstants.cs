public class CrotchDogConstants  {

		//Game Constants
		//types of characters
		public const int NUM_OF_CHARACTERS = 5;
		//max that can be spawned
		public const int MAX_CHARACTERS =15,MAX_SCENE_OBJECTS=10;
		public const float DISTANCE_BETWEEN_LANES = 0.0f;
		public const float CROTCH_MAX_DISTANCE = 5.0f;
		public const int NUM_OF_ESCAPES = 3;
		public const float POINTS_MOVE_SPEED = 0.25f;

		public const float CHASER_INCREMENT = 0.1f,DOG_INCREMENT=0.1f;
		public const float CHARACTER_SPEED = -10.0f, CHARACTER_POWER_UP_SPEED = -15.0f;
		//Spawn Intervals
		public const float MIN_SPAWN_INTERVAL=0.6f,MAX_SPAWN_INTERVAL=3.0f;
		public const float DECREASE_SPAWN_INTERVAL=0.1f,TIME_TILL_DECREASE=5.0f;

		//Dog Constants
		public const float NO_BITE = -1.0f;
		public const float MIN_BITE_SCALE = 0.15f,MAX_BITE_SCALE = 1.0f;
		public const float MAUL_DISTANCE = 0.1f;

		//Face Animation Constants
		public const float IDLE_TIME=0.0f,MISS_TIME=0.5f,MAUL_TIME=0.5f,BITE_TIME =0.5f;

		//scene constants
		public const float CHANGE_SCENE_TIME =1.0f;

		//WAVE Constants
		public const int AMOUNT_OF_WAVES = 3;
		public const int SPAWNS_BEFORE_NEXT_WAVE = 10;


		//Scores
		public const int SCORE_MISSES = -5, SCORE_BITE = 15, SCORE_MAUL = 20,SCORE_ESCAPED = -10,SCORE_COMBO = 20;
		public const int HIGHSCORE_PLAYFUL    =50,
						 HIGHSCORE_NAUGHTY    =100,
					 	 HIGHSCORE_AGGRESIVE  =150,
						 HIGHSCORE_MONSTROUS  =200,
						 HIGHSCORE_CROTCH_DOG =250; 


		//Power Ups
		public const float POWER_UP_LASTS = 5.0f;

		//Time const step
		public const float TIME_STEP = 0.016666667f;




}
