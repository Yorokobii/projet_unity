using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableBehavior : MonoBehaviour {

	private Vector3 m_reset_position;
	private Quaternion m_reset_rotation;
	public int damage = 1;

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
		if(collision.relativeVelocity.magnitude >= 5.0f){
			if(collision.gameObject.CompareTag ("Body")){
				collision.transform.parent.gameObject.GetComponent<Enemy> ().damage(collision.transform.parent.gameObject.GetComponent<Enemy> ().damage_head);
			} else if(collision.gameObject.CompareTag ("Head")){
				collision.transform.parent.gameObject.GetComponent<Enemy> ().damage(collision.transform.parent.gameObject.GetComponent<Enemy> ().damage_body);
			}
		}
	}
}
