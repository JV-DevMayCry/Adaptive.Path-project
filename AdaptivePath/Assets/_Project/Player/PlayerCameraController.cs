using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    public float mouseSensitivity = 200f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private float xRotation = 0f;

    private Transform playerBody;


    void Awake() {

        inputActions = new PlayerInputActions();
        playerBody = transform.parent;

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);

    }

    void OnEnable() {

        inputActions.Enable();
    }

    void OnDisable() { 

        inputActions.Disable(); 
    } 


    void Update() {    

        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void SetMouseSensitivity(float value)
    {

        mouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);

    }
}

    

