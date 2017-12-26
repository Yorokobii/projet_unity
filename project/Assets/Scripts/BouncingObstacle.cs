using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : MonoBehaviour {

	public float speed;
	public AudioClip audio;
	void OnCollisionEnter(Collision other)
	{
		gameObject.GetComponent<AudioSource> ().PlayOneShot (audio);
		other.rigidbody.AddForce((-other.contacts [0].normal)*speed,ForceMode.Impulse);
	}
}
