using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalMove : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public float gravityForce = 9.81f;
    public PlayerInput playerInput;
    private InputAction move;
    private Rigidbody rb;
    private Vector3 surfaceNormal = Vector3.zero;
    public bool isClimbing = false;
    private Vector2 input;
    private float timer = 0;
    public bool justStarted;
    private Vector3 targetUp = Vector3.zero;
    private Vector3 previousUp;
    public Transform currentClimbable;
    private ContactPoint lastValidContact;
    private float lastDetachTime = -1f;
    private float reattachCooldown = 0.2f; // prevents immediate reattachment
    public Animator animator;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        justStarted = true;
        move = playerInput.actions["Move1"];
    }

    //private void OnEnable()
    //{
    //    move.Enable();
    //}

    //private void OnDisable()
    //{
    //    move.Disable();
    //}

    void FixedUpdate()
    {
        if (isClimbing)
        {
            ApplyGravity();
            AlignZDirectionToUp();
            Move();
        }

        if (input == Vector2.zero)
        {
            Debug.Log("Idle");
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        else if (input != Vector2.zero)
        {
            Debug.Log("Walk");
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
        }
    }

    public void SetClimbing(bool climbing, Collision collision)
    {
        if (!climbing || !gameObject.activeSelf)
            return;

        // Prevent reattachment too soon
        if (Time.time - lastDetachTime < reattachCooldown)
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

        // If already climbing a surface, avoid switching unless alignment is acceptable
        if (isClimbing && currentClimbable != null && collision.transform != currentClimbable)
        {
            Vector3 toNewSurface = (highestPoint.point - player.transform.position).normalized;
            float alignment = Vector3.Dot(player.transform.forward, toNewSurface);

            if (alignment < 0.3f)
                return; // Reject switch if player isn't clearly moving toward new surface
        }

        surfaceNormal = highestPoint.normal;
        currentClimbable = collision.transform;
        lastValidContact = highestPoint;

        isClimbing = true;
        AlignToSurface();
        AlignZDirectionToUp();
        rb.useGravity = false;
    }

    public void ClearClimbState()
    {
        isClimbing = false;
        currentClimbable = null;
        surfaceNormal = Vector3.zero;
        rb.useGravity = true;
        lastDetachTime = Time.time;
    }

    private void AlignToSurface()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, surfaceNormal) * player.transform.rotation;
        player.transform.rotation = targetRotation;
    }

    private void AlignZDirectionToUp()
    {
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
        input = move.ReadValue<Vector2>();
        float turnSpeed = 100f;
        float turnAmount = input.x * turnSpeed * Time.fixedDeltaTime;

        player.transform.localRotation *= Quaternion.AngleAxis(turnAmount, Vector3.up);

        Vector3 moveDirection = player.transform.forward * input.y;
        rb.velocity = moveDirection * moveSpeed + Vector3.Project(rb.velocity, surfaceNormal);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable") && collision.transform == currentClimbable)
        {
            ClearClimbState();
        }
    }
}
