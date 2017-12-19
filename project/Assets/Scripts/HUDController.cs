using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class HUDController : MonoBehaviour {

	public GameObject character;
	public GameObject grab;
	public Slider life;
	
	// Update is called once per frame
	void Update () {
		if (character.GetComponent<CustomCharacterController> ().looking)
			grab.SetActive(true);
		else
			grab.SetActive(false);

		life.maxValue = character.GetComponent<CustomCharacterController> ().HP_Max;
		life.value = character.GetComponent<CustomCharacterController> ().HP;
	}
}
