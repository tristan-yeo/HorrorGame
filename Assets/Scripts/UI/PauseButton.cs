using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class pauseButton : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    
    // Visual effect parameters
    [SerializeField] private float pausedVignetteIntensity = 0.8f;
    [SerializeField] private float normalVignetteIntensity = 0.2f;
    [SerializeField] private float pausedBloomIntensity = 10.5f;
    [SerializeField] private float normalBloomIntensity = 1.66f;
    
    private bool isPaused = false;
    private Vignette vignette;
    private Bloom bloom;
    private Button pause;
    
    void Start()
    {
        // Get reference to this button component
        pause = GetComponent<Button>();
        
        // Get references to the post-processing effects
        postProcessingVolume.profile.TryGet(out vignette);
        postProcessingVolume.profile.TryGet(out bloom);
        
        // Set initial values
        if (vignette != null)
            vignette.intensity.value = normalVignetteIntensity;
            
        if (bloom != null)
            bloom.intensity.value = normalBloomIntensity;
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log("Button was clicked! Toggling pause state.");

        
        // Pause/unpause game time
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Change visual effects
        if (vignette != null)
            vignette.intensity.value = isPaused ? pausedVignetteIntensity : normalVignetteIntensity;
            
        if (bloom != null)
            bloom.intensity.value = isPaused ? pausedBloomIntensity : normalBloomIntensity;
            
    }
    
    // Optional: Add this if you want to be able to unpause with Escape key
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) && isPaused) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
}