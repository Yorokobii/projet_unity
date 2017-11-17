using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterController : MonoBehaviour {

	public float speed = 10.0f;
	public float RunMultiplier = 2.0f;
	public KeyCode RunKey = KeyCode.LeftShift;
	public float JumpForce = 30f;

	public Camera cam;
	public GameObject snowball_fab;
	public float throw_speed = 800.0f;
	public float max_grab_distance=6f;
	private float grab_distance=0;
	private GameObject grabbed_object = null;

	private Rigidbody m_RigidBody;
	private CapsuleCollider m_Capsule;
	public MouseLook mouseLook = new MouseLook();
	private float m_YRotation;
	private Vector3 m_GroundContactNormal;
	private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;



	// Use this for initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();
		mouseLook.Init (transform, cam.transform);
	}
	
	// Update is called once per frame
	void Update () {

		RotateView ();

		//fire balls
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			if (!grabbed_object)
				FireSnowBall ();
			else
				ThrowObject ();
		}

		//grab object
		if (CrossPlatformInputManager.GetButtonDown("Fire2"))
		{
			if (!grabbed_object)
				GrabObject ();
			else
				DropObject ();
		}

		if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
		{
			m_Jump = true;
		}

		if(grabbed_object){

		}
		else
			grab_distance=0;



		if (grabbed_object) {

			float tmp = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel")*10;
			if (tmp != 0)
				grab_distance += tmp;
			if(grab_distance<0)
				grab_distance=0;
			if(grab_distance>5)
				grab_distance=5;

			grabbed_object.transform.position = cam.transform.position;
			grabbed_object.transform.Translate (new Vector3(0.0f, -0.5f, 1.2f+grab_distance));
			grabbed_object.transform.rotation = cam.transform.rotation;

		}
		else
			grab_distance=0;
	}


	/// <summary>
	///MOUVEMENTS
	/// </summary>

	private void FixedUpdate()
	{
		GroundCheck();
		Vector2 input = GetInput();

		if ((Mathf.Abs (input.x) > 0f || Mathf.Abs (input.y) > 0f)) {
			// always move along the camera forward as it is the direction that it being aimed at
			Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
			desiredMove = Vector3.ProjectOnPlane (desiredMove, m_GroundContactNormal).normalized;

			desiredMove.x = desiredMove.x;
			desiredMove.z = desiredMove.z;
			desiredMove.y = desiredMove.y;
			if(Input.GetKey(RunKey)) desiredMove*=RunMultiplier;
			m_RigidBody.transform.Translate (desiredMove);
		}

		if (m_IsGrounded)
		{
			if (m_Jump)
			{
				m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
				m_RigidBody.AddForce(new Vector3(0f, JumpForce, 0f), ForceMode.Impulse);
				m_Jumping = true;
			}
		}
		m_Jump = false;

	}

	private Vector2 GetInput()
	{

		Vector2 input = new Vector2
		{
			x = CrossPlatformInputManager.GetAxis("Horizontal"),
			y = CrossPlatformInputManager.GetAxis("Vertical")
		};
		return input;
	}


	private void RotateView()
	{
		//avoids the mouse looking if the game is effectively paused
		if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

		// get the rotation before it's changed
		float oldYRotation = transform.eulerAngles.y;

		mouseLook.LookRotation (transform, cam.transform);

		// Rotate the rigidbody velocity to match the new direction that the character is looking
		Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
		m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
	}

	/// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
	private void GroundCheck()
	{
		m_PreviouslyGrounded = m_IsGrounded;
		RaycastHit hitInfo;
		if (Physics.SphereCast(transform.position, m_Capsule.radius * 0.9f, Vector3.down, out hitInfo,
			((m_Capsule.height/2f) - m_Capsule.radius) + 0.01f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			m_IsGrounded = true;
			m_GroundContactNormal = hitInfo.normal;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundContactNormal = Vector3.up;
		}
		if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
		{
			m_Jumping = false;
		}
	}


	/// <summary>
	///
	/// </summary>


	void FireSnowBall () {
		GameObject snowball = (GameObject) Instantiate(snowball_fab, transform.position, transform.rotation);
		snowball.GetComponent<Rigidbody>().position = cam.transform.position;
		snowball.GetComponent<Rigidbody>().velocity = cam.transform.forward * 10;
	}



	void GrabObject(){
		RaycastHit hit;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, max_grab_distance, LayerMask.GetMask ("Grabbable"))) {
			grabbed_object = hit.collider.gameObject;
			grabbed_object.GetComponent<Collider> ().enabled = false;
			grabbed_object.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

		}

	}

	void ThrowObject(){
		grabbed_object.GetComponent<Collider> ().enabled = true;

		grabbed_object.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		grabbed_object.GetComponent<Rigidbody> ().AddForce (cam.transform.forward*throw_speed);
		grabbed_object = null;
	}

	void DropObject(){
		grabbed_object.GetComponent<Collider> ().enabled = true;
		grabbed_object.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

		grabbed_object = null;
	}




}
