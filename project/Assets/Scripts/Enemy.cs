using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemy : MonoBehaviour {
	private GameObject m_player;
	public GameObject laser_prefab;
	public bool shoot;
	public int HP;
	public int speed;
	public int dmg;
	public int damage_body;
	public int damage_head;
	public int range;
	public int range_shoot;
	private float timer = -1;
	private float timer_audio = -1;
	private bool death_sound = true;

	public AudioClip walk;
	public AudioClip damage_audio;
	public AudioClip death;

	// Use this for initialization
	void Start ()
	{
		m_player = GameObject.Find("Character");

	}

	// Update is called once per frame
	void Update()
	{
		if (HP <= 0) {
			if (death_sound) {
				gameObject.GetComponent<AudioSource> ().PlayOneShot (death);
				death_sound = false;
			}
			die ();
		}

		if ((timer_audio!=-1) && (timer_audio <= Time.time)) {
			gameObject.GetComponent<AudioSource> ().clip = walk;
			gameObject.GetComponent<AudioSource> ().Play();
			timer_audio = -1;
		}
		Vector3 dir = m_player.transform.position - GetComponent<Rigidbody>().transform.position;
		dir.y = 0;
		if (dir.magnitude < range)
		{
			float mag = dir.magnitude;
			Vector3 tmp = dir;
			dir.Normalize();
			transform.Translate(dir * Time.deltaTime * (speed / 100));
			if (shoot)
			{
				if (dir.magnitude < range_shoot) {
					if (timer == -1) 
						timer = Time.time + 5;
					laser_prefab.transform.localScale = new Vector3 (laser_prefab.transform.localScale.x, mag*4, laser_prefab.transform.localScale.z);
					laser_prefab.transform.position = this.transform.position;

					Vector3 targetDir = m_player.transform.position - laser_prefab.transform.position;
					Vector3 newDir = Vector3.RotateTowards(laser_prefab.transform.forward, targetDir, 360, 0.0F);
					Debug.DrawRay(laser_prefab.transform.position, newDir, Color.red);
					Quaternion rot = Quaternion.LookRotation(newDir);
					laser_prefab.transform.rotation = rot;
					laser_prefab.transform.eulerAngles=new Vector3(laser_prefab.transform.eulerAngles.x+90, laser_prefab.transform.eulerAngles.y, laser_prefab.transform.eulerAngles.z);
					this.transform.eulerAngles=new Vector3(0, -laser_prefab.transform.eulerAngles.x+laser_prefab.transform.eulerAngles.y, 90);
					if (timer <= Time.time) {
						//m_player.GetComponent<CustomCharacterController> ().damage (dmg);
						timer = -1;
						//laser_prefab.GetComponentsInChildren<MeshRenderer> ().enabled = false;
					}
				} else {
					timer = -1;
					//laser_prefab.GetComponentsInChildren<MeshRenderer> ().enabled = false;
				}
			}
		}
	}

	public void damage(int value){
		HP -= value;
		gameObject.GetComponent<AudioSource> ().clip=null;
		if (HP > 0) {
			timer_audio = Time.time + damage_audio.length;
			gameObject.GetComponent<AudioSource> ().PlayOneShot (damage_audio);
		}
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
		if (timer_audio==-1)
			timer_audio = Time.time + death.length;
		if (timer_audio > Time.time)
			gameObject.GetComponent<Rigidbody> ().detectCollisions = false;
		else {
			timer_audio = -1;
			Destroy (gameObject);
		}
	}
}