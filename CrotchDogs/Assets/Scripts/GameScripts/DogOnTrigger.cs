using UnityEngine;
using System.Collections;

public class DogOnTrigger : MonoBehaviour {

		public Dog dog;

		void OnTriggerEnter ( Collider other)
		{
				Debug.Log ("enter triggered on dog trigger");
				dog.OnTriggerEnter (other);
		}

		void OnTriggerExit ( Collider other)
		{
				Debug.Log ("exit triggered on dog trigger");
				dog.OnTriggerExit (other);
	
		}
}
