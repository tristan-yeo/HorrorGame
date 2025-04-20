using UnityEngine;

public class ToyDropZone : MonoBehaviour
{
    public PickUpToy pickUpToyScript;   
    public GameEventManager eventManager;
    public GameObject dropPromptText;
    public GameObject teddyInCrib;

    //private bool inReach = false;
    [SerializeField] private bool inReach = false;
    private bool toyPlaced = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = true;

            if (eventManager != null &&
                eventManager.currentState == GameEventManager.GameState.BabyDiscovered &&
                pickUpToyScript != null &&
                pickUpToyScript.HasToy())
            {
                dropPromptText?.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inReach = false;
            dropPromptText?.SetActive(false);
        }
    }


    void Update()
    {
        if (!inReach || toyPlaced) return;

        if (Input.GetButtonDown("Interact") && pickUpToyScript != null && pickUpToyScript.HasToy())
        {
            toyPlaced = true;

            dropPromptText?.SetActive(false);
            teddyInCrib?.SetActive(true);

            if (eventManager != null)
                eventManager.OnToyReturned();

            Debug.Log("Toy placed in crib. Escape now.");
        }
    }
}
