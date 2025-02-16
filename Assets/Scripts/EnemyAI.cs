using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolRadius = 10f; // Radius for random patrol points
    public float soundReactionTime = 3f; // Time to linger at the sound location
    public float detectionRange = 15f; // Range within which sound is detected

    private NavMeshAgent agent;
    private Vector3 lastHeardSoundLocation;
    private bool isReactingToSound = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isReactingToSound)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Generate a random patrol point within the patrol radius
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void HearSound(Vector3 soundLocation)
    {
        if (Vector3.Distance(transform.position, soundLocation) <= detectionRange)
        {
            StopCoroutine("ReactToSound"); // Cancel any ongoing reaction
            lastHeardSoundLocation = soundLocation;
            isReactingToSound = true;
            agent.SetDestination(soundLocation);
            StartCoroutine(ReactToSound());
        }
    }

    private System.Collections.IEnumerator ReactToSound()
    {
        // Wait until the enemy reaches the sound location
        while (!agent.pathPending && agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        // Linger at the sound location for a while
        yield return new WaitForSeconds(soundReactionTime);

        // Resume patrolling
        isReactingToSound = false;
        Patrol();
    }
}