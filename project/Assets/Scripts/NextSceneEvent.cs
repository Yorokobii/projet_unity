using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class NextSceneEvent : MonoBehaviour {
	public string nextScene;
	public GameObject explanations;
	public GameObject gg;
	public GameObject character;
	public GameObject pause;
	private bool done = false;
	private float timer = -1;

	// Use this for initialization
	void Start ()
	{
		character.GetComponent<CustomCharacterController> ().enabled = false;
		pause.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && explanations.activeInHierarchy) {
			explanations.SetActive (false);
			character.GetComponent<CustomCharacterController> ().enabled = true;
			pause.SetActive(true);
		}

		if (done) {
			gg.SetActive (true);
			character.GetComponent<CustomCharacterController> ().enabled = false;
			character.GetComponent<Rigidbody>(). velocity = Vector3.zero;
			pause.SetActive(false);
			timer = Time.time + 1;
			done = false;
		}

		if (timer != -1 && timer < Time.time) {
			character.GetComponent<CustomCharacterController> ().enabled = true;
			pause.SetActive(true);

//			SceneManager.LoadScene (nextScene);
			SceneManager.LoadScene ("test_scene1");
		}
	}

	public void OnTriggerEnter(Collider other){
		done = true;
	}
}
