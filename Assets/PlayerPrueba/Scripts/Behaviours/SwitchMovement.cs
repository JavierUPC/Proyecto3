using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchMovement : MonoBehaviour
{
    public GameObject verticalMovement;
    public GameObject baseMovement;
    private bool grounded = false, climbing = false;

    public InputActionReference climb;

    private void Start()
    {
        verticalMovement.SetActive(false);
        baseMovement.SetActive(true);
    }

    private void OnEnable()
    {
        climb.action.Enable();
        climb.action.started += SwitchState;
    }

    private void OnDisable()
    {
        climb.action.started -= SwitchState;
        climb.action.Disable();
    }

    public void SwitchState(InputAction.CallbackContext obj)
    {
        //Debug.Log("SwitchState called");
        if (grounded && climbing)
        {
            if (verticalMovement.activeSelf)
            {
                verticalMovement.SetActive(false);
                baseMovement.SetActive(true);
                GetComponent<Rigidbody>().useGravity = true;
                float playerYRotationInLocalSpace = transform.localEulerAngles.y;
                transform.localRotation = Quaternion.Euler(0f, playerYRotationInLocalSpace, 0f);
                //Debug.Log("Switched to Base Movement");
            }
            else if (baseMovement.activeSelf)
            {
                baseMovement.SetActive(false);
                verticalMovement.SetActive(true);
                float playerYRotationInLocalSpace = transform.localEulerAngles.y;
                transform.localRotation = Quaternion.Euler(0f, playerYRotationInLocalSpace, 0f);
                GetComponent<Rigidbody>().useGravity = false;
                Debug.Log("Switched to Climbing Movement");
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Grounded1");
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
