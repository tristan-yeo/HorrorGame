using System.Collections;
using UnityEngine;

public class LightFlickerController : MonoBehaviour
{
    public Light pointLight;
    public AudioSource audioSource;
    public float minOnTime = 1f;
    public float maxOnTime = 5f;
    public float minFlickerTime = 1f;
    public float maxFlickerTime = 5f;
    public float minFlickerInterval = 0.05f;
    public float maxFlickerInterval = 0.2f;
    public float fadeSpeed = 2f; // Speed of audio fade in/out

    private void Start()
    {
        // Configure audio source for smooth transitions
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        StartCoroutine(ControlLight());
    }

    private IEnumerator ControlLight()
    {
        while (true)
        {
            // Light stays ON for a random duration
            float onTime = Random.Range(minOnTime, maxOnTime);
            pointLight.enabled = true;
            StartCoroutine(FadeAudio(1f));
            yield return new WaitForSeconds(onTime);
            
            // Light flickers for a random duration
            float flickerTime = Random.Range(minFlickerTime, maxFlickerTime);
            float elapsedTime = 0f;
            while (elapsedTime < flickerTime)
            {
                bool isOn = !pointLight.enabled;
                pointLight.enabled = isOn;
                
                // Control sound based on light state
                StartCoroutine(FadeAudio(isOn ? 1f : 0f));

                float currentFlickerInterval = Random.Range(minFlickerInterval, maxFlickerInterval);
                elapsedTime += currentFlickerInterval;
                yield return new WaitForSeconds(currentFlickerInterval);
            }
            
            // Ensure the light stays on after flickering
            pointLight.enabled = true;
            StartCoroutine(FadeAudio(1f));
        }
    }

    private IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < 0.1f) // Quick fade to prevent clipping
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime * fadeSpeed);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
