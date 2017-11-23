using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_AI : Enemy {

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update () {
		if (HP <= 0)
			die ();
		//TO DO : FAIRE TIRER SUR LE JOUEUR
	}
}
