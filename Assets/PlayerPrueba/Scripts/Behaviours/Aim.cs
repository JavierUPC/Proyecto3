using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    public Camera mainCam;
    public CinemachineFreeLook freeLookCamera;
    public float zoomSpeed = 5f;
    public float aimDistanceMulti = 0.4f;
    public float aimFOV = 40f;
    public float sideDistance = 1.5f;
    public float rotationSpeed;
    public Transform lookAt;
    public InputActionReference aim;
    private void OnEnable()
    {
        aim.action.Enable();
    }

    private void OnDisable()
    {
        aim.action.Disable();
    }

    private float normalFOV;
    private float zoomLevel = 1f; // 1 = normal state, 0 = aiming
    private CinemachineVirtualCamera vCam;

    private float[] originalHeights = new float[3];
    private float[] originalDistances = new float[3];
    private Vector3 initPosLookAt, currentLookAt;

    void Start()
    {
        initPosLookAt = new Vector3(0, 0, 0);
        currentLookAt = lookAt.localPosition;

        vCam = freeLookCamera.GetComponent<CinemachineVirtualCamera>();

        for (int i = 0; i < 3; i++)
        {
            originalHeights[i] = freeLookCamera.m_Orbits[i].m_Height;
            originalDistances[i] = freeLookCamera.m_Orbits[i].m_Radius;
        }

        if (vCam != null)
        {
            normalFOV = vCam.m_Lens.FieldOfView;
        }
        else
        {
            normalFOV = Camera.main.fieldOfView;
        }
    }

    void Update()
    {
        bool isAiming = aim.action.IsPressed();

        if (zoomLevel >= 0.5)
        {
            currentLookAt = initPosLookAt;
        }
        else if(zoomLevel < 0.5)
        {
            currentLookAt = new Vector3(initPosLookAt.x + sideDistance, initPosLookAt.y, initPosLookAt.z);
            Vector3 cameraForward = mainCam.transform.forward;
            cameraForward.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        zoomLevel = Mathf.Lerp(zoomLevel, isAiming ? 0f : 1f, Time.unscaledDeltaTime * zoomSpeed);

        float targetTimeScale = isAiming ? 0.2f : 1f;
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * zoomSpeed);


        AdjustCamera(zoomLevel);
    }

    void AdjustCamera(float zoom)
    {
        for (int i = 0; i < 3; i++)
        {
            freeLookCamera.m_Orbits[i].m_Height = Mathf.Lerp(originalHeights[i], originalHeights[i] * 0.8f, 1 - zoom); 
            freeLookCamera.m_Orbits[i].m_Radius = Mathf.Lerp(originalDistances[i], originalDistances[i] * aimDistanceMulti, 1 - zoom);
        }

        if (vCam != null)
        {
            vCam.m_Lens.FieldOfView = Mathf.Lerp(normalFOV, aimFOV, 1 - zoom);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(normalFOV, aimFOV, 1 - zoom);
        }

        lookAt.transform.localPosition = Vector3.Lerp(lookAt.transform.localPosition, currentLookAt, Time.unscaledDeltaTime * zoomSpeed/2);
    }
}
