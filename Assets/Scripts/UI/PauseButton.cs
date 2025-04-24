using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class pauseButton : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private MonoBehaviour cameraController;
    [SerializeField] private TMP_Text pauseInstructionText;
    [SerializeField] private GameObject audioSourcesContainer; // Reference to GameObject containing audio sources
    [SerializeField] private StartDoors startDoors; // Reference to StartDoors script

    //to hide
    [SerializeField] private TMP_Text objective;
    [SerializeField] private TMP_Text objectiveTitle;

    // Visual effect parameters
    [SerializeField] private float pausedVignetteIntensity = 0.8f;
    [SerializeField] private float normalVignetteIntensity = 0.2f;
    [SerializeField] private float pausedBloomIntensity = 10.5f;
    [SerializeField] private float normalBloomIntensity = 1.66f;

    private bool isPaused = false;
    private Vignette vignette;
    private Bloom bloom;
    private Button pause;
    private AudioSource[] audioSources; // Array to store audio sources
    private bool wasAudioSequenceStarted = false; // To store the original state of audioSequenceStarted

    void Start()
    {
        // Get reference to this button component
        pause = GetComponent<Button>();

        // Get references to the post-processing effects
        postProcessingVolume.profile.TryGet(out vignette);
        postProcessingVolume.profile.TryGet(out bloom);

        // Get all audio sources from the container
        if (audioSourcesContainer != null)
        {
            audioSources = audioSourcesContainer.GetComponentsInChildren<AudioSource>();
        }

        // Set initial values
        if (vignette != null)
            vignette.intensity.value = normalVignetteIntensity;

        if (bloom != null)
            bloom.intensity.value = normalBloomIntensity;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Handle StartDoors audioSequenceStarted flag - now directly accessing the public variable
        if (startDoors != null)
        {
            if (isPaused)
            {
                // Store the current state and set to false when pausing
                wasAudioSequenceStarted = startDoors.audioSequenceStarted;
                startDoors.audioSequenceStarted = false;
            }
            else
            {
                // Restore the original state when unpausing
                startDoors.audioSequenceStarted = wasAudioSequenceStarted;
            }
        }

        // Handle audio sources - pause or unpause
        if (audioSources != null && audioSources.Length > 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
            }
        }

        // Pause/unpause game time
        Time.timeScale = isPaused ? 0f : 1f;

        // Enable/disable camera controller
        if (cameraController != null)
            cameraController.enabled = !isPaused;

        // make sure the instruction text is hidden at start
        if (pauseInstructionText != null)
            pauseInstructionText.gameObject.SetActive(isPaused);

        if (objectiveTitle != null)
            objectiveTitle.gameObject.SetActive(!isPaused);
        objective.gameObject.SetActive(!isPaused);

        // change visual effects // TODO:add more changes
        if (vignette != null)
            vignette.intensity.value = isPaused ? pausedVignetteIntensity : normalVignetteIntensity;

        if (bloom != null)
            bloom.intensity.value = isPaused ? pausedBloomIntensity : normalBloomIntensity;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}