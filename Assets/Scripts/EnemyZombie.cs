using UnityEngine;

public class EnemyZombie : MonoBehaviour {
	
	public Transform player;
	public GameObject zombie;
	public float alertDistance;
	public float meleeDistance;
	public float movementSpeed;
	public int health = 3;
	
	private float playerDistance;
	private Animator zombAnimator;
	
	void Start() {
		
		zombAnimator = zombie.GetComponent<Animator>();
	}
	
    void Update() {
		
		if (zombAnimator.GetBool("dead")) {
			return;
		}

		playerDistance = Vector3.Distance(player.position, transform.position);
		
		// hvis ikke spiller er nært nok, skal idle anim spille
		if (playerDistance > alertDistance) { 
			zombAnimator.SetInteger("state", 0);
			return;
	   	}
		
		// nærme nok for at de skal gå mot spiller
		if (playerDistance <= alertDistance) {
			
			Vector3 playerDirection = player.position - transform.position;
			playerDirection.y = 0;
			
			// sakte rotasjon mot spiller
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 5.0f * Time.deltaTime);
			
			// velge mellom angrip eller å gå
			if (playerDistance <= meleeDistance) {
				zombAnimator.SetInteger("state", 3);
			} else {
				transform.Translate(0, 0, movementSpeed * Time.deltaTime);
				zombAnimator.SetInteger("state", 1);
			}
		}
	}
	
	void takeDamage(int amount) {
		
		health -= amount;
		zombAnimator.SetInteger("state", 2);
		if (health <= 0) { 
			zombAnimator.SetBool("dead", true);
			GetComponent<CapsuleCollider>().enabled = false;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
