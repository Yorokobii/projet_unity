using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum TypeBonus
{
	rapide,
	vie
}
public class BonusBehavior : MonoBehaviour {

	public TypeBonus typeBonus;
	public float dureeBonus = 5.0f;
	public int pointBonus;
	private float timer;
	private bool b = false;
	private GameObject Char;

	void Start(){
		timer = 0.0f;
	}

	void Update()
	{		
		if (Time.time > timer && b)
		{
			finBonus ();
		}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Character")
		{
			timer = Time.time + dureeBonus;
			switch (typeBonus)
			{
				case TypeBonus.rapide:
				{
					if (!b)
					{
						Char = other.gameObject;
						other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.SpeedBonus 
						= other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.Speed * 3;
						b = true;
						gameObject.GetComponent<MeshRenderer> ().enabled = false;
					}
					break;
				}
				case TypeBonus.vie:
				{
					other.gameObject.GetComponent<CustomCharacterController> ().HP+=pointBonus;
					Destroy (gameObject);
					break;
				}
			}
		}
	}

	void finBonus()
	{
		switch (typeBonus)
		{
		case TypeBonus.rapide:
			{
				Char.GetComponent<CustomCharacterController> ().movementSettings.SpeedBonus = 0;
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
				break;
			}
		}
		b = false;
	}


}
