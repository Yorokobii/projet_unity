using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour {

	public float lifespan; //seconds

	// Use this for initialization
	void Start () {
		lifespan = Time.time + 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= lifespan)
			Destroy (this);
	}
}
