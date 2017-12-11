using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum TypeTerrain
{
	rapide,
	lent, 
	glissant,
	grimpant,
	rebondissant
}
public class TerrainBehavior : MonoBehaviour {

	public TypeTerrain typeTerrain;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Character")
		{
			switch (typeTerrain)
			{
				case TypeTerrain.rapide:
				{
					other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.SpeedTerrain 
					= other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.Speed*2;
						break;
					}
				case TypeTerrain.lent:
					{
						other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.SpeedTerrain 
						= -other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.Speed/2;
						break;
					}
				case TypeTerrain.glissant:
					{					
						break;
					}
				case TypeTerrain.grimpant:
					{					
						break;
					}
				case TypeTerrain.rebondissant:
					{					
						other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.JumpForceTerrain 
						= other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.JumpForce/2;
						break;
					}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Character")
		{
			switch (typeTerrain)
			{
			case TypeTerrain.rapide:
				{
					other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.SpeedTerrain 
					= 0;
					break;
				}
			case TypeTerrain.lent:
				{
					other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.SpeedTerrain 
					= 0;
					break;
				}
			case TypeTerrain.glissant:
				{					
					break;
				}
			case TypeTerrain.grimpant:
				{					
					break;
				}
			case TypeTerrain.rebondissant:
				{					
					other.gameObject.GetComponent<CustomCharacterController> ().movementSettings.JumpForceTerrain 
					= 0;
					break;
				}
			}
		}
	}
}
