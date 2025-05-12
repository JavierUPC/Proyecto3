using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchMovement : MonoBehaviour
{
    public GameObject verticalMovement;
    public GameObject baseMovement;
    private bool grounded = false, climbing = false;
    private Coroutine fallCheckCoroutine = null;

    public float fallDelay = 0.5f;
    public float checkDistance = 0.6f; // --- MODIFIED: How far to raycast for climbables

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

        if (verticalMovement.activeSelf)
        {
            bool foundClimbableBelow = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 dir = contact.point - transform.position;
                if (Vector3.Dot(dir.normalized, -transform.up) > 0.5f)
                {
                    if (collision.gameObject.CompareTag("Climbable"))
                    {
                        foundClimbableBelow = true;
                        break;
                    }
                }
            }

            if (!foundClimbableBelow && fallCheckCoroutine == null)
            {
                fallCheckCoroutine = StartCoroutine(DelayedFall());
            }
            else if (foundClimbableBelow && fallCheckCoroutine != null)
            {
                StopCoroutine(fallCheckCoroutine);
                fallCheckCoroutine = null;
            }
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

    private IEnumerator DelayedFall()
    {
        yield return new WaitForSeconds(fallDelay);

        if (!IsClimbableBelow()) // --- MODIFIED
        {
            Fall(new InputAction.CallbackContext());
        }

        fallCheckCoroutine = null;
    }

    // --- ADDED: Uses raycast in local down direction to re-check climbable below
    private bool IsClimbableBelow()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
        {
            return hit.collider.CompareTag("Climbable");
        }
        return false;
    }
}
