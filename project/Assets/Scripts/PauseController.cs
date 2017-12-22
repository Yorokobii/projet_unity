using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseController : MonoBehaviour {
	public GameObject canvas;
	public GameObject Character;


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && canvas.activeInHierarchy == false) {
			Pause ();
		}
		if (canvas.activeInHierarchy) {
			if (AudioListener.pause)
				GameObject.Find ("Sound Mute").GetComponent<Text> ().text = "Unmute Sound";
			else
				GameObject.Find ("Sound Mute").GetComponent<Text> ().text = "Mute Sound";
		}
	}

	public void Pause(){
		if(canvas.activeInHierarchy == false){
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			canvas.SetActive(true);
			Time.timeScale = 0;
			Character.GetComponent<CustomCharacterController> ().enabled = false;
		}
		else{
			canvas.SetActive(false);
			Time.timeScale = 1;
			Character.GetComponent<CustomCharacterController> ().enabled = true;
			Cursor.visible = false;
		}
	}

	public void Exit(){
		Pause ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void Mute(){
		AudioListener.pause = !AudioListener.pause;
	}
}
