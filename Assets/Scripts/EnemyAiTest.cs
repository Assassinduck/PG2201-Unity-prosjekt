using UnityEngine;

public class EnemyAiTest : MonoBehaviour {
	
	public Transform player;
	public float alertDistance;
	public float meleeDistance;
	public float movementSpeed;
	
	private float playerDistance;
	
	void Start() {
		
	}
	
    void Update() {
		
		playerDistance = Vector3.Distance(player.position, transform.position);
		
		if (playerDistance > alertDistance) { return; }
		
		if (playerDistance <= alertDistance) {
			
			Vector3 playerDirection = player.position - transform.position;
			playerDirection.y = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 5.0f * Time.deltaTime);
			
			if (playerDistance <= meleeDistance) {
				Debug.Log("Attacked");
			} else {
				transform.Translate(0, 0, movementSpeed * Time.deltaTime);
			}
		}
	}
}
