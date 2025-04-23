using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this for TextMeshPro
using System.Collections;

public class GameEventManager : MonoBehaviour
{
    public enum GameState
    {
        Spawn, // initial state of player
        Explore,
        BabyDiscovered,
        ToyReturned
    }

    public GameState currentState = GameState.Explore;

    [Header("Game Object References")]
    public GameObject kuntilanak;
    public GameObject cryingBaby;
    public GameObject wallBlocker;
    public GameObject doorObject;
    public GameObject player;
    private BoxCollider kuntilanakCollider;
    
    [Header("Audio References")]
    [SerializeField] private AudioSource whatTheHell;
    [SerializeField] private AudioSource shutThatBabyUp;
    [SerializeField] private AudioSource toyReturnedSound;
    
    [Header("Door Control")]
    [SerializeField] private StartDoors entranceDoor;
    [SerializeField] private float doorTriggerDelay = 1f;

    [Header("UI References")]
    public TMP_Text objectiveText;

    [TextArea] public string spawnObjective = "Press 'C' to use your paranormal camera";
    [TextArea] public string exploreObjective = "Take pictures of some paranormal content";
    [TextArea] public string babyDiscoveredObjective = "Soothe the baby with something";
    [TextArea] public string toyReturnedObjective = "Run";

    private GameState previousState; // To track state changes

    void Start()
    {
        // Start passive
        wallBlocker.SetActive(false);
        previousState = currentState;

        if (kuntilanak != null)
        {
            kuntilanakCollider = kuntilanak.GetComponent<BoxCollider>();
            kuntilanak.SetActive(false);

            if (kuntilanakCollider != null)
                kuntilanakCollider.enabled = false;
        }

        // Set initial objective text
        UpdateObjectiveText();
        
        // Start the door trigger sequence with a delay
        if (entranceDoor != null)
        {
            StartCoroutine(TriggerDoorAfterDelay());
        }
    }
    
    private IEnumerator TriggerDoorAfterDelay()
    {
        yield return new WaitForSeconds(doorTriggerDelay);
        entranceDoor.TriggerDoorSequence();
        Debug.Log("Door sequence triggered after " + doorTriggerDelay + " seconds");
    }

    void Update()
    {
        // Check if state has changed
        if (previousState != currentState)
        {
            previousState = currentState;
            UpdateObjectiveText(); // Update text when state changes
        }

        switch (currentState)
        {
            case GameState.Spawn:
                break;

            case GameState.Explore:
                // passive state – waiting for player to take photo of baby
                break;

            case GameState.BabyDiscovered:
                // entity becomes aggressive
                if (kuntilanak != null && !kuntilanak.activeSelf)
                {
                    kuntilanak.SetActive(true);

                    if (kuntilanakCollider != null)
                        kuntilanakCollider.enabled = true;

                    Debug.Log("Entity is enabled.");
                }
                GenerateSound();
                break;


            case GameState.ToyReturned:
                // Escape enabled
                break;
        }
    }

    // New method to update the objective text
    private void UpdateObjectiveText()
    {
        if (objectiveText == null) return;

        switch (currentState)
        {
            case GameState.Spawn:
                objectiveText.text = spawnObjective;
                break;

            case GameState.Explore:
                objectiveText.text = exploreObjective;
                break;

            case GameState.BabyDiscovered:
                objectiveText.text = babyDiscoveredObjective;
                break;

            case GameState.ToyReturned:
                objectiveText.text = toyReturnedObjective;
                break;
        }
    }

    public void OnBabyPhotoTaken()
    {
        if (currentState != GameState.Explore) return;

        currentState = GameState.BabyDiscovered;

        Debug.Log("Baby photographed. Kuntilanak becomes aggressive.");

        if (wallBlocker != null)
            wallBlocker.SetActive(true);

        if (doorObject != null)
            doorObject.SetActive(false);
            
        // Handle crying baby audio sources
        if (cryingBaby != null)
        {
            AudioSource[] audioSources = cryingBaby.GetComponents<AudioSource>();
            if (audioSources.Length >= 2)
            {
                // Disable the first audio source
                audioSources[0].Stop();
                audioSources[0].enabled = false;
                
                // Start playing the second audio source
                audioSources[1].enabled = true;
                audioSources[1].loop = true;
                audioSources[1].Play();
                
                Debug.Log("Changed baby crying audio");
            }
            else
            {
                Debug.LogWarning("Crying baby doesn't have enough audio sources.");
            }
        }
        
        // Play audio sources with delays
        StartCoroutine(PlayDelayedAudio());
    }
    
    private IEnumerator PlayDelayedAudio()
    {
        // Wait 3 seconds before playing whatTheHell
        yield return new WaitForSeconds(3f);
        if (whatTheHell != null)
        {
            whatTheHell.Play();
            Debug.Log("Playing 'What the hell' audio");
        }
        
        // Wait another 3 seconds before playing shutThatBabyUp
        yield return new WaitForSeconds(3f);
        if (shutThatBabyUp != null)
        {
            shutThatBabyUp.Play();
            Debug.Log("Playing 'Shut that baby up' audio");
        }
    }

    public void OnToyReturned()
    {
        if (currentState == GameState.ToyReturned) return;

        currentState = GameState.ToyReturned;

        if (wallBlocker != null)
            wallBlocker.SetActive(false);

        if (doorObject != null)
            doorObject.SetActive(true);
            
        // Play toy returned sound
        if (toyReturnedSound != null)
        {
            toyReturnedSound.Play();
            Debug.Log("Playing toy returned sound");
        }
            
        // Disable all audio sources on the crying baby
        if (cryingBaby != null)
        {
            AudioSource[] audioSources = cryingBaby.GetComponents<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                source.Stop();
                source.enabled = false;
            }
            Debug.Log("All baby crying audio disabled");
        }

        //Debug.Log("Toy returned to baby. Door restored. Escape enabled.");
    }

    void GenerateSound()
    {
        EnemyAI enemy = FindObjectOfType<EnemyAI>();
        if (enemy != null && player != null)
        {
            Vector3 soundPosition = player.transform.position;
            enemy.HearSound(soundPosition);
            Debug.Log("Enemy has heard sound at player location: " + soundPosition);
        }
        else
        {
            Debug.LogWarning("Enemy or player reference missing.");
        }
    }
}