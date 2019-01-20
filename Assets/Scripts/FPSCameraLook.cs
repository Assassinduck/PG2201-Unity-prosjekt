using UnityEngine;

public class FPSCameraLook : MonoBehaviour {
	
	public float sensitivityX = 1.0f;
	public float sensitivityY = 1.0f;
	public GameObject playerController;
	
	private float yRotation = 0.0f;
	
    void Update() {
		
		float rotateAmountX = Input.GetAxis("Mouse X") * sensitivityX;
		float rotateAmountY = Input.GetAxis("Mouse Y") * sensitivityY;
		
		// roterer PlayerController horisontalt og seg selv vertikalt
		// stopper vertikalt med bruk av clamp
        playerController.transform.Rotate(rotateAmountX * Vector3.up, Space.Self);
		yRotation -= rotateAmountY;
		yRotation = Mathf.Clamp(yRotation, -80f, 80f);
		transform.localRotation = Quaternion.Euler(yRotation, 0.0f, 0.0f);
    }
}
