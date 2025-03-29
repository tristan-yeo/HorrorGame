using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material oldMaterial;  // Material A
    public Material newMaterial;  // Material B

    void Start()
    {
        // Find all objects with the old material
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer rend in allRenderers)
        {
            // Check if the object uses the old material
            if (rend.sharedMaterial == oldMaterial)
            {
                // Change to the new material
                rend.sharedMaterial = newMaterial;
            }
        }
    }
}
