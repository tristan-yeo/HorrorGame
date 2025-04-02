using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBatteries : MonoBehaviour
{
    public GameObject batteryPair;       // Reference to the Battery Pair GameObject (parent of Battery and Battery(1))
    public GameObject invOB;             // Battery icon or inventory object to enable
    public GameObject pickUpText;        // Pickup UI prompt
    public AudioSource batterySound;     // Pickup sound effect

    public bool inReach;

    public ToggleCRT toggleCRT;          // To check if CRT is on (used for showing pickup text)
    public CameraBattery cameraBattery;  // Reference to CameraBattery script

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
            if (toggleCRT != null && toggleCRT.IsCameraOn())
            {
                pickUpText.SetActive(true);
            }
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
        if (toggleCRT != null && !toggleCRT.IsCameraOn())
        {
            pickUpText.SetActive(false);
        }

        if (inReach && Input.GetButtonDown("Interact"))
        {
            // Deactivate the batteries in the world
            batteryPair.SetActive(false);

            // Play pickup SFX
            if (batterySound != null)
                batterySound.Play();

            // Show the inventory battery icon
            if (invOB != null)
                invOB.SetActive(true);

            // Hide pickup prompt
            pickUpText.SetActive(false);

            // Recharge the camera battery to full
            if (cameraBattery != null)
                cameraBattery.RechargeBattery();
        }
    }
}
