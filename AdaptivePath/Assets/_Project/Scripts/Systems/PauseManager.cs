using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject settingsMenu;

    private PlayerInputActions inputActions;
    private bool isPaused;

    void awake() 
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable() 
    {
       
        inputActions.Enable();

    }

    void OnDisable() 
    {

        inputActions.Disable();

    }
    void Start()
    {
        
    }

    
    void Update()
    {

        if (inputActions.Player.Pause.triggered)
        {

            if (isPaused) Resume();
            else Pause();

        }
        
    }

    public void Pause() 
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        CursorManager.Instance.UnlockCursor();

    }

    public void Resume() 
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        CursorManager.Instance.LockCursor();

    }

    public void OpenSettings() 
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
     public void CloseSettings() 
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
