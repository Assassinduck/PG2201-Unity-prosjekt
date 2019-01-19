using UnityEngine;

public class GUIController : MonoBehaviour {
	
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
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			screenMode = Mathf.Abs(screenMode - 1);
			bool isInGame = (screenMode == 0) ? true : false;
			activeScripts(isInGame);
			for (int i = 1; i < screenModeObj.Length; i++) {
				screenModeObj[i - 1].SetActive((screenMode == i) ? true : false);
			}
		}
	}
	
	private void setCursorLock(bool bLock) {
		
		if (bLock) { Cursor.lockState = CursorLockMode.Locked; }
		else { Cursor.lockState = CursorLockMode.None; }
		Cursor.visible = !bLock;
		mouseLook.enabled = bLock;
	}
	
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
