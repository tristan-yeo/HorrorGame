using BrewedInk.CRT;
using UnityEngine;

public class ToggleCRT : MonoBehaviour
{
    [SerializeField]
    private CRTCameraBehaviour crtCameraBehaviour; // Drag & drop in Inspector

    [SerializeField]
    private GameObject cameraGameObject; // Drag & drop the "Camera" GameObject in Inspector

    void Update()
    {
        // Check if 'ToggleCamera' button is pressed
        if (Input.GetButtonDown("ToggleCamera"))
        {
            // Toggle the CRTCameraBehaviour script on/off
            crtCameraBehaviour.enabled = !crtCameraBehaviour.enabled;

            // Toggle the Camera GameObject and all its children
            if (cameraGameObject != null)
            {
                cameraGameObject.SetActive(!cameraGameObject.activeSelf);
            }
            else
            {
                Debug.LogWarning("Camera GameObject is not assigned!");
            }
        }
    }
}
