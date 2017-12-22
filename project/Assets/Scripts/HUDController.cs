using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class HUDController : MonoBehaviour {

	public GameObject character;
	public GameObject grab;
	public GameObject release;
	public Slider life;
	public Text ammo;
	
	// Update is called once per frame
	void Update () {
		if (character.GetComponent<CustomCharacterController> ().looking)
			grab.SetActive(true);
		else
			grab.SetActive(false);

		if (character.GetComponent<CustomCharacterController> ().Grabbing ()) {
			release.SetActive (true);
		}
		else{
			release.SetActive (false);
		}

		life.maxValue = character.GetComponent<CustomCharacterController> ().HP_Max;
		life.value = character.GetComponent<CustomCharacterController> ().HP;
		ammo.text = character.GetComponent<CustomCharacterController> ().GetAmmo() +"/"+character.GetComponent<CustomCharacterController> ().AmmoMax;
	}
}
