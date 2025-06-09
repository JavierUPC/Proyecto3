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
        // Search the current object and all its children
        var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        var skinnedRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);

        foreach (var renderer in meshRenderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        foreach (var renderer in skinnedRenderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                    return mat;
            }
        }

        // Now search upwards through parents
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

        return null; // No material found anywhere
    }
}
