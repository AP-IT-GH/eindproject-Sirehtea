using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keycard : MonoBehaviour
{
    public GameObject exitDoor;  // Assign the exit door in the inspector
    public float activationDistance = 2.0f;  // Distance within which the door will disappear

    private bool doorOpened = false;

    private void Update()
    {
        if (!doorOpened)
        {
            CheckProximityToExit();
        }
    }

    private void CheckProximityToExit()
    {
        float distanceToExit = Vector3.Distance(transform.position, exitDoor.transform.position);
        if (distanceToExit <= activationDistance)
        {
            OpenExit();
        }
    }

    private void OpenExit()
    {
        doorOpened = true;
        if (exitDoor != null)
        {
            // Make the door disappear
            exitDoor.SetActive(false);

            // Optionally, you can play an animation or sound effect here
            // Animator animator = exitDoor.GetComponent<Animator>();
            // animator.SetTrigger("OpenDoor");
        }
    }
}
