using UnityEngine;

public class EnemyAiTest : MonoBehaviour {
	
	public Transform player;
	public float alertDistance;
	public float meleeDistance;
	public float movementSpeed;
	
	private float playerDistance;
	private int health = 3;
	
	void Start() {
		
	}
	
    void Update() {
		
		playerDistance = Vector3.Distance(player.position, transform.position);
		
		// hvis ikke spiller er nært nok, skal ingenting skje
		if (playerDistance > alertDistance) { return; }
		
		// nærme nok for at de skal gå mot spiller
		if (playerDistance <= alertDistance) {
			
			Vector3 playerDirection = player.position - transform.position;
			playerDirection.y = 0;
			
			// sakte rotasjon mot spiller
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 5.0f * Time.deltaTime);
			
			// velge mellom angrip eller å gå
			if (playerDistance <= meleeDistance) {
				Debug.Log("Attacked");
			} else {
				transform.Translate(0, 0, movementSpeed * Time.deltaTime);
			}
		}
	}
	
	void takeDamage(int amount) {
		
		health -= amount;
		if (health <= 0) { Destroy(gameObject); }
	}
}
