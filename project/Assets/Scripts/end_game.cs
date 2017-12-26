using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end_game : MonoBehaviour {

	public float scene_time;
	private float timer = -1;
	public GameObject canvas;
	private bool menu = false;

	// Use this for initialization
	void Start () {
		timer = Time.time + scene_time;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer != -1 && timer <= Time.time) {
			if(menu) SceneManager.LoadScene ("MainMenu");
			canvas.SetActive(true);
			timer = Time.time + 2; //wait 3 secs and return to mainmenu
			menu = true;
		}
	}
}
