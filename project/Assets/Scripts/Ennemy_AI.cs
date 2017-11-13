using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_AI : MonoBehaviour {

	private GameObject player;
	public int HP;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		if (HP <= 0)
			die ();
		Vector3 dir = player.transform.position - GetComponent<Rigidbody>().transform.position;
		dir.y = 0;
		dir.Normalize ();
		transform.Translate (dir*Time.deltaTime*2);
	}

	void die(){
		Destroy (gameObject);
	}
}
