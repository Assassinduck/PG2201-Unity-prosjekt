using UnityEngine;

public class SelfDestroy : MonoBehaviour {
	
	public float destroyTime = 5.0f;
	
    void Start() {
        Destroy(gameObject, destroyTime);
    }
}
