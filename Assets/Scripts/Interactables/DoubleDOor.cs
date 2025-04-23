using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [SerializeField] private Animator door = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip doorCloseSound;

    [Header("Lockable Door Logic")]
    public bool isEntranceDoor = false;
    public bool lockAfterEnter = false;
    private bool isLocked = false;

    [Header("Door Collision Control")]
    [SerializeField] private Collider doorCollider;

    public GameEventManager eventManager;

    private void OnTriggerEnter(Collider other)
    {
        if (isLocked) return;
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
            PlayDoorSound();
            // player entering hosp
            if (isEntranceDoor && eventManager != null && eventManager.currentState == GameEventManager.GameState.Spawn)
            {
                eventManager.currentState = GameEventManager.GameState.Explore;
                Debug.Log("Player entered hospital � state changed to Explore.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLocked) return;
        if (other.CompareTag("Player"))
        {
            door.Play("DoorClose", 0, 0.0f);
            PlayDoorCloseSound();

            if (isEntranceDoor && lockAfterEnter)
            {
                isLocked = true;
                Debug.Log("Entrance door locked behind player.");
                if (doorCollider != null)
                    doorCollider.isTrigger = false;
            }
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
