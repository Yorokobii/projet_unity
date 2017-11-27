using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehavior : MonoBehaviour {

	private Vector3 m_reset_position;
	private Quaternion m_reset_rotation;

	// Use this for initialization
	void Start () {
		m_reset_position = transform.position;
		m_reset_rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawn(){
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		transform.position = m_reset_position;
		transform.rotation = m_reset_rotation;
	}

	void OnCollisionEnter(Collision collision){
		if(gameObject.layer == 0){
			if(collision.gameObject.CompareTag ("Enemy"))
				collision.gameObject.GetComponent<Enemy> ().damage(collision.gameObject.GetComponent<Enemy> ().damage_body);
			gameObject.layer = LayerMask.NameToLayer("Grabbable");
		}
	}
}
