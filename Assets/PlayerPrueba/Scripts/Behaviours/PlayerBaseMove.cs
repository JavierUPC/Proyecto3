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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = player.GetComponent<Rigidbody>();
        speed = moveSpeed;
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
        if (grounded)
        {
            Move();
        }

        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
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
