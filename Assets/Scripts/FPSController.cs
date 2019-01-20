using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour {
	
	public string moveForward = "w";
	public string moveBackward = "s";
	public string moveLeft = "a";
	public string moveRight = "d";
	
	public Text speedCount;
	
	// viktige variabler for movement
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
		
		// henter components
        rb = GetComponent<Rigidbody>();
		capsule = GetComponent<CapsuleCollider>();
		wpCont = GetComponent<WeaponController>();
    }

    void Update() {
		
		// gjør sånn at man kan se hastigheten, rundet nedover
		float speed = rb.velocity.magnitude;
		speed = Mathf.Floor(speed);
		speedCount.text = speed.ToString();
    }
	
	void FixedUpdate() {
		
		Vector3 newVelocity = rb.velocity;
		moveDir = getMoveDir();
		isGrounded = isOnGround();
		
		// hvis man har vært på bakken i forrige frame
		if (prevGrounded) { 
			
			// reduser fart med friksjon, og velg mellom idle og walk animation
			newVelocity = applyFriction(newVelocity, groundFriction);
			if (rb.velocity.magnitude > 1.0f) { wpCont.wpAnimator.SetInteger("state", 1); }
			else { wpCont.wpAnimator.SetInteger("state", 0); }

		} else if (isGrounded) {
			
			// gjør landings animasjon hvis du landet denne framen	
			wpCont.wpAnimator.SetInteger("state", 3);
		}
		
		if (isGrounded) {
			// kan akselerere hvis man er på bakken
			newVelocity = accelerateVelocity(moveDir, newVelocity, moveAcceleration, speedCap);
	   	} else {
			
			Vector3 localVelocity = transform.InverseTransformDirection(newVelocity);
			
			// air control hvis man har fart i lokal z-akse og ikke er på bakken
			if ((!Input.GetKey(moveForward) && !Input.GetKey(moveBackward)) && (localVelocity.z > speedCap)) {
				Vector2 horVelocity = new Vector2(newVelocity.x, newVelocity.z);
				float vertVelocity = newVelocity.y;
				newVelocity = transform.TransformDirection(horVelocity.magnitude * Vector3.forward);
				newVelocity.y = vertVelocity;
			}
			
			// spill av idle animasjon i lufta (fungerer fint nok)
			wpCont.wpAnimator.SetInteger("state", 0);
		}
		
		// hvis du holder nede space og er på bakken
		if (Input.GetKey(KeyCode.Space) && isGrounded) {
		   	newVelocity.y = 5.0f;
			prevGrounded = false;
			wpCont.wpAnimator.SetInteger("state", 2);
	   	} else {
			// hvis man prøver å hoppe på første frame skjer ikke dette og da blir det
			// ikke friksjon
			prevGrounded = isGrounded ? true : false;
		}
		
		// prioriter "fire" animasjonen
		if (wpCont.wpAnimator.GetBool("fire")) {
			wpCont.wpAnimator.SetInteger("state", 4);
		}
		
		// setter rigidbody hastighet til en oppdatert hastighet
		rb.velocity = newVelocity;
	}
	
	// All akselerasjon i bevegelse skjer her, og er lagd for å få quake style strafejump og bunnyhopping
	// 
	//	accelDir 		- retning localt man ønsker å akselerere(se getMoveDir())
	//	prevVelocity 	- nåværende fart fra rigidbody
	//	accelerate 		- faktor for akselerasjon
	//	max_velocity 	- speedcap for vanlig movement, kan brytes
	//
	private Vector3 accelerateVelocity(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity) {
		
		float projVel, addspeed;
		
		// vektor projeksjon av fartsvektoren på akselerasjons vektoren(bad read of velocity)
		projVel = Vector3.Dot(prevVelocity, accelDir);
		addspeed = max_velocity - projVel; // forskjellen mellom speedcap og hva man """tror""" er farten
		
		// ikke akselerer hvis man er over speedcap
		if (addspeed <= 0) {
			return prevVelocity;
		}
		
		// hvis man kan akselerere litt for å nå speedcap
		if (accelerate > addspeed) {
			accelerate = addspeed;
		}
		
		// plus sammen forrige farts vektor med en akselerasjonsvektor
		return prevVelocity + accelerate * accelDir;
	}
	
	// sjekker input og returnerer en normalisert vektor for retning
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
	
	// returnerer fartsvektoren med friksjon påvirkning
	private Vector3 applyFriction(Vector3 prevVelocity, float friction) {
		
		float speed = prevVelocity.magnitude;
		
		// farten er null hvis friksjonen er for høy eller man ikke har fart fra før
		if (speed == 0 || friction > 0.9f) {
			return Vector3.zero;
		}
		
		// reduserer med friksjonsfaktoren
		float drop = speed * friction;
		return prevVelocity *= (speed - drop) / speed; // ganger med fartsendring delt på fart
	}
	
	// bruker spherecasting til å se om man er på bakken
	private bool isOnGround() {
		
		RaycastHit hitInfo;
		
		if (Physics.SphereCast(transform.position, capsule.radius - 0.2f, Vector3.down, out hitInfo,
			((capsule.height / 2f) - capsule.radius) + groundCheckDistance + 0.2f,
		   		Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			
			return true;
		} else { return false; }
	}
}
