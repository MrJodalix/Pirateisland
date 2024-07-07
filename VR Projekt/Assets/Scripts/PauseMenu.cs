using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputActionAsset InputSystem;
    InputAction pauseGame;
    public GameObject PauseMenuUI;
    public GameObject leftHandRayIndicator;
    public GameObject rightHandRayIndicator;
    public GameObject endMenu;
    public GameObject controlMenu;

    public bool PauseMenuActive = true;

    void Start()
    {
        DisplayPauseUI();
        
        var actionMap = InputSystem.FindActionMap("XRI LeftHand Interaction");

        pauseGame = actionMap.FindAction("PauseMenu");

        pauseGame.performed += PauseButtonPressed;
        pauseGame.canceled += PauseButtonPressed;
        pauseGame.Enable();

    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        
        if(context.performed)
        {
            DisplayPauseUI();
        }
    }

    public void DisplayPauseUI()
    {   
        if(PauseMenuActive)
        {
            PauseMenuUI.SetActive(false);
            controlMenu.SetActive(false);
            PauseMenuActive = false;
            Time.timeScale = 1;
            leftHandRayIndicator.SetActive(false);
            rightHandRayIndicator.SetActive(false);
        }
        else if(!PauseMenuActive)
        {
            PauseMenuUI.SetActive(true);
            PauseMenuActive = true;
            Time.timeScale = 0;
            leftHandRayIndicator.SetActive(true);
            rightHandRayIndicator.SetActive(true);
        }
    }


    public void ContinueGame()
    {
        DisplayPauseUI();
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void StayInScene()
    {
        Time.timeScale = 1;
        endMenu.SetActive(false);
        leftHandRayIndicator.SetActive(false);
        rightHandRayIndicator.SetActive(false);
    }

    public void DisplayControlMenu()
    {
        PauseMenuUI.SetActive(false);
        PauseMenuActive = false;
        Time.timeScale = 0;
        leftHandRayIndicator.SetActive(true);
        rightHandRayIndicator.SetActive(true);
        controlMenu.SetActive(true);
    }

    public void ControlMenuBack()
    {
        controlMenu.SetActive(false);
        DisplayPauseUI();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
