using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class FlickerWithSound : MonoBehaviour
{
    private Light lightComponent;
    private AudioSource audioSource;

    [Header("State Durations")]
    [SerializeField, Range(1f, 5f)] private float minStateDuration = 1f;
    [SerializeField, Range(1f, 5f)] private float maxStateDuration = 5f;

    [Header("Flicker Settings")]
    [SerializeField, Range(0f, 1f)] private float flickerInterval = 0.1f;
    [SerializeField, Range(0f, 1f)] private float minLightIntensity = 0f;
    [SerializeField, Range(0f, 1f)] private float maxLightIntensity = 1f;

    private float currentStateTimer;
    private float currentStateDuration;
    private float flickerTimer;
    private bool isFlickeringState;
    private float originalIntensity;

    private void Start()
    {
        // Get required components
        lightComponent = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        // Store the original light intensity
        originalIntensity = lightComponent.intensity;

        // Start in steady state
        isFlickeringState = false;
        SetNewStateDuration();
    }

    private void Update()
    {
        currentStateTimer += Time.deltaTime;

        // Check if we need to switch states
        if (currentStateTimer >= currentStateDuration)
        {
            SwitchState();
        }

        // Handle the current state
        if (isFlickeringState)
        {
            HandleFlickeringState();
        }
        else
        {
            HandleSteadyState();
        }
    }

    private void HandleFlickeringState()
    {
        flickerTimer += Time.deltaTime;
        if (flickerTimer >= flickerInterval)
        {
            flickerTimer = 0f;
            // Randomly turn the light on or off
            bool isOn = Random.value > 0.5f;
            lightComponent.intensity = isOn ? maxLightIntensity : minLightIntensity;
            
            // Play or stop the sound based on light state
            if (isOn && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else if (!isOn && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void HandleSteadyState()
    {
        // Ensure light is on at original intensity
        lightComponent.intensity = originalIntensity;
        
        // Ensure sound is playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void SwitchState()
    {
        isFlickeringState = !isFlickeringState;
        currentStateTimer = 0f;
        flickerTimer = 0f;
        SetNewStateDuration();
    }

    private void SetNewStateDuration()
    {
        currentStateDuration = Random.Range(minStateDuration, maxStateDuration);
    }
}
