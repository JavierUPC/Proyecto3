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
    private Transform currentClimbable;
    private ContactPoint lastValidContact;


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
            AlignZDirectionToUp(); // only align up direction, not forward
            Move();
        }
    }

    public void SetClimbing(bool climbing, Collision collision)
    {
        if (!climbing || !gameObject.activeSelf)
            return;

        // Determine the highest contact point
        ContactPoint highestPoint = collision.contacts[0];
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y > highestPoint.point.y)
            {
                highestPoint = contact;
            }
        }

        // If we're already latched onto a surface
        if (isClimbing && currentClimbable != null && collision.transform != currentClimbable)
        {
            // Determine direction to new surface
            Vector3 toNewSurface = (highestPoint.point - player.transform.position).normalized;
            float alignment = Vector3.Dot(player.transform.forward, toNewSurface);

            // Only switch if player is moving toward the new surface
            if (alignment < 0.3f)  // Adjust this threshold to taste
            {
                return; // Reject the switch
            }
        }

        surfaceNormal = highestPoint.normal;
        currentClimbable = collision.transform;
        lastValidContact = highestPoint;

        isClimbing = true;
        AlignToSurface();
        AlignZDirectionToUp();
        rb.useGravity = false;
    }

    private void AlignToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, surfaceNormal) * player.transform.rotation;
        player.transform.rotation = targetRotation;
    }

    private void AlignZDirectionToUp()
    {
        // Keep the player's up aligned to the surface normal without affecting the forward direction
        Quaternion alignUp = Quaternion.FromToRotation(player.transform.up, surfaceNormal);
        player.transform.rotation = alignUp * player.transform.rotation;
    }

    private void ApplyGravity()
    {
        Vector3 gravityDirection = -surfaceNormal * gravityForce;
        rb.AddForce(gravityDirection, ForceMode.Acceleration);
    }

    private void Move()
    {
        input = move.action.ReadValue<Vector2>();

        // Turn left/right (A/D) using local Y axis
        float turnSpeed = 100f;
        float turnAmount = input.x * turnSpeed * Time.fixedDeltaTime;

        // Rotate around the player's local up (which is aligned with the surface normal)
        player.transform.localRotation *= Quaternion.AngleAxis(turnAmount, Vector3.up);

        // Move forward/backward (W/S) in the direction the player is facing
        Vector3 moveDirection = player.transform.forward * input.y;
        rb.velocity = moveDirection * moveSpeed + Vector3.Project(rb.velocity, surfaceNormal);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable") && collision.transform == currentClimbable)
        {
            isClimbing = false;
            currentClimbable = null;
            rb.useGravity = true;
        }
    }
}
