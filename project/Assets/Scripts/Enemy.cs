using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemy : MonoBehaviour {
	private GameObject m_player;
	public int HP;
	public int speed;
	public int dmg;
	public int damage_body;
	public int damage_head;
	public int range;

	// Use this for initialization
	void Start ()
	{
		m_player = GameObject.Find("Character");
	}

	// Update is called once per frame
	void Update()
	{
		if (HP <= 0)
			die();
		Vector3 dir = m_player.transform.position - GetComponent<Rigidbody>().transform.position;
		dir.y = 0;
		if (dir.magnitude < range)
		{
			dir.Normalize();
			transform.Translate(dir * Time.deltaTime * (speed / 100));
		}
}

	public void damage(int value){
		HP -= value;
		Debug.Log (value);
	}

	public void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.CompareTag ("Character")) {
			KnockBack (-(gameObject.GetComponent<Rigidbody> ().velocity));
			other.gameObject.GetComponent<CustomCharacterController>().damage (dmg);
		}
	}

	public void KnockBack(Vector3 vec){
		vec.y=0;
		vec.Normalize();
		vec.y=0.2f;
		vec.Normalize();
		gameObject.GetComponent<Rigidbody>().AddForce(vec*speed*2);
	}

	public void die(){
		Destroy (gameObject);
	}
}