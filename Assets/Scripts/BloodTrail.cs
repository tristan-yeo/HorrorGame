using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrail : MonoBehaviour
{
    public GameObject[] bloodPrefabs;
    public float spawnInterval = 1f;
    public float bloodLifespan = 5f;
    private float timer = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (animator.GetBool("isWalking") && timer >= spawnInterval)
        {
            SpawnBlood();
            timer = 0f;
        }
    }

    void SpawnBlood()
    {
        if (bloodPrefabs.Length == 0) return;
        GameObject randomBloodPrefab = bloodPrefabs[Random.Range(0, bloodPrefabs.Length)];

        Vector3 spawnPos = transform.position + new Vector3(0f, 0.01f, 0f);
        GameObject blood = Instantiate(
            randomBloodPrefab,
            spawnPos,
            Quaternion.Euler(0f, Random.Range(0, 360f), 0f)
        );

        Destroy(blood, bloodLifespan);
    }
}
