using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLights : MonoBehaviour
{
    private Light lightToFlicker;
    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.5f;
    [SerializeField, Range(0f, 3f)] private float maxIntensity = 1.2f;
    [SerializeField, Min(0f)] private float timeBetweenIntensity = 0.1f;
    private float currentTimer;
   
    void Awake()
    {
        if (lightToFlicker == null)
        {
            lightToFlicker = GetComponent<Light>();
        }
        ValidateIntensityBounds();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if (!(currentTimer >= timeBetweenIntensity)) return;
        lightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
        currentTimer = 0;
    }

    private void ValidateIntensityBounds()
    {
        if(!(minIntensity > maxIntensity))
        {
            return;
        }
        Debug.LogWarning("Min Intensity is greater than max Intensity, swapping values!");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}
