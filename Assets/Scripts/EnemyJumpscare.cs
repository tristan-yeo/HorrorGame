using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyJumpscare : MonoBehaviour
{
    public GameObject jumpscareEnemyPrefab;
    public AudioClip[] audioClips;
    public float jumpscareDuration = 1f;
    public Vector3 jumpscarePositionOffset = new Vector3(0, 0, 0);
    public Vector3 jumpscareRotationOffset = new Vector3(0, 0, 0);

    private bool hasTriggered = false;

    void OnTriggerEnter (Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(DoJumpscare(other.transform));
        }
    }

    IEnumerator DoJumpscare (Transform player)
    {
        // Stop player controls
        FirstPersonController controller = player.GetComponent<FirstPersonController>();
        if (controller != null) controller.enabled = false;

        // Save enemy position & rotation before disabling components
        Vector3 enemyPosition = transform.position;
        Quaternion enemyRotation = transform.rotation;

        // Disable the real enemy
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().enabled = false;

        // Spawn the jumpscare enemy
        Vector3 spawnPos = enemyPosition + transform.TransformDirection(jumpscarePositionOffset);
        spawnPos.y = 1.2f;

        Vector3 lookDirection = (player.position - spawnPos).normalized;
        lookDirection.y = 0f; // Optional: Keep it horizontal to avoid tilting
        Quaternion spawnRot = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(jumpscareRotationOffset);

        GameObject jumpscareEnemy = Instantiate(jumpscareEnemyPrefab, spawnPos, spawnRot);

        // Make the camera look at the enemy
        Transform cam = Camera.main.transform;
        Transform faceTarget = jumpscareEnemy.transform.Find("FaceTarget");

        if (faceTarget != null)
        {
            Vector3 direction = (faceTarget.position - cam.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            cam.rotation = lookRotation;
        }
        else
        {
            Debug.LogWarning("FaceTarget not found on enemy");
        }
        // Wait for animation to complete
        yield return new WaitForSeconds(jumpscareDuration);

        // Shake Camera
        Shake shakeScript = player.GetComponent<Shake>();
        if (shakeScript != null)
        {
            shakeScript.start = true;
            yield return new WaitForSeconds(shakeScript.duration - 0.3f);
        }
        else
        {
            Debug.LogWarning("Shake script not found on player");
        }

        // Simulate player falling
        Quaternion startRotation = cam.rotation;
        Quaternion targetRotation = Quaternion.Euler(-80f, cam.eulerAngles.y, 0f);
        float fallDuration = 0.7f;
        float t = 0f;
        while (t < fallDuration)
        {
            t += Time.deltaTime;
            cam.rotation = Quaternion.Slerp(startRotation, targetRotation, t / fallDuration);
            yield return null;
        }

        Quaternion sideRotation = Quaternion.Euler(-80f, cam.eulerAngles.y, 15f); // Tilt to the side
        Vector3 dropPosition = cam.position + new Vector3(0, -1f, 0); // Drop the camera

        float dropDuration = 0.7f;
        float t2 = 0f;
        Vector3 initialPos = cam.position;
        Quaternion initialRot = cam.rotation;

        while (t2 < dropDuration)
        {
            t2 += Time.deltaTime;
            cam.position = Vector3.Lerp(initialPos, dropPosition, t2 / dropDuration);
            cam.rotation = Quaternion.Slerp(initialRot, sideRotation, t2 / dropDuration);
            yield return null;
        }

        // Shake Camera
        if (shakeScript != null)
        {
            shakeScript.start = true;
            yield return new WaitForSeconds(shakeScript.duration);
        }
        else
        {
            Debug.LogWarning("Shake script not found on player");
        }

        // Optional: End game, reload scene, fade out, etc.
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
