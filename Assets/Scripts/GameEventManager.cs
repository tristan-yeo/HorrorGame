using UnityEngine;
using TMPro; // Add this for TextMeshPro

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

    [Header("UI References")]
    public TMP_Text objectiveText;

    // Objective text for each state
    [Header("Objective Text")]
    [TextArea] public string spawnObjective = "enter the hospital.. at your own risk";
    [TextArea] public string exploreObjective = "explore the house and find something spooky";
    [TextArea] public string babyDiscoveredObjective = "make baby stfu.. find the baby's toy in the room the baby died in";
    [TextArea] public string toyReturnedObjective = "escape from kunti";

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
    }

    public void OnToyReturned()
    {
        if (currentState == GameState.ToyReturned) return;

        currentState = GameState.ToyReturned;

        if (wallBlocker != null)
            wallBlocker.SetActive(false);

        if (doorObject != null)
            doorObject.SetActive(true);
    }
}