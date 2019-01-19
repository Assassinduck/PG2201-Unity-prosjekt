using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour {
	
	public string moveForward = "w";
	public string moveBackward = "s";
	public string moveLeft = "a";
	public string moveRight = "d";
	
	public Text speedCount;
	public float speedCap = 5f;
	public float moveAcceleration = 2.0f;
	public float groundFriction = 0.05f;
	public float groundCheckDistance = 0.1f;
	
	private Rigidbody rb;
	private CapsuleCollider capsule;
	private Vector3 moveDir;
	private WeaponController wpCont;
	private bool isGrounded;
	private bool prevGrounded;
	
    void Start() {
		
        rb = GetComponent<Rigidbody>();
		capsule = GetComponent<CapsuleCollider>();
		wpCont = GetComponent<WeaponController>();
    }

    void Update() {

		float speed = rb.velocity.magnitude;
		speed = Mathf.Floor(speed);
		speedCount.text = speed.ToString();
    }
	
	void FixedUpdate() {
		
		Vector3 newVelocity = rb.velocity;
		moveDir = getMoveDir();
		isGrounded = isOnGround();
		
		if (prevGrounded) { 
			
			newVelocity = applyFriction(newVelocity, groundFriction);
			if (rb.velocity.magnitude > 1.0f) { wpCont.wpAnimator.SetInteger("state", 1); }
			else { wpCont.wpAnimator.SetInteger("state", 0); }

		} else if (isGrounded) {
			
			wpCont.wpAnimator.SetInteger("state", 3);
		}

		if (isGrounded) { 
			newVelocity = accelerateVelocity(moveDir, newVelocity, moveAcceleration, speedCap);
	   	} else {
			
			Vector3 localVelocity = transform.InverseTransformDirection(newVelocity);
			
			if ((!Input.GetKey(moveForward) && !Input.GetKey(moveBackward)) && (localVelocity.z > speedCap)) {
				Vector2 horVelocity = new Vector2(newVelocity.x, newVelocity.z);
				float vertVelocity = newVelocity.y;
				newVelocity = transform.TransformDirection(horVelocity.magnitude * Vector3.forward);
				newVelocity.y = vertVelocity;
			}
			
			wpCont.wpAnimator.SetInteger("state", 0);
		}
		
		if (Input.GetKey(KeyCode.Space) && isGrounded) {
		   	newVelocity.y = 5.0f;
			prevGrounded = false;
			wpCont.wpAnimator.SetInteger("state", 2);
	   	} else {
			prevGrounded = isGrounded ? true : false;
		}

		if (wpCont.wpAnimator.GetBool("fire")) {
			wpCont.wpAnimator.SetInteger("state", 4);
		}
		
		rb.velocity = newVelocity;
	}

	private Vector3 accelerateVelocity(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity) {
		
		float projVel, addspeed;
		projVel = Vector3.Dot(prevVelocity, accelDir);
		addspeed = max_velocity - projVel;
		
		if (addspeed <= 0) {
			return prevVelocity;
		}
		
		if (accelerate > addspeed) {
			accelerate = addspeed;
		}

		return prevVelocity + accelerate * accelDir;
	}
	
	private Vector3 getMoveDir() {
		
		Vector3 moveDir = new Vector3(0.0f, 0.0f, 0.0f);
		
		if (Input.GetKey(moveForward)) {
			moveDir.z += 1.0f;
		}
		if (Input.GetKey(moveBackward)) {
			moveDir.z -= 1.0f;
		}
		if (Input.GetKey(moveLeft)) {
			moveDir.x -= 1.0f;
		}
		if (Input.GetKey(moveRight)) {
			moveDir.x += 1.0f;
		}
		
		return transform.TransformDirection(moveDir.normalized);
	}

	private Vector3 applyFriction(Vector3 prevVelocity, float friction) {
		
		float speed = prevVelocity.magnitude;
			
		if (speed == 0 || friction > 0.9f) {
			return Vector3.zero;
		}
		
		float drop = speed * friction;
		return prevVelocity *= (speed - drop) / speed;
	}

	private bool isOnGround() {
		
		RaycastHit hitInfo;
		
		if (Physics.SphereCast(transform.position, capsule.radius - 0.2f, Vector3.down, out hitInfo,
			((capsule.height / 2f) - capsule.radius) + groundCheckDistance + 0.2f,
		   		Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			
			return true;
		} else { return false; }
	}
}
