using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public enum GameState
    {
        Explore,
        BabyDiscovered,
        ToyReturned
    }

    public GameState currentState = GameState.Explore;

    [Header("Game Object References")]
    public EnemyAI kuntilanakAI;
    public GameObject cryingBaby;
    public GameObject wallBlocker;
    public GameObject doorObject;
    public GameObject player;

    void Start()
    {
        // start passive
        wallBlocker.SetActive(false);

        if (kuntilanakAI != null)
            kuntilanakAI.enabled = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Explore:
                // passive state — waiting for player to take photo of baby
                break;

            case GameState.BabyDiscovered:
                // entity becomes aggressive
                if (kuntilanakAI != null)
                {
                    kuntilanakAI.enabled = true;
                    Debug.Log("Entity is enabled.");
                }
                break;

            case GameState.ToyReturned:
                // Escape enabled
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

        //Debug.Log("Toy returned to baby. Door restored. Escape enabled.");
    }
}
