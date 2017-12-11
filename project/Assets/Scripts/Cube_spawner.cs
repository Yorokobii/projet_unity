using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_spawner : MonoBehaviour
{
	private Vector3 m_pos;
	private bool b = false;
	public GameObject Cube;
	
	void Start()
	{
		m_pos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((int) Time.time % 5 == 0)
		{
			if (!b)
			{
				Instantiate(Cube, m_pos, Quaternion.identity);
				b = true;
			}
		}
		else
		{
			b = false;
		}
	}
}
