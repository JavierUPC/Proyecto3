using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camuflaje : MonoBehaviour
{
    private Material matContact;
    private Material matPlayer;
    private Material matPrevious;
    private float useTimer, cooldownTimer;
    private bool cooldown;
    private Collider playerCollider;
    public bool isCamo;
    public bool previousState;

    public float camoUseTime;
    public float camoCoolDown;
    public InputActionReference camouflage;
    private void OnEnable()
    {
        camouflage.action.Enable();
    }

    private void OnDisable()
    {
        camouflage.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        matPlayer = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(!cooldown)
        isCamo = camouflage.action.IsPressed();

        if (isCamo)
            GetComponent<Renderer>().material = matContact;
        else
        {
            GetComponent<Renderer>().material = matPlayer;
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
