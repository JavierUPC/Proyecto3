using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camuflaje : MonoBehaviour
{
    public Material matContact;
    public Material matPlayer;
    private float useTimer, cooldownTimer;
    private bool cooldown;
    public bool isCamo;
    public bool previousState;
    public ApplyAbilty checkMoscaEnBoca;

    public Renderer playerSkin;
    public float camoUseTime;
    public float camoCoolDown;
    public PlayerInput playerInput;
    private InputAction camouflage;

    private void OnEnable()
    {
        //camouflage.Enable();
    }

    private void OnDisable()
    {
        //camouflage.Disable();
    }

    void Start()
    {
        camouflage = playerInput.actions["Camo"];
    }

    void Update()
    {
        if (!cooldown)
            isCamo = (camouflage.IsPressed() && checkMoscaEnBoca.BugInMouth());

        if (isCamo)
            playerSkin.material = matContact;
        else
        {
            playerSkin.material = matPlayer;
            if (previousState)
                cooldown = true;
        }

        previousState = isCamo;
    }

    private void FixedUpdate()
    {
        if (isCamo)
        {
            useTimer += Time.deltaTime;
        }
        else
            useTimer = 0;

        if (useTimer >= camoUseTime)
        {
            cooldown = true;
            isCamo = false;
        }

        if (cooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= camoCoolDown)
            {
                cooldown = false;
                cooldownTimer = 0;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Material collisionMat = FindMaterialInObjectOrChildren(collision.gameObject);
            if (collisionMat != null)
            {
                matContact = collisionMat;
                //Debug.Log($"Material found: {collisionMat.name} on {collision.gameObject.name}");
            }
            else
            {
                //Debug.LogWarning($"No material found on {collision.gameObject.name} or its children");
            }
        }
    }

    private Material FindMaterialInObjectOrChildren(GameObject obj)
    {
        // Get all MeshRenderer and SkinnedMeshRenderer components in this object and children
        var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
        var skinnedRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

        // Check MeshRenderers
        foreach (var meshRenderer in meshRenderers)
        {
            foreach (var mat in meshRenderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        // Check SkinnedMeshRenderers
        foreach (var skinnedRenderer in skinnedRenderers)
        {
            foreach (var mat in skinnedRenderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        return null;
    }
}
