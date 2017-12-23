using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour {
	private int x;
	public GameObject particles;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) { //other is the other object colliding with the snowball
		//Damage other if ennemy
		if (other.CompareTag ("Head") || other.CompareTag ("Body")){
			//Destroy (other); //use that one for cool effect but doesn't destroy the ennemy
			if (other.CompareTag ("Head")) {
				Debug.Log ("Headshot");
				x = other.transform.parent.GetComponent<Enemy> ().damage_head;
			}
			else if (other.CompareTag ("Body")) {
				Debug.Log ("Bodyshot");
				x = other.transform.parent.GetComponent<Enemy> ().damage_body;
			}
			other.transform.parent.GetComponent<Enemy> ().damage (x);
			other.transform.parent.GetComponent<Enemy> ().KnockBack (gameObject.GetComponent<Rigidbody> ().velocity);	
		}
		Instantiate(particles, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
	}
}
