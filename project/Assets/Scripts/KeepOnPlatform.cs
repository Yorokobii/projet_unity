using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnPlatform : MonoBehaviour
{
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Character")
		other.transform.parent = gameObject.transform;
	}

	void OnTriggerExit (Collider other ) {
		if (other.tag == "Character")
			other.transform.parent = null;
	}
}