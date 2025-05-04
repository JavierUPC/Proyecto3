using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchMovement : MonoBehaviour
{
    public GameObject verticalMovement;
    public GameObject baseMovement;
    private bool grounded = false, climbing = false;

    public InputActionReference climb, fall;

    private void Start()
    {
        verticalMovement.SetActive(false);
        baseMovement.SetActive(true);
    }

    private void OnEnable()
    {
        fall.action.Enable();
        climb.action.Enable();
        climb.action.started += SwitchState;
        fall.action.started += Fall;
    }

    private void OnDisable()
    {
        fall.action.Disable();
        climb.action.started -= SwitchState;
        fall.action.started -= Fall;
        climb.action.Disable();
    }

    public void Fall(InputAction.CallbackContext obj)
    {
        if (verticalMovement.activeSelf)
        {
            verticalMovement.SetActive(false);
            baseMovement.SetActive(true);
            GetComponent<Rigidbody>().useGravity = true;
            transform.up = Vector3.up;

            // Clear climbing state explicitly
            verticalMovement.GetComponent<PlayerVerticalMove>().ClearClimbState();
        }
    }

    public void SwitchState(InputAction.CallbackContext obj)
    {
        if (grounded && climbing)
        {
            if (verticalMovement.activeSelf)
            {
                verticalMovement.SetActive(false);
                baseMovement.SetActive(true);
                GetComponent<Rigidbody>().useGravity = true;
                float playerYRotationInLocalSpace = transform.localEulerAngles.y;
                transform.localRotation = Quaternion.Euler(0f, playerYRotationInLocalSpace, 0f);

                verticalMovement.GetComponent<PlayerVerticalMove>().ClearClimbState();
            }
            else if (baseMovement.activeSelf)
            {
                baseMovement.SetActive(false);
                verticalMovement.SetActive(true);
                float playerYRotationInLocalSpace = transform.localEulerAngles.y;
                transform.localRotation = Quaternion.Euler(0f, playerYRotationInLocalSpace, 0f);
                GetComponent<Rigidbody>().useGravity = false;
                verticalMovement.GetComponent<PlayerVerticalMove>().justStarted = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            climbing = true;
            verticalMovement.GetComponent<PlayerVerticalMove>().SetClimbing(climbing, collision);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            baseMovement.GetComponent<PlayerBaseMove>().SetGrounded(grounded);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            climbing = false;
            verticalMovement.GetComponent<PlayerVerticalMove>().SetClimbing(climbing, collision);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            baseMovement.GetComponent<PlayerBaseMove>().SetGrounded(grounded);
        }
    }
}
