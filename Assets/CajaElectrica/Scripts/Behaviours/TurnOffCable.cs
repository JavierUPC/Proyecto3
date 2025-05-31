using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCable : MonoBehaviour
{
    public List<GameObject> cableToTurnOff; // List of objects to apply the material change to

    //Particulas

    void Start()
    {
        // This method will not be necessary if you don't need to do anything on Start()
    }

    // Call this method to switch the material on each object in the list
    public void SwitchOffCable()
    {
        foreach (GameObject obj in cableToTurnOff)
        {
            //Apagar particulas electricas
            obj.GetComponent<EliminatePlayer>().On = false;
        }
    }
}
