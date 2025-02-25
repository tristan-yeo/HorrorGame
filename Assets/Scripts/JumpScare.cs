using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public GameObject jumpScareImg; // The scary image
    public AudioSource jumpScareSound; // Scary sound effect
    public GameObject gameOverScreen; // The Game Over UI
    public MonoBehaviour playerController; // Player movement script

    private bool isTriggered = false;

    void Start()
    {
        // Ensure UI elements are hidden at the start
        jumpScareImg.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true; // Prevents re-triggering
            StartCoroutine(TriggerJumpScare());
        }
    }

    IEnumerator TriggerJumpScare()
    {
        // Show jumpscare image instantly
        jumpScareImg.SetActive(true);

        // Play jumpscare sound immediately
        if (jumpScareSound != null)
        {
            jumpScareSound.Play();
        }

        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Wait for jumpscare duration
        yield return new WaitForSeconds(2f);

        // Hide jumpscare and show Game Over Screen
        jumpScareImg.SetActive(false);
        gameOverScreen.SetActive(true);

        // Unlock cursor for menu interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Restart Level
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Ensure time resumes
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the scene
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); // Quits the application
    }
}
