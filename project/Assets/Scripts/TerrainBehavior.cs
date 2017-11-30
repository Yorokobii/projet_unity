using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeTerrain
{
	rapide,
	lent, 
	glissant,
	grimpant
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
						Debug.Log ("rapide");
						break;
					}
				case TypeTerrain.lent:
					{					
						Debug.Log ("lent");
						break;
					}
				case TypeTerrain.glissant:
					{					
						Debug.Log ("glissant");
						break;
					}
				case TypeTerrain.grimpant:
					{					
						Debug.Log ("grimpant");
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
					Debug.Log ("rapide");
					break;
				}
			case TypeTerrain.lent:
				{					
					Debug.Log ("lent");
					break;
				}
			case TypeTerrain.glissant:
				{					
					Debug.Log ("glissant");
					break;
				}
			case TypeTerrain.grimpant:
				{					
					Debug.Log ("grimpant");
					break;
				}
			}
		}
	}
}
