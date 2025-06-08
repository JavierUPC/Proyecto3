using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseMove : MonoBehaviour
{
    public float moveSpeed = 0, runSpeed = 0, rotationSpeed = 0;
    public PlayerInput playerInput;
    private InputAction move, run;
    public Transform playerCam;

    private bool grounded = false;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float speed;

    public GameObject player;
    public Animator animator;

    public SFX_Manager sfx;
    private float timer = 0;
    private int clipNr = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = player.GetComponent<Rigidbody>();
        speed = moveSpeed;
        animator.speed = 5f;
    }


    //ACTION EVENTLISTENERS
    private void OnEnable()
    {
        run = playerInput.actions["Run"];
        move = playerInput.actions["Move"];

        run.started += Run;
        run.canceled += StopRun;
    }

    private void OnDisable()
    {
        //move.Disable();
        //run.Disable();

        run.started -= Run;
        run.canceled -= StopRun;
    }
    //------




    //UPDATE
    void Update()
    {
        Move();

        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        if (moveDirection.x == 0f && moveDirection.y == 0f)
        {
            //Debug.Log("Idle");
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        else if (speed == moveSpeed)
        {
            timer += Time.deltaTime;

            bool isMoving = moveDirection.magnitude > 0.1f;

            if (isMoving && timer >= 0.6f)
            {
                sfx.Play(clipNr);
                clipNr++;

                if (clipNr < 4)
                    clipNr = 0;

                timer = 0f;
            }

            //Debug.Log("Walk");
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
        }
        else if (speed == runSpeed)
        {
            timer += Time.deltaTime;

            bool isMoving = moveDirection.magnitude > 0.1f;

            if (isMoving && timer >= 0.25f)
            {
                sfx.Play(clipNr);
                clipNr++;

                if (clipNr < 4)
                    clipNr = 0;

                timer = 0f;
            }

            //Debug.Log("Run");
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
        }

    }
    //------


    //FIXEDUPDATE
    private void FixedUpdate()
    {
    }
    //------


    //MOVE
    private void Move()
    {
        Vector2 worldMoveDirection = move.ReadValue<Vector2>();
        Vector3 worldDirection = new Vector3(worldMoveDirection.x, 0, worldMoveDirection.y);
        moveDirection = playerCam.TransformDirection(worldDirection);
        if (moveDirection != new Vector3(0f, moveDirection.y, 0f))
        {
            Quaternion targetRotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x, playerCam.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetRotation, Time.unscaledDeltaTime * rotationSpeed);
        }
        //Debug.Log(worldMoveDirection);
        //Debug.Log("Move: " + moveDirection);
    }
    //------


    //RUN
    private void Run(InputAction.CallbackContext obj)
    {
        if (grounded)
            speed = runSpeed;
    }

    private void StopRun(InputAction.CallbackContext obj)
    {
        speed = moveSpeed;
    }
    //-----


    //GROUNDED
    public void SetGrounded(bool state)
    {
        //Debug.Log("Grounded");
        if (!gameObject.activeSelf)
            return;

        grounded = state;
    }
    //--------
}
