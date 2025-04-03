using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBatteries : MonoBehaviour
{
    public GameObject batteryPair;
    public GameObject invOB;
    public GameObject pickUpText;
    public AudioSource batterySound;

    public bool inReach;

    public ToggleCRT toggleCRT;
    public CameraBattery cameraBattery;

    void Start()
    {
        inReach = false;
        pickUpText.SetActive(false);
        invOB.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            // Always show the pickup text when in range (regardless of camera state)
            pickUpText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            pickUpText.SetActive(false);
        }
    }

    void Update()
    {
        // Removed camera check here — we always want to allow pickup
        if (inReach && Input.GetButtonDown("Interact"))
        {
            batteryPair.SetActive(false);

            if (batterySound != null)
                batterySound.Play();

            if (invOB != null)
                invOB.SetActive(true);

            pickUpText.SetActive(false);

            if (cameraBattery != null)
                cameraBattery.RechargeBattery();
        }
    }
}
