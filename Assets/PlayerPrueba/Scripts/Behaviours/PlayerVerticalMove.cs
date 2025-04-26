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
    private Vector2 input;
    private float timer = 0;
    public bool justStarted;
    private Vector3 targetUp = Vector3.zero;
    private Vector3 previousUp;


    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        justStarted = true;
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
        AlignZDirectionToUp(collision.transform);
        rb.useGravity = false;
    }

    private void AlignToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, surfaceNormal) * player.transform.rotation;
        player.transform.rotation = targetRotation;
    }

    private void AlignZDirectionToUp(Transform climbableTransform)
    {
        if(targetUp != climbableTransform.up || targetUp != -climbableTransform.up)
        {
            if (previousUp == targetUp)
                targetUp = climbableTransform.up;
            else
                targetUp = -climbableTransform.up;
        }


        if (justStarted)
            targetUp = climbableTransform.up;

        if (input.y < 0 && timer > 0.5f)
        {
            timer = 0;
            if (targetUp != climbableTransform.up)
            {
                targetUp = climbableTransform.up;
                Debug.Log("Target Up: " + targetUp);
            }
            else
            {
                targetUp = -climbableTransform.up;
                Debug.Log("Target Up: " + targetUp);
            }
        }

        Quaternion newRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(targetUp, surfaceNormal), surfaceNormal);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, newRotation, 0.4f);
        justStarted = false;
        timer += Time.deltaTime;
        previousUp = climbableTransform.up;
    }

    private void ApplyGravity()
    {
        Vector3 gravityDirection = -surfaceNormal * gravityForce;
        rb.AddForce(gravityDirection, ForceMode.Acceleration);
    }

    private void Move()
    {
        input = move.action.ReadValue<Vector2>();
        Vector3 worldDirection = new Vector3(input.x, 0, input.y);
        Vector3 relativeDirection = player.transform.TransformDirection(worldDirection);
        rb.velocity = relativeDirection * moveSpeed + Vector3.Project(rb.velocity, surfaceNormal);


        //if (input.y < 0)
        //{
        //    player.transform.rotation = Quaternion.LookRotation(-player.transform.forward, player.transform.up);
        //}
        //else if (input.y > 0)
        //{
        //    player.transform.rotation = Quaternion.LookRotation(player.transform.forward, player.transform.up);
        //}
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
