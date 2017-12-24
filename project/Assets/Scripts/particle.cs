using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{
	private float timer;
	public float aliveTime;
	public Vector3 velocity;
	public float speed;
	
	// Use this for initialization
	void Start ()
	{
		if(aliveTime >= 0)
			timer = aliveTime + Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (velocity * speed * Time.deltaTime);

		if (aliveTime > 0 || !gameObject.GetComponent<ParticleSystem>().isPlaying)
			if(aliveTime <= 0 || timer <= Time.time)
				Destroy(gameObject);
	}
}
