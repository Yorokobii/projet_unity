using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_AI : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = player.transform.position - GetComponent<Rigidbody>().transform.position;
		dir.y = 0;
		dir.Normalize ();
		transform.Translate (dir*Time.deltaTime*2);
	}
}
