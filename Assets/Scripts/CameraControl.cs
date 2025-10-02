using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 100f;
    public Transform body;
    public Transform camera;


    private float xRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        
        camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        body.Rotate(Vector3.up * mouseX);
        
        
    }
}
