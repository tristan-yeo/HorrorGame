using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [SerializeField] private Animator door = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip doorCloseSound;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                door.Play("DoorOpen", 0, 0.0f);
                PlayDoorSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                door.Play("DoorClose", 0, 0.0f);
                PlayDoorCloseSound();
        }
    }

    public void PlayDoorSound()
    {
        if (audioSource != null && doorSound != null)
            audioSource.PlayOneShot(doorSound);
    }

    public void PlayDoorCloseSound()
    {
        if (audioSource != null && doorCloseSound != null)
            audioSource.PlayOneShot(doorCloseSound);
    }
}
