using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameLogic : MonoBehaviour {

	public GameObject character;
	public bool KillAllEnemies;
	private bool win = false;
	private bool loose = false;
	private float end_timer;
	public float end_time;
	public String WinScene;
	public GameObject looseScreen;
	public GameObject winScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!win && !loose && KillAllEnemies && CheckAliveEnemies ()) {
			win = true;
			end_timer = Time.time + end_time;
			winScreen.SetActive(true);
		}

		if (!win && !loose && character.GetComponent<CustomCharacterController> ().Dead ()) {
			loose = true;
			end_timer = Time.time + end_time;
			looseScreen.SetActive(true);
		}

		if (win){
			if (end_timer <= Time.time)
				SceneManager.LoadScene (WinScene);
		} else if (loose) {
			if (end_timer <= Time.time)
				SceneManager.LoadScene ("MainMenu");
		}

	}


	private bool CheckAliveEnemies(){
		GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
		return enemies.Length <= 0;
	}
}