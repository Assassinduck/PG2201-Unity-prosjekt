using UnityEngine;

public class itemPickup : MonoBehaviour {
	
	public int itemType;
	public int itemAmount;
    
	void OnTriggerEnter(Collider other) {
		
		if (other.name == "PlayerController") {

			Destroy(gameObject);
		}
	}
}
