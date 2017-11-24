using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCube : MonoBehaviour {

	public GameObject player;

	void Start()
	{
		player = GameObject.Find ("Character");
	}
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Character")
			Physics.IgnoreCollision (player.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
	}
}
