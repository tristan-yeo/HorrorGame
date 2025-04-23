using UnityEngine;
using UnityEngine.AI;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 6f;

    private NavMeshAgent agent;
    private float nextPlayTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ScheduleNextSound();
    }

    void Update()
    {
        if (agent != null && agent.velocity.magnitude > 0.1f && Time.time >= nextPlayTime)
        {
            PlayRandomSound();
            ScheduleNextSound();
        }
    }

    void PlayRandomSound()
    {
        if (audioClips.Length > 0 && SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(audioClips, transform, 1f);
        }
    }

    void ScheduleNextSound()
    {
        nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}
