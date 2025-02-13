using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseMove : MonoBehaviour
{
    public float moveSpeed = 0, runSpeed = 0, jumpForce = 0, sensX = 0, sensY = 0;
    public InputActionReference move, rotate;
    public InputActionAsset inputActions;
    public Transform playerCam;

    private bool grounded;
    private Rigidbody rb;
    private Vector2 rotationDelta;
    private Vector3 moveDirection;
    private float speed;
    private float vertRot, horiRot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        speed = moveSpeed;
    }


    //ACTION EVENTLISTENERS
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    //------


    //JUMP
    private void Jump(InputAction.CallbackContext obj)
    {
        if (grounded)
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
    //------


    //UPDATE
    void Update()
    {
        //if (grounded)
        //{
        Move();

        if (grounded)
            Run();
        //}

        Rotate();

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
        Vector2 worldMoveDirection = move.action.ReadValue<Vector2>();
        Vector3 worldDirection = new Vector3(worldMoveDirection.x, 0, worldMoveDirection.y);
        moveDirection = transform.TransformDirection(worldDirection);

        //Debug.Log("Move: " + moveDirection);
    }
    //------


    //ROTATE    
    private void Rotate()
    {
        rotationDelta = rotate.action.ReadValue<Vector2>();

        //Debug.Log(rotationDelta);

        vertRot = rotationDelta.y * sensY * Time.deltaTime;
        horiRot = rotationDelta.x * sensX * Time.deltaTime;

        //Evitar que la cámara rote de más
        Vector3 currentRotation = playerCam.localEulerAngles;
        float pitch = currentRotation.x;
        if (pitch > 180)
            pitch -= 360;
        pitch = Mathf.Clamp(pitch - vertRot, -90f, 90f);
        currentRotation.x = pitch;
        //-------------------------------

        playerCam.localEulerAngles = currentRotation;
        transform.Rotate(Vector3.up, horiRot);
    }
    //------

    //RUN
    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            speed = runSpeed;
        else
            speed = moveSpeed;
    }
    //-----


    //GROUNDED
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }
    //--------
}
