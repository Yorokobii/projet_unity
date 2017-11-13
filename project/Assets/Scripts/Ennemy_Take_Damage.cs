﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_Take_Damage : MonoBehaviour {

	public int damage_mulitplier;

	public void damage(int value){
		gameObject.transform.parent.GetComponent<Ennemy_AI> ().HP -= value*damage_mulitplier;
	}

	public void KnockBack(Vector3 vec){
		vec.y=0;
		vec.Normalize();
		vec.y=1;
		vec.Normalize();
		gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().AddForce(vec*500);
	}
}