using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	private static bool tp = false;

    public Transform destination;

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Character")
        {
			if (!tp) {
				
				other.transform.position = destination.position;
				other.transform.Translate (0f, 0.8f, 0f);
				other.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				tp = true;
			} else
				tp = false;
        }
    }
}
