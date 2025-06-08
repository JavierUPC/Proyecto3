using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    public GameObject vertical;
    public GameObject horizontal;
    public bool isAiming;
    public Camera mainCam;
    public CinemachineOrbitalFollow freeLookCamera;
    public float zoomSpeed = 5f;
    public float aimDistanceMulti = 0.4f;
    public float aimFOV = 40f;
    public Vector2 displaceCam = new Vector2(1.5f, 0.5f);
    public Vector2 displaceCamClimb = new Vector2(0.5f, 1.5f);
    public float rotationSpeed;
    public Transform lookAt;
    public PlayerInput playerInput;
    private InputAction fire, aim;
    public Animator uiApuntarLento, uiApuntarHabilidad;
    private void OnEnable()
    {
        fire = playerInput.actions["Fire"];
        aim = playerInput.actions["Aim"];

        fire.performed += Fire;
        fire.canceled += FireReleased;
    }

    private void OnDisable()
    {
        //aim.Disable();
        //fire.Disable();

        fire.performed -= Fire;
        fire.canceled -= FireReleased;
    }

    public FireAbilty fireAbilty;
    public ApplyAbilty checkIfBugInMouth;
    public GameObject playerVertical;
    private float normalFOV;
    private float zoomLevel = 1f;
    private CinemachineVirtualCamera vCam;
    private CinemachineBrain cinemachineBrain;
    public Lengua lengua;

    private float[] originalHeights = new float[3];
    private float[] originalDistances = new float[3];
    private Vector3 initPosLookAt, currentLookAt;
    private float fixedDeltaTimeInit;
    public Animator animator;
    public Animator lenguaAnimator;
    public GameObject toungue;
    public SFX_Manager sfx;

    private int toungueCooldown = 1;
    private float timer = 0f;

    void Start()
    {
        initPosLookAt = new Vector3(0, 0, 0);
        currentLookAt = lookAt.localPosition;

        fixedDeltaTimeInit = Time.fixedDeltaTime;

        vCam = freeLookCamera.GetComponent<CinemachineVirtualCamera>();

        if (vCam != null)
        {
            normalFOV = vCam.m_Lens.FieldOfView;
        }
        else
        {
            normalFOV = Camera.main.fieldOfView;
        }

        cinemachineBrain = mainCam.GetComponent<CinemachineBrain>();
        cinemachineBrain.UpdateMethod = CinemachineBrain.UpdateMethods.ManualUpdate;
        lenguaAnimator.speed = 15;
        toungue.SetActive(false);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (timer >= toungueCooldown)
        {
            if (isAiming && zoomLevel < 0.1 && checkIfBugInMouth.BugInMouth())
            {
                lengua.firing = true;
                sfx.Play(5);
                lenguaAnimator.SetTrigger("Shoot");
                animator.SetTrigger("Shoot");
            }
            else if (isAiming && zoomLevel < 0.1 && !checkIfBugInMouth.BugInMouth())
            {
                fireAbilty.fireAbility = true;
                lenguaAnimator.SetTrigger("Shoot");
                animator.SetTrigger("Shoot");
            }

            timer = 0;
        }
    }

    private void FireReleased(InputAction.CallbackContext context)
    {
        lengua.firing = false;
        fireAbilty.fireAbility = false;
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        cinemachineBrain.ManualUpdate();

        isAiming = aim.IsPressed();

        AnimatorStateInfo stateInfo = lenguaAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("disparo"))
        {
            toungue.SetActive(true);
        }
        else
        {
            toungue.SetActive(false);
        }

        if (isAiming)
        {
            animator.SetBool("Aim", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Aim", false);
        }

            if (isAiming && vertical.activeSelf)
        {
            cinemachineBrain.WorldUpOverride = transform;
            freeLookCamera.TrackerSettings.BindingMode = Unity.Cinemachine.TargetTracking.BindingMode.LockToTarget;
        }
        else
        {
            cinemachineBrain.WorldUpOverride = null;
            freeLookCamera.TrackerSettings.BindingMode = Unity.Cinemachine.TargetTracking.BindingMode.WorldSpace;
        }

        Vector2 currentDisplaceCam = horizontal.activeSelf ? displaceCam : (vertical.activeSelf ? displaceCamClimb : displaceCam);

        if (zoomLevel >= 0.5)
        {
            currentLookAt = initPosLookAt;

            uiApuntarHabilidad.SetBool("Activar", false);

        }
        else if (zoomLevel < 0.5)
        {
            currentLookAt = new Vector3(initPosLookAt.x + currentDisplaceCam.x, initPosLookAt.y + currentDisplaceCam.y, initPosLookAt.z);

                uiApuntarHabilidad.SetBool("Activar", true);

            if (!playerVertical.activeSelf)
            {
                Vector3 cameraForward = mainCam.transform.forward;
                cameraForward.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.unscaledDeltaTime * rotationSpeed);
            }
        }

        zoomLevel = Mathf.Lerp(zoomLevel, isAiming ? 0f : 1f, Time.unscaledDeltaTime * zoomSpeed);

        float targetTimeScale;

        targetTimeScale = isAiming ? 0.1f : 1f;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * zoomSpeed);
        Time.fixedDeltaTime =  targetTimeScale*fixedDeltaTimeInit;

        AdjustCamera(zoomLevel);
    }

    void AdjustCamera(float zoom)
    {
        float[] aimHeights = { 2f, 0f, -2f };
        float[] defaultHeights = { 17f, 1.5f, -17f };
        float[] aimDistances = { 2f, 1.5f, 2f };
        float[] defaultDistances = { 14f, 14f, 14f };

        freeLookCamera.Orbits.Top.Height = Mathf.Lerp(defaultHeights[0], aimHeights[0], 1 - zoom);
        freeLookCamera.Orbits.Top.Radius = Mathf.Lerp(defaultDistances[0], aimDistances[0], 1 - zoom);

        freeLookCamera.Orbits.Center.Height = Mathf.Lerp(defaultHeights[1], aimHeights[1], 1 - zoom);
        freeLookCamera.Orbits.Center.Radius = Mathf.Lerp(defaultDistances[1], aimDistances[1], 1 - zoom);

        freeLookCamera.Orbits.Bottom.Height = Mathf.Lerp(defaultHeights[2], aimHeights[2], 1 - zoom);
        freeLookCamera.Orbits.Bottom.Radius = Mathf.Lerp(defaultDistances[2], aimDistances[2], 1 - zoom);

        if (vCam != null)
        {
            vCam.m_Lens.FieldOfView = Mathf.Lerp(normalFOV, aimFOV, 1 - zoom);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(normalFOV, aimFOV, 1 - zoom);
        }

        lookAt.transform.localPosition = Vector3.Lerp(lookAt.transform.localPosition, currentLookAt, Time.unscaledDeltaTime * zoomSpeed / 2);
    }
}
