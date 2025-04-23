using UnityEngine;

public class cursor : MonoBehaviour
{
    void Awake()
    {
        // Force unlock cursor as soon as scene loads
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // This ensures the cursor stays visible and unlocked even if something tries to change it
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Cursor was locked or invisible - forcing reset");
        }
    }
}