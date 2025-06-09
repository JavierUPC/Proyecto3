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
        GameObject other = collision.gameObject;

        if (other.layer == LayerMask.NameToLayer("Wall") || other.layer == LayerMask.NameToLayer("Ground"))
        {
            Material collisionMat = FindMaterialInHierarchy(other);
            if (collisionMat != null)
            {
                matContact = collisionMat;
                // Debug.Log($"Material found: {collisionMat.name} on {other.name}");
            }
            else
            {
                // Debug.LogWarning($"No material found on {other.name} or its hierarchy");
            }
        }
    }

    private Material FindMaterialInHierarchy(GameObject obj)
    {
        // 1. Search in children (including self)
        var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        foreach (var renderer in meshRenderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        var skinnedRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
        foreach (var renderer in skinnedRenderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        // 2. Search up through parents
        Transform current = obj.transform.parent;
        while (current != null)
        {
            var parentMesh = current.GetComponent<MeshRenderer>();
            if (parentMesh != null)
            {
                foreach (var mat in parentMesh.sharedMaterials)
                {
                    if (mat != null)
                        return mat;
                }
            }

            var parentSkinned = current.GetComponent<SkinnedMeshRenderer>();
            if (parentSkinned != null)
            {
                foreach (var mat in parentSkinned.sharedMaterials)
                {
                    if (mat != null)
                        return mat;
                }
            }

            current = current.parent;
        }

        return null; // No material found
    }
}
