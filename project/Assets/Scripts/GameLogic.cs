using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject ennemy;
	public GameObject Cube;
	private bool bennemy=true;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (8, 9);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Time.time%5);
		if (((int)Time.time) % 5 > 0) {
			if (bennemy) {
				//Instantiate (ennemy, new Vector3 (0.0f, 1.5f, 0.0f), Quaternion.identity);
				Instantiate (Cube, new Vector3 (0.0f, 1.5f, 0.0f), Quaternion.identity);
				bennemy = false;
			}
		} else {
			bennemy = true;
		}
	}
}
