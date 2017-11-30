using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof (Rigidbody))]
	[RequireComponent(typeof (CapsuleCollider))]
	public class CustomCharacterController : MonoBehaviour
	{
		[Serializable]
		public class MovementSettings
		{
			public float Speed = 8.0f;
			public float SpeedBonus = 0f;
			public float SpeedTerrain = 0f;
			public float RunMultiplier = 2.0f;   // Speed when sprinting
			public KeyCode RunKey = KeyCode.LeftShift;
			public float JumpForce = 30f;
			public float JumpForceBonus = 0f;
			public float JumpForceTerrain = 0f;
			public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
			[HideInInspector] public float CurrentTargetSpeed = 8f;

			#if !MOBILE_INPUT
			private bool m_Running;
			#endif

			public void UpdateDesiredTargetSpeed(Vector2 input)
			{
				if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = Speed+SpeedBonus+SpeedTerrain;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = Speed+SpeedBonus+SpeedTerrain;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = Speed+SpeedBonus+SpeedTerrain;
				}
				#if !MOBILE_INPUT
				if (Input.GetKey(RunKey))
				{
					CurrentTargetSpeed *= RunMultiplier;
					m_Running = true;
				}
				else
				{
					m_Running = false;
				}
				#endif
			}

			#if !MOBILE_INPUT
			public bool Running
			{
				get { return m_Running; }
			}
			#endif
		}


		[Serializable]
		public class AdvancedSettings
		{
			public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
			public float stickToGroundHelperDistance = 0.5f; // stops the character
			public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
			public bool airControl; // can the user control the direction that is being moved in the air
			[Tooltip("set it to 0.1 or more if you get stuck in wall")]
			public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
		}


		public Camera cam;
		public GameObject snowball_prefab;
		public float throw_speed = 800.0f;
		public float max_grab_distance = 6.0f;
		private float m_grab_distance = 0.0f;
		private GameObject m_grabbed_object = null;

		public int HP;
		public float FireVelocity = 10f;
		public int AmmoMax = 10;
		private int m_current_ammo;
		public float RateOfFire = 1f;
		private float m_rate_of_fire_timer = 0f;

		public GameObject object_snap_point;
		private Vector3 snap_position;
		public KeyCode RotateObjectKey = KeyCode.R;
		private Vector3 m_reset_position;
		private Quaternion m_reset_rotation;

		public MovementSettings movementSettings = new MovementSettings();
		public MouseLook mouseLook = new MouseLook();
		public AdvancedSettings advancedSettings = new AdvancedSettings();


		private Rigidbody m_RigidBody;
		private CapsuleCollider m_Capsule;
		private float m_YRotation;
		private Vector3 m_GroundContactNormal;
		private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


		public Vector3 Velocity
		{
			get { return m_RigidBody.velocity; }
		}

		public bool Grounded
		{
			get { return m_IsGrounded; }
		}

		public bool Jumping
		{
			get { return m_Jumping; }
		}

		public bool Running
		{
			get
			{
				#if !MOBILE_INPUT
				return movementSettings.Running;
				#else
				return false;
				#endif
			}
		}


		private void Start()
		{
			m_current_ammo = AmmoMax;
			m_rate_of_fire_timer = 0;
			snap_position = object_snap_point.transform.localPosition;
			m_reset_position = transform.position;
			m_reset_rotation = transform.rotation;

			Physics.IgnoreLayerCollision (8, 9);

			m_RigidBody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			mouseLook.Init (transform, cam.transform);
		}


		private void Update()
		{

			if (!m_grabbed_object || !Input.GetKey(RotateObjectKey))
				RotateView ();
			else{

				float zRot = CrossPlatformInputManager.GetAxis("Mouse X");
				float xRot = CrossPlatformInputManager.GetAxis("Mouse Y");

				object_snap_point.transform.Rotate(new Vector3 (xRot, 0, zRot));

			}

			//FireBalls
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				if (!m_grabbed_object)
					FireSnowBall ();
				else
					ThrowObject ();
			}

			//grab object
			if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
				if (!m_grabbed_object)
					GrabObject ();
				else
					DropObject ();
			}

			if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
			{
				m_Jump = true;
			}

			if (m_grabbed_object) {
				float tmp = CrossPlatformInputManager.GetAxis ("Mouse ScrollWheel") * 10;
				if (tmp != 0)
					m_grab_distance += tmp;
				if (m_grab_distance < 0)
					m_grab_distance = 0;
				if (m_grab_distance > 5)
					m_grab_distance = 5;

				object_snap_point.transform.localPosition = new Vector3 (snap_position.x, snap_position.y, snap_position.z+m_grab_distance);
				m_grabbed_object.transform.position = object_snap_point.transform.position;
				m_grabbed_object.transform.rotation = object_snap_point.transform.rotation;
			} else
				m_grab_distance = 0;
		}

		/// <summary>
		/// MOUVEMENTS
		/// </summary>

		private void FixedUpdate()
		{
			GroundCheck();
			Vector2 input = GetInput();

			if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
			{
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
				desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

				desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
				desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
				desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
				if (m_RigidBody.velocity.sqrMagnitude <
					(movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
				{
					m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
				}
			}

			if (m_IsGrounded)
			{
				m_RigidBody.drag = 5f;

				if (m_Jump)
				{
					m_RigidBody.drag = 0f;
					m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
					m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce+movementSettings.JumpForceBonus+movementSettings.JumpForceTerrain, 0f), ForceMode.Impulse);
					m_Jumping = true;
				}

				if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
				{
					m_RigidBody.Sleep();
				}
			}
			else
			{
				m_RigidBody.drag = 0f;
				if (m_PreviouslyGrounded && !m_Jumping)
				{
					StickToGroundHelper();
				}
			}
			m_Jump = false;
		}


		private float SlopeMultiplier()
		{
			float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
			return movementSettings.SlopeCurveModifier.Evaluate(angle);
		}


		private void StickToGroundHelper()
		{
			RaycastHit hitInfo;
			if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				((m_Capsule.height/2f) - m_Capsule.radius) +
				advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
				{
					m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
				}
			}
		}


		private Vector2 GetInput()
		{

			Vector2 input = new Vector2
			{
				x = CrossPlatformInputManager.GetAxis("Horizontal"),
				y = CrossPlatformInputManager.GetAxis("Vertical")
			};
			movementSettings.UpdateDesiredTargetSpeed(input);
			return input;
		}


		private void RotateView()
		{
			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

			// get the rotation before it's changed
			float oldYRotation = transform.eulerAngles.y;

			mouseLook.LookRotation (transform, cam.transform);

			if (m_IsGrounded || advancedSettings.airControl)
			{
				// Rotate the rigidbody velocity to match the new direction that the character is looking
				Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
				m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
			}
		}

		/// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
		private void GroundCheck()
		{
			m_PreviouslyGrounded = m_IsGrounded;
			RaycastHit hitInfo;
			if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
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
		/// ADDED BY GUILLAUME BOEHM
		/// </summary>

		void FireSnowBall()
		{
			if (GetAmmo() > 0 && Time.time > m_rate_of_fire_timer)
			{
				m_rate_of_fire_timer = Time.time + RateOfFire;
				GameObject snowball = (GameObject) Instantiate(snowball_prefab, transform.position, transform.rotation);
				snowball.transform.position = cam.transform.position;
				snowball.GetComponent<Rigidbody>().velocity = cam.transform.forward * FireVelocity;
				m_current_ammo--;
				Debug.Log(m_current_ammo.ToString());
			}
		}

		void GrabObject(){
			SetAmmo(AmmoMax);
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, max_grab_distance, LayerMask.GetMask ("Grabbable"))) {
				m_grabbed_object = hit.collider.GetComponentInParent<Rigidbody>().gameObject;
				foreach(Collider c in m_grabbed_object.GetComponentInParent<Rigidbody>().gameObject.GetComponentsInChildren<Collider>() ){
					c.enabled = false;
				}
				m_grabbed_object.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				object_snap_point.transform.localRotation = m_grabbed_object.transform.rotation;
			}
		}

		void ThrowObject(){
			object_snap_point.transform.localPosition = new Vector3 (snap_position.x, snap_position.y, snap_position.z);
			m_grabbed_object.transform.position = object_snap_point.transform.position;

			foreach(Collider c in m_grabbed_object.GetComponentInParent<Rigidbody>().gameObject.GetComponentsInChildren<Collider>() ){
				c.enabled = true;
			}

			m_grabbed_object.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			m_grabbed_object.GetComponentInParent<Rigidbody> ().AddForce (cam.transform.forward * throw_speed);

			m_grabbed_object.layer = 0;
			
			m_grabbed_object = null;
			object_snap_point.transform.localRotation = Quaternion.identity;
		}

		void DropObject(){
			foreach(Collider c in m_grabbed_object.GetComponentInParent<Rigidbody>().gameObject.GetComponentsInChildren<Collider>() ){
				c.enabled = true;
			}
			m_grabbed_object.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.None;

			m_grabbed_object = null;
			object_snap_point.transform.localRotation = Quaternion.identity;
		}

		public void respawn(){
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			transform.position = m_reset_position;
			transform.rotation = m_reset_rotation;
		}

		public void damage(int value){
			HP -= value;
			if (HP <= 0)
				Die ();
		}

		private void Die(){
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

		public void KnockBack(Vector3 vel){
			GetComponent<Rigidbody> ().AddForce (vel, ForceMode.Impulse);
		}

		public int GetAmmo()
		{
			return m_current_ammo;
		}

		public void SetAmmo(int value)
		{
			m_current_ammo = value >= AmmoMax ? AmmoMax : value;
		}
		
		public void AddAmmo(int value)
		{
			value = m_current_ammo + value;
			m_current_ammo = value >= AmmoMax ? AmmoMax : value;
		}
		
		

		// END ADDED
	}
}
