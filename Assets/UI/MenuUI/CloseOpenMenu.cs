using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CloseOpenMenu : MonoBehaviour
{
    public InputActionReference escapeKey;
    public GameObject menuWindow, volumeWindow, controlsWindow, hubWindow;
    public InputActionAsset inputActions;
    public PlayerInput playerInput;
    public Aim aim;

    private bool isMenuOpen = false;

    private void Start()
    {
        menuWindow.SetActive(false);
        hubWindow.SetActive(false);
    }

    private void OnEnable()
    {
        escapeKey.action.Enable();
        escapeKey.action.started += OnEscapePressed;
    }

    private void OnDisable()
    {
        escapeKey.action.Disable();
        escapeKey.action.started -= OnEscapePressed;
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        //Debug.Log("Escape pressed.");
        if (menuWindow.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        menuWindow.SetActive(true);
        hubWindow.SetActive(true);
        volumeWindow.SetActive(false);
        controlsWindow.SetActive(false);

        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseMenu()
    {
        menuWindow.SetActive(false);
        hubWindow.SetActive(false);
        volumeWindow.SetActive(false);
        controlsWindow.SetActive(false);
        PassSaveFiles.Save(SceneManager.GetActiveScene().name);

        aim.enabled = true;
        playerInput.actions.FindActionMap("Player").Enable();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
