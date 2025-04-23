using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFloor : MonoBehaviour
{
    public AudioClip glassSound;
    
    [Tooltip("Volume of the glass sound")]
    [Range(0f, 1f)]
    public float volume = 0.7f;
    
    public AudioSource audioSource;
    
    public string playerTag = "Player";
    
    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayGlassSound();
        }
    }

    private void PlayGlassSound()
    {
        if (glassSound != null)
        {
            audioSource.clip = glassSound;
            audioSource.volume = volume;
            audioSource.Play(); // TRISTAN TODO
        }
        else
        {
            Debug.LogWarning("Glass sound not assigned to " + gameObject.name);
        }
    }
}