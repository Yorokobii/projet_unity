using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	
	public GameObject Walker;
	public GameObject Runner;
	public GameObject GrosPorc;
	public GameObject Sniper;
	public GameObject Shooter;
	public Vector3[] spawn_location;
	public GameObject[] spawn_enemy;
	public float[] spawn_time;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < spawn_enemy.Length; ++i)
		{
			if (Time.time > spawn_time[i] && spawn_time[i] > 0)
			{
				Instantiate<GameObject>(spawn_enemy[i], spawn_location[i], Quaternion.identity);
				//parceque c'est plus rigolo sans le fix attention à pas mourir par contre... Ctrl+P sinon XD
				//spawn_time[i] = -1;
			}
		}
	}
}