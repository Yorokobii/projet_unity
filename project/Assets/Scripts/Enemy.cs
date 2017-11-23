using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public GameObject player;
	public int HP;
	public int speed;
	public int damage_body;
	public int damage_head;

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
		transform.Translate (dir*Time.deltaTime*(speed/100));
	}

	public void damage(int value){
		HP -= value;
	}

	public void KnockBack(Vector3 vec){
		vec.y=0;
		vec.Normalize();
		vec.y=0.2f;
		vec.Normalize();
		gameObject.GetComponent<Rigidbody>().AddForce(vec*speed*2);
	}

	public void die(){
		Destroy (gameObject);
	}
}
