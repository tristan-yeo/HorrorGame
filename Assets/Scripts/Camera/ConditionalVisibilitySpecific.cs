using UnityEngine;

public class ConditionalVisibilitySpecific : MonoBehaviour
{
    [SerializeField]
    private ToggleCRT toggleCRT; // Assign the ToggleCRT component from your player camera

    private Renderer[] objectRenderers;
    private Collider[] objectColliders;

    void Awake()
    {
        // Retrieve all Renderer and Collider components on this GameObject and its children.
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        // Check if the CRT camera is active/toggled on.
        bool isVisible = toggleCRT != null && toggleCRT.IsCameraOn();

        // Toggle all renderers.
        foreach (Renderer renderer in objectRenderers)
        {
            renderer.enabled = isVisible;
        }

        // Toggle all colliders, unless the object is tagged "IgnoreVisibilityToggle".
        foreach (Collider col in objectColliders)
        {
            if (col.gameObject.CompareTag("IgnoreVisibilityToggle"))
                continue;

            col.enabled = isVisible;
        }
    }
}
