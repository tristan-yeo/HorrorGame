using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TakePicture : MonoBehaviour
{
    [Header("Interact & Inventory")]
    public ToggleCRT toggleCRT;
    public GameObject takePicText;
    public GameObject photoCollectedObj;
    public AudioSource shutterSound;

    [Header("Bloom Flash Settings")]
    [Tooltip("Your global Volume with a Bloom override.")]
    public Volume postProcessVolume;
    [Tooltip("How bright the bloom goes at the moment of the flash.")]
    public float flashBloomIntensity = 10f;
    [Tooltip("How long (in seconds) it takes to fade bloom back to original.")]
    public float bloomFadeDuration = 0.5f;

    // internal bloom handle
    private Bloom _bloom;
    private float _originalBloomIntensity;

    private bool inReach = false;
    private bool pictureTaken = false;

    void Start()
    {
        // hide UI prompts
        if (takePicText != null) takePicText.SetActive(false);
        if (photoCollectedObj != null) photoCollectedObj.SetActive(false);

        // grab your Bloom override from the Volume
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out _bloom))
        {
            // ensure it's in override mode so we can write to it
            _bloom.intensity.overrideState = true;
            // store whatever your scene started with
            _originalBloomIntensity = _bloom.intensity.value;
        }
        else
        {
            Debug.LogWarning("[TakePicture] Could not find a Bloom override on your Volume.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (pictureTaken) return;
        if (other.CompareTag("Reach") && toggleCRT.IsCameraOn())
        {
            inReach = true;
            takePicText.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (pictureTaken) return;
        if (!other.CompareTag("Reach")) return;
        // show/hide prompt if player toggles camera mid-hover
        if (toggleCRT.IsCameraOn() && !takePicText.activeSelf) takePicText.SetActive(true);
        if (!toggleCRT.IsCameraOn() && takePicText.activeSelf) takePicText.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (pictureTaken) return;
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            takePicText.SetActive(false);
        }
    }

    void Update()
    {
        if (pictureTaken) return;

        if (inReach
            && toggleCRT != null
            && toggleCRT.IsCameraOn()
            && Input.GetButtonDown("Interact"))
        {
            // play camera shutter
            shutterSound?.Play();

            // reveal your photo‐collected icon
            photoCollectedObj?.SetActive(true);

            // hide the prompt & prevent re-trigger
            takePicText.SetActive(false);
            pictureTaken = true;

            // stop further triggers
            var col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            // start the bloom flash
            if (_bloom != null)
                StartCoroutine(FlashBloom());
        }
    }

    private IEnumerator FlashBloom()
    {
        // jump to the high-intensity bloom
        _bloom.intensity.value = flashBloomIntensity;

        float elapsed = 0f;
        // lerp back down over bloomFadeDuration
        while (elapsed < bloomFadeDuration)
        {
            elapsed += Time.deltaTime;
            _bloom.intensity.value = Mathf.Lerp(
                flashBloomIntensity,
                _originalBloomIntensity,
                elapsed / bloomFadeDuration
            );
            yield return null;
        }

        // ensure we end exactly at your original setting
        _bloom.intensity.value = _originalBloomIntensity;
    }
}
