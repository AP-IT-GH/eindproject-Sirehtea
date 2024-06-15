using UnityEngine;

public class Prisoner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            Debug.Log("HIT!");
        }
        else
        {
            Debug.Log("Collided with: " + other.tag);
        }
    }
}
