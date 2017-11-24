using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : MonoBehaviour {

	public float speed;
	void OnCollisionEnter(Collision other)
	{
		other.rigidbody.AddForce((-other.contacts [0].normal)*speed,ForceMode.Impulse);
	}
}
