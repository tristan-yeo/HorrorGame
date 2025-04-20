using UnityEngine;

public class PickUpToy : MonoBehaviour
{
    public GameObject toyObject;           
    public GameObject pickUpText;       
    public AudioSource pickupSound;     
    public ToggleCRT toggleCRT;       
    public GameEventManager eventManager; 

    //private bool inReach = false;
    [SerializeField] private bool inReach = false;
    private bool toyPicked = false;

    void Start()
    {
        pickUpText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach") && toggleCRT.IsCameraOn())
        {
            inReach = true;
            pickUpText?.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            pickUpText?.SetActive(false);
        }
    }

    void Update()
    {
        if (toyPicked) return;

        if (toggleCRT != null && !toggleCRT.IsCameraOn())
            pickUpText?.SetActive(false);

        if (inReach && Input.GetButtonDown("Interact") && toggleCRT.IsCameraOn())
        {
            toyObject.SetActive(false);
            pickupSound?.Play();
            pickUpText?.SetActive(false);
            toyPicked = true;
        }
    }

    public bool HasToy()
    {
        return toyPicked;
    }
}
