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

	void Start(){
		//if the scene was loaded as the game was paused we need to make sure the audio is playing again
		AudioListener.pause = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && canvas.activeInHierarchy == false) {
			Pause ();
		}
		if (canvas.activeInHierarchy) {
			if (AudioListener.volume <= 0)
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
			AudioListener.pause = true;
		}
		else{
			canvas.SetActive(false);
			Time.timeScale = 1;
			Character.GetComponent<CustomCharacterController> ().enabled = true;
			Cursor.visible = false;
			AudioListener.pause = false;
		}
	}

	public void Exit(){
		Pause ();
		SceneManager.LoadScene ("MainMenu");
	}

	public void Mute(){
		if (AudioListener.volume > 0)
			AudioListener.volume = 0;
		else
			AudioListener.volume = 1;
	}
}
