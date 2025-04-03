using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject invOB;
    public GameObject pickUpText;
    public AudioSource keySound;

    public bool inReach;

    // New: Reference to ToggleCRT to check the camera state
    public ToggleCRT toggleCRT;

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
            // Show pickup text only if CRT camera is on.
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
        // If the CRT camera is off, ensure the pickup text is hidden.
        if (toggleCRT != null && !toggleCRT.IsCameraOn())
        {
            pickUpText.SetActive(false);
        }

        if (inReach && Input.GetButtonDown("Interact"))
        {
            keyOB.SetActive(false);
            keySound.Play();
            invOB.SetActive(true);
            pickUpText.SetActive(false);
        }
    }
}
