using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloseTabX : MonoBehaviour
{
    public GameObject tab;
    public PlayerInput playerInput;
    public Aim aim;
    public void CloseTab()
    {
        tab.SetActive(false);

        aim.enabled = true;
        playerInput.actions.FindActionMap("Player").Enable();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
