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
			end_timer = Time.time + winScreen.GetComponent<AudioSource>().clip.length;
			winScreen.SetActive(true);
			GameObject.Find ("PauseController").SetActive (false);
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
				go.SetActive (false);
		}

		if (!win && !loose && character.GetComponent<CustomCharacterController> ().Dead ()) {
			loose = true;
			end_timer = Time.time + looseScreen.GetComponent<AudioSource>().clip.length;
			looseScreen.SetActive(true);
			GameObject.Find ("PauseController").SetActive (false);
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
				go.SetActive (false);
		}

		if (win){
			if (end_timer <= Time.time || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
				SceneManager.LoadScene (WinScene);
		} else if (loose) {
			if (end_timer <= Time.time || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
				SceneManager.LoadScene ("MainMenu");
		}

	}


	private bool CheckAliveEnemies(){
		GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
		return enemies.Length <= 0;
	}
}