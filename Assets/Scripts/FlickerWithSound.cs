using System.Collections;
using UnityEngine;

public class LightFlickerController : MonoBehaviour
{
    public Light pointLight;
    public Light secondaryBulbLight;    // Optional secondary light for bulb glow
    public AudioSource audioSource;
    public MeshRenderer bulbRenderer;   // Optional mesh renderer for material-based glow
    public float minOnTime = 1f;
    public float maxOnTime = 5f;
    public float minFlickerTime = 1f;
    public float maxFlickerTime = 5f;
    public float minFlickerInterval = 0.05f;
    public float maxFlickerInterval = 0.2f;
    public float fadeSpeed = 2f;
    public float dimEmissionMultiplier = 0.2f;
    public float dimLightIntensityMultiplier = 0.2f;

    private Material bulbMaterial;
    private Color fullEmissionColor;
    private Color dimEmissionColor;
    private float fullLightIntensity;
    private float dimLightIntensity;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        if (bulbRenderer != null)
        {
            // create a material instance to avoid affecting other objects
            bulbMaterial = new Material(bulbRenderer.material);
            bulbRenderer.material = bulbMaterial;

            if (bulbMaterial.IsKeywordEnabled("_EMISSION") || bulbMaterial.HasProperty(EmissionColor))
            {
                fullEmissionColor = bulbMaterial.GetColor(EmissionColor);
                dimEmissionColor = fullEmissionColor * dimEmissionMultiplier;
            }
        }

        if (secondaryBulbLight != null)
        {
            fullLightIntensity = secondaryBulbLight.intensity;
            dimLightIntensity = fullLightIntensity * dimLightIntensityMultiplier;
        }

        if (pointLight != null || secondaryBulbLight != null || bulbRenderer != null)
        {
            StartCoroutine(ControlLight());
        }
        else
        {
            Debug.LogWarning("LightFlickerController has no light or renderer assigned.");
        }
    }

    private IEnumerator ControlLight()
    {
        while (true)
        {
            // Light stays ON for a random duration
            float onTime = Random.Range(minOnTime, maxOnTime);
            SetLightState(true);
            yield return new WaitForSeconds(onTime);

            // Light flickers for a random duration
            float flickerTime = Random.Range(minFlickerTime, maxFlickerTime);
            float elapsedTime = 0f;
            while (elapsedTime < flickerTime)
            {
                bool isOn = pointLight == null || !pointLight.enabled;
                SetLightState(isOn);

                float currentFlickerInterval = Random.Range(minFlickerInterval, maxFlickerInterval);
                elapsedTime += currentFlickerInterval;
                yield return new WaitForSeconds(currentFlickerInterval);
            }

            // Ensure the light stays on after flickering
            SetLightState(true);
        }
    }

    private void SetLightState(bool isOn)
    {
        if (pointLight != null)
        {
            pointLight.enabled = isOn;
        }

        if (bulbMaterial != null && (bulbMaterial.IsKeywordEnabled("_EMISSION") || bulbMaterial.HasProperty(EmissionColor)))
        {
            bulbMaterial.EnableKeyword("_EMISSION");
            bulbMaterial.SetColor(EmissionColor, isOn ? fullEmissionColor : dimEmissionColor);
        }

        if (secondaryBulbLight != null)
        {
            secondaryBulbLight.intensity = isOn ? fullLightIntensity : dimLightIntensity;
        }

        if (audioSource != null)
        {
            StartCoroutine(FadeAudio(isOn ? 1f : 0f));
        }
    }

    private IEnumerator FadeAudio(float targetVolume)
    {
        if (audioSource == null) yield break;

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