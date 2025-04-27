using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCable : MonoBehaviour
{
    public Material offCable; // Assign this in the Inspector
    public List<GameObject> cableToTurnOff; // List of objects to apply the material change to

    private Renderer rend;

    void Start()
    {
        // This method will not be necessary if you don't need to do anything on Start()
    }

    // Call this method to switch the material on each object in the list
    public void SwitchOffCable()
    {
        foreach (GameObject obj in cableToTurnOff)
        {
            rend = obj.GetComponent<Renderer>();
            if (rend != null && offCable != null)
            {
                rend.material = offCable;
                obj.GetComponent<Electricity>().On = false;
            }
            else
            {
                Debug.LogWarning("Renderer or offCable material is missing on " + obj.name);
            }
        }
    }
}
