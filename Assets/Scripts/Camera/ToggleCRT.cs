using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleCRT : MonoBehaviour
{
    [SerializeField] private UniversalRendererData rendererData;
    [SerializeField] private string featureName = "FullScreenPassRendererFeature";
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private AudioSource toggleOnSound;

    private ScriptableRendererFeature fullScreenFeature;
    private bool canToggle = true;

    void Awake()
    {
        if (rendererData == null)
        {
            Debug.LogError("ToggleCRT: Renderer Data not assigned!");
            return;
        }

        foreach (var feat in rendererData.rendererFeatures)
        {
            if (feat != null && feat.name == featureName)
            {
                fullScreenFeature = feat;
                break;
            }
        }

        if (fullScreenFeature == null)
            Debug.LogError($"ToggleCRT: Cannot find feature '{featureName}' in {rendererData.name}.");
    }

    void Update()
    {
        if (!canToggle || fullScreenFeature == null)
            return;

        if (Input.GetButtonDown("ToggleCamera"))
        {
            bool newState = !fullScreenFeature.isActive;

            fullScreenFeature.SetActive(newState);

            if (cameraUI != null)
                cameraUI.SetActive(newState);
            else
                Debug.LogWarning("ToggleCRT: Camera UI GameObject not assigned!");

            // PLAY SOUND WHEN CAMERA TURNS ON
            if (newState)
            {
                if (toggleOnSound != null)
                    toggleOnSound.Play();
                else
                    Debug.LogWarning("ToggleCRT: toggleOnSound AudioSource not assigned!");
            }
        }
    }

    public void DisableCamera()
    {
        canToggle = false;
        if (fullScreenFeature != null)
            fullScreenFeature.SetActive(false);

        if (cameraUI != null)
            cameraUI.SetActive(false);
    }

    public void EnableCameraToggle()
    {
        canToggle = true;
    }

    public bool IsCameraOn()
    {
        return fullScreenFeature != null && fullScreenFeature.isActive;
    }
}
