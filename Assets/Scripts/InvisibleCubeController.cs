using UnityEngine;
using UnityEngine.SceneManagement;

public class InvisibleCubeController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Debug.Log("Hunter detected! Loading GameOver scene."); // Debug message to check if the trigger is working
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            Debug.Log("Collided with: " + other.tag); // Debug message for other collisions
        }
    }
}
