using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalMove : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public float gravityForce = 9.81f;
    public InputActionReference move;
    private Rigidbody rb;
    private Vector3 surfaceNormal;
    public bool isClimbing = false;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        move.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            ApplyGravity();
            Move();
        }
    }

    public void SetClimbing(bool climbing, Collision collision)
    {
        if (!climbing || !gameObject.activeSelf)
            return;

        ContactPoint highestPoint = collision.contacts[0];
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y > highestPoint.point.y)
            {
                highestPoint = contact;
            }
        }

        surfaceNormal = highestPoint.normal;
        isClimbing = true;
        AlignToSurface();
        AlignZDirectionToUp(collision.transform); //QUITAR ESTO PARA QUE NO SE ALINEE CON LA DIRECCIÓN DE ARRIBA DEL OBJETO QUE SE ESCALA
        rb.useGravity = false;
    }

    private void AlignToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, surfaceNormal) * player.transform.rotation;
        player.transform.rotation = targetRotation;
    }

    private void AlignZDirectionToUp(Transform climbableTransform) //QUITAR ESTO PARA QUE NO SE ALINEE CON LA DIRECCIÓN DE ARRIBA DEL OBJETO QUE SE ESCALA
    {
        Vector3 targetUp = climbableTransform.up;
        Quaternion newRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(targetUp, surfaceNormal), surfaceNormal);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, newRotation, 0.1f);
    }
    //HACER QUE CUANDO SE CLIQUE LA "S", CAMBIE LA DIROCCIÓN DE Y DEL OBJETO EN LA QUE APUNTA

    private void ApplyGravity()
    {
        Vector3 gravityDirection = -surfaceNormal * gravityForce;
        rb.AddForce(gravityDirection, ForceMode.Acceleration);
    }

    private void Move()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        Vector3 worldDirection = new Vector3(input.x, 0, input.y);
        Vector3 relativeDirection = player.transform.TransformDirection(worldDirection);
        rb.velocity = relativeDirection * moveSpeed + Vector3.Project(rb.velocity, surfaceNormal);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = false;
            rb.useGravity = true;
        }
    }
}
