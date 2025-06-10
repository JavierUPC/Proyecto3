using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public float startTextAfter = 2, startTutorialAfter = 2;
    public GameObject Text0, Text1, Text2, Text3, TutorialScreen;
    public PlayerInput playerInput;
    public Aim aim;
    public SpawnerMosca flySpawner;
    private bool started = false, once = false, first = false, firstText = false;
    public CientificoPersecucion cientificoPersecucion;

    void Start()
    {
        StartCoroutine(TimerToStart());
        TutorialScreen.SetActive(false);
        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
        Text0.SetActive(false);
    }

    void Update()
    {
        if(flySpawner.tutorialHelp && started && !once)
        {
            once = true;
            StartCoroutine(MoscaEnBoca());
        }
        if(cientificoPersecucion.enFaseDeteccion && !first)
        {
            first = true;
            SetText3();
        }
    }

    private IEnumerator TimerToStart()
    {
        yield return new WaitForSeconds(startTutorialAfter);
        SetText0();
    }

    private void SetText0()
    {
        Text0.SetActive(true);
        TutorialScreen.SetActive(true);

        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private IEnumerator TimerToStart2()
    {
        yield return new WaitForSeconds(startTextAfter);
        SetText1();
    }

    private void SetText1()
    {
        Text1.SetActive(true);
        TutorialScreen.SetActive(true);

        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        if(!firstText)
        {
            firstText = true;
            StartCoroutine(TimerToStart2());
        }

        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
        Text0.SetActive(false);
        TutorialScreen.SetActive(false);

        aim.enabled = true;
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("UI").Enable();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        started = true;
    }

    private IEnumerator MoscaEnBoca()
    {
        yield return new WaitForSeconds(0.5f);
        SetText2();
    }


    private void SetText2()
    {
        Text2.SetActive(true);
        TutorialScreen.SetActive(true);

        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SetText3()
    {
        Text3.SetActive(true);
        TutorialScreen.SetActive(true);

        aim.enabled = false;
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("UI").Disable();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
