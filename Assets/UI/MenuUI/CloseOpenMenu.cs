using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloseOpenMenu : MonoBehaviour
{
    public InputActionReference escapeKey;
    public GameObject menuWindow;
    public InputActionAsset inputActions;

    private bool isMenuOpen = false;

    private void Start()
    {
        menuWindow.SetActive(false);
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
        Debug.Log("Escape pressed.");
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

        // Disable "Player" action map
        inputActions.FindActionMap("Player").Disable();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseMenu()
    {
        menuWindow.SetActive(false);

        // Enable "Player" action map
        inputActions.FindActionMap("Player").Enable();

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
