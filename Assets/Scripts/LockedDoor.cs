using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoors : MonoBehaviour
{
    public Animator door; // Animator for door opening
    public GameObject keyOBNeeded; // The key required to open the door
    public GameObject openText; // Text prompting player to open
    public GameObject keyMissingText; // Text when key is missing
    public AudioSource doorSound; // Sound effect when door opens

    private bool inReach; // Whether the player is in range
    private bool isOpen; // If the door is already open

    void Start()
    {
        inReach = false;
        openText.SetActive(false);
        keyMissingText.SetActive(false);
        isOpen = false;
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
        if (isOpen) return; // Prevent further interactions if already open

        if (keyOBNeeded.activeInHierarchy && inReach && Input.GetButtonDown("Interact"))
        {
            keyOBNeeded.SetActive(false); // Remove the key
            doorSound.Play();
            door.SetBool("Open", true);
            door.SetBool("Closed", false);
            openText.SetActive(false);
            keyMissingText.SetActive(false);
            isOpen = true;
        }
        else if (!keyOBNeeded.activeInHierarchy && inReach && Input.GetButtonDown("Interact"))
        {
            openText.SetActive(false);
            keyMissingText.SetActive(true);
        }

        if (isOpen)
        {
            door.GetComponent<BoxCollider>().enabled = false;
            this.enabled = false; // Disable this script since door is permanently open
        }
    }
}
