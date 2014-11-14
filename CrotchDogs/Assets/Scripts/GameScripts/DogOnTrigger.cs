using UnityEngine;
using System.Collections;

public class DogOnTrigger : MonoBehaviour {

		public Dog dog;

		void OnTriggerEnter ( Collider other)
		{
				dog.OnTriggerEnter (other);
		}

		void OnTriggerExit ( Collider other)
		{
			dog.OnTriggerExit (other);
		}
}
