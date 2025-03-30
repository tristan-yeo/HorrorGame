using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResetPrefabInstances : MonoBehaviour
{
    public GameObject prefab; // Assign the prefab itself in the Inspector

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return;
        }

        // Find all instances of this prefab in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> instances = new List<GameObject>();

        Debug.Log("Searching for instances of prefab: " + prefab.name);
        
        foreach (GameObject obj in allObjects)
        {
            if (PrefabUtility.GetPrefabInstanceHandle(obj) != null && PrefabUtility.GetCorrespondingObjectFromSource(obj) == prefab)
            {
                instances.Add(obj);
                Debug.Log("Found instance: " + obj.name);
            }
        }

        if (instances.Count == 0)
        {
            Debug.LogWarning("No instances of the prefab found in the scene.");
        }

        // Revert all instances to their original prefab state
        foreach (GameObject instance in instances)
        {
            Debug.Log("Reverting instance: " + instance.name);
            RevertInstance(instance);
        }
    }

    void RevertInstance(GameObject instance)
    {
        #if UNITY_EDITOR
        // Revert the main prefab instance
        Debug.Log("Reverting prefab instance to original state: " + instance.name);
        PrefabUtility.RevertPrefabInstance(instance, InteractionMode.UserAction);

        // Check for MeshRenderer in the instance itself and its children
        ResetMaterial(instance);

        // Traverse children of the instance and reset materials
        foreach (Transform child in instance.transform)
        {
            Debug.Log("Reverting material on child: " + child.gameObject.name);
            ResetMaterial(child.gameObject);
        }
        #else
        Debug.LogWarning("RevertPrefabInstance only works in the Unity Editor.");
        #endif
    }

    void ResetMaterial(GameObject obj)
    {
        // Check if there's a MeshRenderer and reset its material
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Debug.Log("Resetting material on: " + obj.name + " (Material: " + renderer.sharedMaterial.name + ")");
            renderer.sharedMaterial = PrefabUtility.GetCorrespondingObjectFromSource(renderer)?.sharedMaterial;
        }
        else
        {
            Debug.LogWarning("No MeshRenderer found on: " + obj.name);
        }
    }
}
