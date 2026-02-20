using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

public enum ProjectileType { Single, Spread, Burst }

public class HorseProjectile : MonoBehaviour
{
    [Header("General Settings")]
    public ProjectileType ProjectileType = ProjectileType.Single;
    public float Speed = 40f;
    public float LifeSpan = 3f;

    [Header("Behavior Data")]
    public int burstCount = 3;
    public float burstInterval = 0.1f;
    public float spreadAngle = 15f;

    [HideInInspector] public bool isSubProjectile = false;

    private Vector3 spawnLocation;
    private Quaternion spawnRotation;

    void Start()
    {
        spawnLocation = transform.position;
        spawnRotation = transform.rotation;
        Destroy(gameObject, LifeSpan);
        if (!isSubProjectile)
        {
            ExecuteBehavior();
        }
    }

    void Update()
    {
       transform.position = transform.position + transform.forward * Speed * Time.deltaTime;
    }

    void ExecuteBehavior()
    {
        switch (ProjectileType)
        {
            case ProjectileType.Spread:
                //change to how many pellets entry
                SpawnCopy(-spreadAngle);
                SpawnCopy(spreadAngle);
                break;
            case ProjectileType.Burst:
                StartCoroutine(BurstRoutine());
                break;
        }
    }

    void SpawnCopy(float angleOffset)
    {
        Quaternion rotation = spawnRotation * Quaternion.Euler(0, angleOffset, 0);
        GameObject copy = Instantiate(gameObject, spawnLocation, rotation);

        copy.GetComponent<HorseProjectile>().isSubProjectile = true;
    }

    IEnumerator BurstRoutine()
    {
        for (int i = 0; i < burstCount - 1; i++) // -1 because the first one already exists
        {
            yield return new WaitForSeconds(burstInterval);
            SpawnCopy(0);
        }
    }
}


