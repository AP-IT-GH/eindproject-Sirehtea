using System.Collections;
using UnityEngine;

public class QuitAfterTime : MonoBehaviour
{
    public float quitAfterSeconds = 10f;

    void Start()
    {
        // Start the coroutine that will wait for the specified time and then quit the game
        StartCoroutine(QuitAfterDelay(quitAfterSeconds));
    }

    private IEnumerator QuitAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delay);

        Debug.Log("QUIT");
        Application.Quit();
    }
}
