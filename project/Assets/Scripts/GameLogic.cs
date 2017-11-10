using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public Rigidbody ennemy;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (8, 9);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time % 5 == 0)
			Instantiate (ennemy, new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity);
	}
}
