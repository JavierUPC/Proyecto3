using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camuflaje : MonoBehaviour
{
    private Material matContact;
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

    // Start is called before the first frame update
    void Start()
    {
        camouflage = playerInput.actions["Camo"];
    }

    // Update is called once per frame
    void Update()
    {
        if(!cooldown)
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

        if(useTimer >= camoUseTime)
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

    //-----------------------------------------------------------------------------------------
    //AÑADIR METODO PARA ACTICAR COOLDOWN PARA NO PODER ACTIVAR CAMO SI TIENE MOSCA EN LA BOCA
    //-----------------------------------------------------------------------------------------

    private void OnCollisionStay(Collision collision)
    {
        Material collisionMat = collision.gameObject.GetComponent<MeshRenderer>().material;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            matContact = collisionMat;
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {

    //    }
    //}
}
