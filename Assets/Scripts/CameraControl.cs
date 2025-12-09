using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float sensitivity = 100f;
    public Transform body;
    public Transform camera;


    private float xRotation;

    private InputSystemActions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        inputActions = new InputSystemActions();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 mouse = inputActions.Player.Look.ReadValue<Vector2>() * Time.deltaTime * sensitivity;

        xRotation -= mouse.y;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        
        camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        body.Rotate(Vector3.up * mouse.x);
        
        
    }
}
