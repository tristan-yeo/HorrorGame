using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign Pause UI Panel
    public AudioSource ambientSound; // Assign Background Ambience
    public MonoBehaviour playerController; // Assign your FirstPersonController script here
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freezes movement
        ambientSound.Pause(); // Mutes background music

        // Disable Camera Movement
        if (playerController != null)
            playerController.enabled = false;

        Cursor.visible = true; // Show cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
    }

    void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume movement
        ambientSound.Play(); // Resume background music

        // Enable Camera Movement
        if (playerController != null)
            playerController.enabled = true;

        Cursor.visible = false; // Hide cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
    }
}
