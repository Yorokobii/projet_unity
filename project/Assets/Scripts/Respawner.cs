using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Respawner : MonoBehaviour {

	public int damages;

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.CompareTag ("Character")) { //if character touches
			collider.gameObject.GetComponent<CustomCharacterController> ().damage (damages);
			collider.gameObject.GetComponent<CustomCharacterController> ().respawn ();
		} else if (collider.gameObject.CompareTag ("Dynamic")) { //if a dynamic object touches
			if(collider.gameObject.GetComponent<GrabbableBehavior> ())
				collider.gameObject.GetComponent<GrabbableBehavior> ().respawn ();
			else
				collider.transform.parent.gameObject.GetComponent<GrabbableBehavior> ().respawn ();
		} else if (collider.gameObject.CompareTag ("Body")) { //if an ennemy touches
			Destroy (collider.gameObject);
		}
	}
}