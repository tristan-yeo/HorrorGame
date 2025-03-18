using BrewedInk.CRT;
using UnityEngine;

public class ToggleCRT : MonoBehaviour
{
    [SerializeField]
    private CRTCameraBehaviour crtCameraBehaviour; // Drag & drop in Inspector

    void Update()
    {
        // Check if 'C' is pressed
        if (Input.GetButtonDown("ToggleCamera"))
        {
            // Toggle the script on/off
            crtCameraBehaviour.enabled = !crtCameraBehaviour.enabled;
        }
    }
}
