using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
	
	public GameObject groundHitPrefab;
	public GameObject enemyHitPrefab;
	public GameObject fpsCamera;
	public float fireRate = 1.0f;
	
	// brukes av FPSController
	public Animator wpAnimator;
	
	// weapon array
	public int currentWeapon = 0;
	public GameObject shotgun;
	private GameObject[] wpArray = new GameObject[1];
	
	private float fireTimer = 0f;
	
	void Start() {
		
		wpArray[0] = shotgun;
		wpAnimator = wpArray[currentWeapon].GetComponent<Animator>();
	}
	
	void Update() {
		
		// be om at skyte animasjonen kan stoppe
		if (fireTimer <= (fireRate - 0.1f)) {
			wpAnimator.SetBool("fire", false);
		}
		
		// Hvis man kan skyte
		if (Input.GetKey(KeyCode.Mouse0) && fireTimer == 0f) {
			fireWeapon();
			wpAnimator.SetBool("fire", true);
			fireTimer = fireRate;
		}
		
		// teller nedover og sikrer at fireTimer ikke går under 0f
		if (fireTimer != 0f) {
			fireTimer -= Time.deltaTime;
			if (fireTimer < 0f) { 
				fireTimer = 0f;
			}
		}
		
		// Todo: Sjekk om man ønsker å bytte våpen, og så endre animator
    }

	private void fireWeapon() {
		
		RaycastHit hitInfo;
		Vector3 localForward = fpsCamera.transform.TransformDirection(Vector3.forward);
		
		// sjekker om man treffer noe, og får hitInfo definert
		if (Physics.Raycast(fpsCamera.transform.position, localForward, out hitInfo,
			100f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			
			bool didHit = (hitInfo.collider.tag == "Enemy") ? true : false;

			// lager et nytt objekt basert på prefab etter om man treffer fiende eller ikke
			Instantiate(didHit ? enemyHitPrefab : groundHitPrefab, hitInfo.point,
				Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
			
			if (didHit) { hitInfo.collider.SendMessage("takeDamage", 1); }
		}
	}
}
