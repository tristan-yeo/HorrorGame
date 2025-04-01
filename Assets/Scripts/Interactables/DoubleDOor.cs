using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [SerializeField] private Animator door = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorClose", 0, 0.0f);
        }
    }
}
