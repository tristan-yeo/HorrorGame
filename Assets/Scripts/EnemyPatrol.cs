using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] corridorWaypoints; // Waypoints along corridors
    public Transform[] rooms; // Room entry points

    private NavMeshAgent agent;
    private Transform currentTarget;
    private Dictionary<Transform, bool> roomVisitStatus = new Dictionary<Transform, bool>();
    private bool exploringRoom = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Mark all rooms as unexplored initially
        foreach (Transform room in rooms)
        {
            roomVisitStatus[room] = false;
        }

        // Start patrolling in corridors
        PickNextCorridorWaypoint();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (exploringRoom)
            {
                exploringRoom = false;
                PickNextCorridorWaypoint(); // Return to corridor patrol
            }
            else
            {
                if (ShouldExploreRoom())
                {
                    PickRoomToExplore();
                }
                else
                {
                    PickNextCorridorWaypoint();
                }
            }
        }
    }

    void PickNextCorridorWaypoint()
    {
        if (corridorWaypoints.Length == 0) return;
        currentTarget = corridorWaypoints[Random.Range(0, corridorWaypoints.Length)];
        agent.SetDestination(currentTarget.position);
    }

    bool ShouldExploreRoom()
    {
        return Random.value < 0.5f; // 50% chance to explore a room at each waypoint
    }

    void PickRoomToExplore()
    {
        Transform selectedRoom = SelectWeightedRandomRoom();
        if (selectedRoom != null)
        {
            agent.SetDestination(selectedRoom.position);
            exploringRoom = true;
            roomVisitStatus[selectedRoom] = true;
        }
        else
        {
            PickNextCorridorWaypoint();
        }
    }

    Transform SelectWeightedRandomRoom()
    {
        List<Transform> unexploredRooms = new List<Transform>();
        List<Transform> exploredRooms = new List<Transform>();

        foreach (var room in roomVisitStatus)
        {
            if (!room.Value)
                unexploredRooms.Add(room.Key);
            else
                exploredRooms.Add(room.Key);
        }

        if (unexploredRooms.Count > 0)
        {
            return unexploredRooms[Random.Range(0, unexploredRooms.Count)];
        }
        else if (exploredRooms.Count > 0)
        {
            return exploredRooms[Random.Range(0, exploredRooms.Count)];
        }
        return null;
    }
}
