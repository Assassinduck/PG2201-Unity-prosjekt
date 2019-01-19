using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
	
	public Transform prefab;
	public GameObject fpsCamera;
	public float fireRate = 0.0f;
	
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
		
		if (Input.GetKey(KeyCode.Mouse0) && fireTimer == 0f) {
			fireWeapon();
			wpAnimator.SetBool("fire", true);
			fireTimer = fireRate;
		}
		
		if (fireTimer < (fireRate / 2)) {
			wpAnimator.SetBool("fire", false);
		}
		
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
		
		if (Physics.Raycast(fpsCamera.transform.position, localForward, out hitInfo,
			100f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
			
			Instantiate(prefab, hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
		}
	}
}
