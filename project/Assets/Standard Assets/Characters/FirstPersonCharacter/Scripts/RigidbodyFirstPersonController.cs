using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float Speed = 8.0f;   // Speed when walking forward
            public float RunMultiplier = 2.0f;   // Speed when sprinting
	        public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				CurrentTargetSpeed = Speed;
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
		public Rigidbody snowball_fab;
		public int throw_speed;
        public float max_grab_distance=6f;
        private float grab_distance=0;
		private GameObject grabbed_object = null;
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
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
        }


        private void Update()
        {
            RotateView();

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


			///
			///ADDED
			///

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

			///
			///
        }




		/// <summary>
		/// ADDED PART
		/// </summary>
		void FireSnowBall () {
			Rigidbody snowball = (Rigidbody) Instantiate(snowball_fab, transform.position, transform.rotation);
			snowball.position = cam.transform.position;
			snowball.velocity = cam.transform.forward * 10;
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


		/// <summary>
		/// </summary>








        private void FixedUpdate()
        {
            GroundCheck();
            Vector2 input = GetInput();

			if ((Mathf.Abs (input.x) > 0f || Mathf.Abs (input.y) > 0f)) {
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
				desiredMove = Vector3.ProjectOnPlane (desiredMove, m_GroundContactNormal).normalized;

				desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
				desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
				desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
				if (m_RigidBody.velocity.sqrMagnitude <
				    (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed)) {
					m_RigidBody.AddForce (desiredMove * SlopeMultiplier (), ForceMode.Impulse);
				}
			} else {
				if (m_Jumping) {
					m_RigidBody.velocity = new Vector3(0, m_RigidBody.velocity.y, 0);
					
				}
			}

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < 0f && Mathf.Abs(input.y) < 0f)
                {
					m_RigidBody.velocity = Vector3.zero;
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
				if (m_PreviouslyGrounded && !m_Jumping) {
					StickToGroundHelper ();
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
    }
}
