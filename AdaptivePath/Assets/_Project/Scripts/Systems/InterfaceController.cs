using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceController : MonoBehaviour
{

    public GameObject inventoryPanel;

    private bool isInventoryOpen = false;
    private PlayerInputActions inputActions;

    
    void Awake()
    {

        inputActions = new PlayerInputActions();
        
    }

    void OnEnable() 
        {
            inputActions.Enable();
            inputActions.UI.Inventory.performed += onInventoryPressed;
        }
    
    void OnDisable() 
        {
            inputActions.UI.Inventory.performed -= onInventoryPressed;
            inputActions.Disable();
        }

    public void onInventoryPressed(InputAction.CallbackContext context) 
    {
        ToggleInventory();
    }

    void ToggleInventory() 
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;
    }
    // Update is called once per frame
    void Update()
    {
        
        

    }
}
