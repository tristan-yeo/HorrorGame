using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBoxScript : MonoBehaviour
{
    public Animator boxOB; // Box Animator
    public GameObject keyOBNeeded; // The key required to open the box
    public GameObject keyInsideBox; // The key inside the box (door key)
    public GameObject openText; // UI text: "Press to Open"
    public GameObject keyMissingText; // UI text: "You need a key!"
    public AudioSource openSound; // Sound effect when opening the box

    private bool inReach; // Player is in range
    private bool isOpen; // Box has been opened
    private Collider keyInsideBoxCollider; // Reference to door key's collider

    void Start()
    {
        inReach = false;
        openText.SetActive(false);
        keyMissingText.SetActive(false);
        isOpen = false;

        // Get the door key's collider and disable it at the start
        if (keyInsideBox != null)
        {
            keyInsideBoxCollider = keyInsideBox.GetComponent<Collider>();
            if (keyInsideBoxCollider != null)
            {
                keyInsideBoxCollider.enabled = false; // Prevent picking up the door key early
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            openText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            openText.SetActive(false);
            keyMissingText.SetActive(false);
        }
    }

    void Update()
    {
        if (isOpen) return; // If already opened, no further action needed

        if (keyOBNeeded.activeInHierarchy && inReach && Input.GetButtonDown("Interact"))
        {
            // Unlock and open the box
            keyOBNeeded.SetActive(false); // The player uses the key to unlock the box
            OpenBox();
        }
        else if (!keyOBNeeded.activeInHierarchy && inReach && Input.GetButtonDown("Interact"))
        {
            // If the player doesn't have the key, show the "Key Missing" message
            openText.SetActive(false);
            keyMissingText.SetActive(true);
        }
    }

    void OpenBox()
    {
        openSound.Play();
        boxOB.SetBool("open", true);
        openText.SetActive(false);
        keyMissingText.SetActive(false);
        isOpen = true;

        // Enable the door key's collider so the player can pick it up
        if (keyInsideBoxCollider != null)
        {
            keyInsideBoxCollider.enabled = true;
        }

        // Disable box interactions after opening
        boxOB.GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }
}
