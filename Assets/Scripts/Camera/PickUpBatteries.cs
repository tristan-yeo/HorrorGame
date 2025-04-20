using UnityEngine;

public class PickUpBatteries : MonoBehaviour
{
    [Header("Battery & UI")]
    public GameObject batteryPair;
    public GameObject invOB;
    public GameObject pickUpText;
    public AudioSource batterySound;

    [Header("Battery Logic")]
    public CameraBattery cameraBattery;

    void Start()
    {
        pickUpText.SetActive(false);
        invOB.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
            pickUpText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
            pickUpText.SetActive(false);
    }

    void Update()
    {
        if (pickUpText.activeSelf && Input.GetButtonDown("Interact"))
        {
            batteryPair.SetActive(false);
            batterySound?.Play();
            invOB?.SetActive(true);
            pickUpText.SetActive(false);

            cameraBattery?.RechargeBattery();
        }
    }
}
