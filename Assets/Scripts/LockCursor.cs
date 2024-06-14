using UnityEngine;

public class LockCursor : MonoBehaviour
{
    void Start()
    {
        // Zorg ervoor dat de cursor onzichtbaar is
        Cursor.visible = true;

        // Lock de cursor in het midden van het scherm
        Cursor.lockState = CursorLockMode.Locked;
    }
}
