using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class billboard : MonoBehaviour
{

	private GameObject character;
	
	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void Update ()
	{
//		float angle = Vector3.Angle(transform.position, character.transform.position);
//		Vector3 rot = Vector3.RotateTowards(transform.position, character.transform.position, 0, 0);
//		transform.Rotate(rot);
//		Quaternion.RotateTowards(transform.rotation, character.transform.rotation, 0);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, character.transform.rotation, 360);
		
	}
}
