using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonToNextScene : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the select event
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        // Unsubscribe from the select event
        GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        // Load the next scene
        SceneManager.LoadScene(2);
    }
}
