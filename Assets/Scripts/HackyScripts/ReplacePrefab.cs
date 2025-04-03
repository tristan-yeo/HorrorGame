using UnityEngine;

public class ReplacePrefabs : MonoBehaviour
{
    public GameObject oldPrefab;
    public GameObject newPrefab;

    private void Start()
    {
        ReplaceAllInstances();
    }

    private void ReplaceAllInstances()
    {
        GameObject[] instances = GameObject.FindGameObjectsWithTag(oldPrefab.tag);

        foreach (GameObject instance in instances)
        {
            Vector3 position = instance.transform.position;
            Quaternion rotation = instance.transform.rotation;
            Transform parent = instance.transform.parent;

            GameObject newInstance = Instantiate(newPrefab, position, rotation, parent);
            Destroy(instance);
        }
    }
}
