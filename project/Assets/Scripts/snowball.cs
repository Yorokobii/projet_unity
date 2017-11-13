using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other) { //other is the other object colliding with the snowball
		//Damage other if ennemy
		if (other.CompareTag ("Ennemy")){
			//Destroy (other); //use that one for cool effect but doesn't destroy the ennemy
			other.GetComponent<Ennemy_Take_Damage>().damage(1);
			other.GetComponent<Ennemy_Take_Damage>().KnockBack(gameObject.GetComponent<Rigidbody>().velocity);
		}
		Destroy(gameObject);
	}
}
