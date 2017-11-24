using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject enemy;
	public GameObject Cube;
	private bool benemy=true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Time.time%5);
		if (((int)Time.time) % 5 > 0) {
			if (benemy) {
				//Instantiate (enemy, new Vector3 (0.0f, 1.5f, 0.0f), Quaternion.identity);
				//Instantiate (Cube, new Vector3 (0.0f, 1.5f, 0.0f), Quaternion.identity);
				benemy = false;
			}
		} else {
			benemy = true;
		}
	}
}
