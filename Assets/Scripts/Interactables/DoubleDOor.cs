using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [SerializeField] private Animator door = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip doorSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorClose", 0, 0.0f);
        }
    }

    public void PlayDoorSound()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }



}
