using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum TypeBonus
{
	ammo,
	vie
}
public class BonusBehavior : MonoBehaviour {

	public TypeBonus typeBonus;
	public float dureeRespawnAmmo = 5.0f;
	public int pointBonus;
	private float timer;
	private bool b = false;

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
			timer = Time.time + dureeRespawnAmmo;
			switch (typeBonus)
			{
				case TypeBonus.ammo:
				{
					if (!b)
					{
						other.gameObject.GetComponent<CustomCharacterController> ().AddAmmo(pointBonus);
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
		case TypeBonus.ammo:
			{
				gameObject.GetComponent<MeshRenderer> ().enabled = true;
				break;
			}
		}
		b = false;
	}


}
