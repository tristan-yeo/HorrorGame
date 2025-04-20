using UnityEngine;

public class ConditionalVisibilitySpecific : MonoBehaviour
{
    [SerializeField] private ToggleCRT toggleCRT;

    private Renderer[] objectRenderers;
    private Collider[] objectColliders;

    void Awake()
    {
        // Grab everything in this GameObject & its children
        objectRenderers = GetComponentsInChildren<Renderer>(true);
        objectColliders = GetComponentsInChildren<Collider>(true);
    }

    void Update()
    {
        bool isVisible = (toggleCRT != null) && toggleCRT.IsCameraOn();

        // Flip renderers
        foreach (var rend in objectRenderers)
            rend.enabled = isVisible;

        // Flip colliders (unless tagged to ignore)
        foreach (var col in objectColliders)
        {
            if (col.CompareTag("IgnoreVisibilityToggle"))
                continue;
            col.enabled = isVisible;
        }
    }
}
