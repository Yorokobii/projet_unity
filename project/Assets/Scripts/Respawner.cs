using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Respawner : MonoBehaviour {

	public int damages;

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.CompareTag ("Character")) {
			collider.gameObject.GetComponent<CustomCharacterController> ().damage (damages);
			collider.gameObject.GetComponent<CustomCharacterController> ().respawn ();
		} else if (collider.gameObject.CompareTag ("Dynamic")) {
			collider.gameObject.GetComponent<GrabbableBehavior> ().respawn ();
		} else if (collider.gameObject.CompareTag ("Body")) {
			Destroy (collider.gameObject);
		}
	}
}