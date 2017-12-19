using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseController : MonoBehaviour {
	public GameObject canvas;
	public GameObject Character;


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}
	}

	public void Pause(){
		if(canvas.activeInHierarchy == false){
			canvas.SetActive(true);
			Time.timeScale = 0;
			Character.GetComponent<CustomCharacterController> ().enabled = false;
			Cursor.visible = true;
		}
		else{
			canvas.SetActive(false);
			Time.timeScale = 1;
			Cursor.visible = false;
			Character.GetComponent<CustomCharacterController> ().enabled = true;
		}
	}

	public void Exit(){
		Application.Quit();
	}

	public void Mute(){
		AudioListener.pause = !AudioListener.pause;
		if (AudioListener.pause)
			GameObject.Find ("Sound Mute").GetComponent<Text> ().text = "Unmute Sound";
		else
			GameObject.Find ("Sound Mute").GetComponent<Text> ().text = "Mute Sound";
	}
}
