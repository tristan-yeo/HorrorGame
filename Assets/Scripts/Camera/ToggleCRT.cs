using BrewedInk.CRT;
using UnityEngine;

public class ToggleCRT : MonoBehaviour
{
    [SerializeField]
    private CRTCameraBehaviour crtCameraBehaviour; // Drag & drop in Inspector

    [SerializeField]
    private GameObject cameraGameObject; // Drag & drop the "Camera" GameObject in Inspector

    // Flag to determine if toggling is allowed
    private bool canToggle = true;

    void Update()
    {
        if (!canToggle)
            return;

        if (Input.GetButtonDown("ToggleCamera"))
        {
            // Toggle the CRT behavior
            crtCameraBehaviour.enabled = !crtCameraBehaviour.enabled;

            // Toggle the camera GameObject and all its children
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

    // Called by the battery script when battery reaches 0%
    public void DisableCamera()
    {
        canToggle = false;
        crtCameraBehaviour.enabled = false;

        if (cameraGameObject != null)
        {
            cameraGameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Camera GameObject is not assigned!");
        }
    }

    // Called when the battery is recharged to allow toggling again
    public void EnableCameraToggle()
    {
        canToggle = true;
    }

    // Returns true if the camera is currently on (active)
    public bool IsCameraOn()
    {
        if (cameraGameObject == null)
            return false;
        return cameraGameObject.activeSelf;
    }
}
