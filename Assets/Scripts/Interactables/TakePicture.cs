using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [Header("References")]
    public ToggleCRT toggleCRT;
    public GameObject takePicText;
    public GameObject photoCollectedObj;
    public AudioSource shutterSound;

    private bool inReach = false;
    private bool pictureTaken = false;

    void Start()
    {
        if (takePicText != null) takePicText.SetActive(false);
        if (photoCollectedObj != null) photoCollectedObj.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (pictureTaken) return;

        if (other.CompareTag("Reach"))
        {
            inReach = true;
            // only show if camera is already on
            if (toggleCRT != null && toggleCRT.IsCameraOn())
                takePicText.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (pictureTaken) return;

        if (other.CompareTag("Reach"))
        {
            // if camera got toggled on while standing there
            if (toggleCRT.IsCameraOn() && !takePicText.activeSelf)
                takePicText.SetActive(true);

            // if camera got toggled off, always hide
            if (!toggleCRT.IsCameraOn() && takePicText.activeSelf)
                takePicText.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (pictureTaken) return;

        if (other.CompareTag("Reach"))
        {
            inReach = false;
            takePicText.SetActive(false);
        }
    }

    void Update()
    {
        if (pictureTaken) return;

        if (inReach
            && toggleCRT != null
            && toggleCRT.IsCameraOn()
            && Input.GetButtonDown("Interact"))
        {
            shutterSound?.Play();

            if (photoCollectedObj != null)
                photoCollectedObj.SetActive(true);

            takePicText.SetActive(false);
            pictureTaken = true;

            var col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }
}
