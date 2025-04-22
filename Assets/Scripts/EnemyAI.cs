using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolRadius = 10f;
    public float sightRange = 20f;          // How far the enemy can see
    public float sightAngle = 60f;          // Field of view angle
    public Transform player;                // Assign player in inspector
    public LayerMask obstructionMask;       // Layers considered obstacles
    public Animator animator;

    private NavMeshAgent agent;
    private Vector3 lastHeardSoundLocation;
    private bool isReactingToSound = false;
    private bool isChasingPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    void Update()
    {
        bool isMoving = agent.hasPath && agent.remainingDistance > 0.1f;
        animator.SetBool("isWalking", isMoving);

        if (agent.velocity.magnitude > 0.1f)
        {
            transform.forward = agent.velocity.normalized;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isReactingToSound && !isChasingPlayer)
        {
            Patrol();
        }

        // Check for player in sight
        if (CanSeePlayer())
        {
            isChasingPlayer = true;
            agent.SetDestination(player.position);
        }
        else
        {
            isChasingPlayer = false;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= sightRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < sightAngle / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out RaycastHit hit, sightRange, obstructionMask))
                {
                    return true;
                }
                else if (hit.transform == player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void Patrol()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void HearSound(Vector3 soundLocation)
    {
        StopCoroutine("ReactToSound");
        lastHeardSoundLocation = soundLocation;
        isReactingToSound = true;
        agent.SetDestination(soundLocation);
        StartCoroutine(ReactToSound());
    }

    private IEnumerator ReactToSound()
    {
        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        isReactingToSound = false;
        Patrol();
    }
}
