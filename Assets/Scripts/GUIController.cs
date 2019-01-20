using UnityEngine;

public class GUIController : MonoBehaviour {
	
	// bruker et array som oversikt over de forskjellige menyene
	// arrayet består av de public objektene
	public int screenMode = 0;
	public GameObject pauseCan;
	public GameObject optionsCan;
	public GameObject keybindCan;
	private GameObject[] screenModeObj = new GameObject[3];

	public GameObject fpsCamera;
	public GameObject player;
	private FPSController fpsCont;
	private FPSCameraLook mouseLook;
	private WeaponController wpCont;

	void Start() {
		
		fpsCont = player.GetComponent<FPSController>();
		mouseLook = fpsCamera.GetComponent<FPSCameraLook>();
		wpCont = player.GetComponent<WeaponController>();

		setCursorLock(true);
		
		screenModeObj[0] = pauseCan;
		screenModeObj[1] = optionsCan;
		screenModeObj[2] = keybindCan;
	}
	
	void Update() {
		
		// blar bakover i menyen hvis du trykker esc
		// og kjører metoden som gjør at ting stopper riktig
		if (Input.GetKeyDown(KeyCode.Escape)) {
			
			screenMode = Mathf.Abs(screenMode - 1);
			bool isInGame = (screenMode == 0) ? true : false;
			activeScripts(isInGame);
			
			for (int i = 1; i < screenModeObj.Length; i++) {
				screenModeObj[i - 1].SetActive((screenMode == i) ? true : false);
			}
		}
	}
	
	// gjemmer mus og låser den på midten
	private void setCursorLock(bool bLock) {
		
		if (bLock) { Cursor.lockState = CursorLockMode.Locked; }
		else { Cursor.lockState = CursorLockMode.None; }
		Cursor.visible = !bLock;
		mouseLook.enabled = bLock;
	}
	
	// setter om scripts og annet components skal være aktive
	private void activeScripts(bool setActive) {
		
		fpsCont.enabled = setActive;
		setCursorLock(setActive);
		wpCont.enabled = setActive;
		Time.timeScale = setActive ? 1.0f : 0.0f;
	}
	
	public void exitGame() {
		Application.Quit();
	}
}
