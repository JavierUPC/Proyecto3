using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTabX : MonoBehaviour
{
    public GameObject tab;
    public void CloseTab()
    {
        tab.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
