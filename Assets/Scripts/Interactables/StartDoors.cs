using UnityEngine;

public class StartDoors : MonoBehaviour
{
    [SerializeField] private Animator door = null;
    [SerializeField] private AudioSource doorAudioSource = null;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip doorCloseSound;
    [SerializeField] private AudioSource triggerAudioSource = null;

    [Header("Lockable Door Logic")]
    public bool isEntranceDoor = false;
    public bool lockAfterEnter = false;
    private bool isLocked = false;
    private bool isOpen = false;
    
    // Flag to track if audio sequence has started
    private bool audioSequenceStarted = false;

    public GameEventManager eventManager;

    private void Start()
    {
        if (triggerAudioSource != null)
        {
            triggerAudioSource.Stop();
            triggerAudioSource.loop = false;
        }
    }

    private void Update()
    {
        // Only check if audio is done playing if the sequence has started and the door isn't open yet
        if (audioSequenceStarted && !isOpen && triggerAudioSource != null)
        {
            // Check if the audio has finished playing
            if (!triggerAudioSource.isPlaying)
            {
                Debug.Log("Trigger audio finished playing, opening door");
                OpenDoor();
                audioSequenceStarted = false; // Reset the flag after opening
            }
        }
    }

    public void TriggerDoorSequence()
    {
        if (isLocked) return;
        Debug.Log("Triggering door sequence");
        
        if (triggerAudioSource != null && triggerAudioSource.clip != null)
        {
            Debug.Log("Playing trigger audio");
            triggerAudioSource.Play();
            audioSequenceStarted = true; // Set the flag when audio starts playing
        }
        else
        {
            // If no audio trigger is set, open the door immediately
            Debug.Log("No audio trigger set");
        }
    }

    private void OpenDoor()
    {
        if (isLocked || isOpen) return;

        Debug.Log("Opening door");
        door.Play("DoorOpen", 0, 0.0f);
        PlayDoorSound();
        isOpen = true;
        
        // player entering hospital
        if (isEntranceDoor && eventManager != null && eventManager.currentState == GameEventManager.GameState.Spawn)
        {
            eventManager.currentState = GameEventManager.GameState.Explore;
            Debug.Log("Player entered hospital - state changed to Explore.");
        }
    }

    public void CloseDoor()
    {
        if (isLocked) return;
        
        door.Play("DoorClose", 0, 0.0f);
        PlayDoorCloseSound();
        isOpen = false;

        if (isEntranceDoor && lockAfterEnter)
        {
            isLocked = true;
            Debug.Log("Entrance door locked behind player.");
        }
    }

    public void PlayDoorSound()
    {
        if (doorAudioSource != null && doorSound != null)
            doorAudioSource.PlayOneShot(doorSound);
    }

    public void PlayDoorCloseSound()
    {
        if (doorAudioSource != null && doorCloseSound != null)
            doorAudioSource.PlayOneShot(doorCloseSound);
    }
}
