using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PaintingMovement))]
public class PaintingController : MonoBehaviour
{
    PaintingMovement movement;

    [SerializeField] float maxHealth = 300.0f;
    [SerializeField] List<GameObject> projectilePrefabs;
    [SerializeField] float projectileSpawnInterval = 2.0f;
    
    float health;
    float projectileSpawnTimer = 0.0f;

    public void Spawn()
    {
        if (movement.GetState() != PaintingMovement.State.None) return;
        movement.Emerge();
    }

    public void DoDamage(float damage)
    {
        // if (movement.GetState() == PaintingMovement.State.Idle ||
        //     movement.GetState() == PaintingMovement.State.Moving)
        {
            health -= damage;
            GameManager.GetManager().NotifyDamageDealt(damage);
            if (health <= 0.0f)
            {
                // TODO: death
            }
        }
    }

    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PaintingMovement>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.GetState() == PaintingMovement.State.Moving)
        {
            if (projectilePrefabs != null && projectilePrefabs.Count > 0)
            {
                projectileSpawnTimer += Time.deltaTime;
                if (projectileSpawnTimer >= projectileSpawnInterval)
                {
                    int randomIndex = Random.Range(0, projectilePrefabs.Count);
                    GameObject projectilePrefab = projectilePrefabs[randomIndex];
                    Vector3 playerPosition = GameManager.GetManager().GetPlayerPosition();
                    Vector3 toPlayer = playerPosition - transform.position;
                    toPlayer.y = 0.0f;
                    Instantiate(projectilePrefab, transform.position + toPlayer.normalized * 2.0f, Quaternion.LookRotation(toPlayer));
                    projectileSpawnTimer = 0.0f;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            GameManager.GetManager().DamagePlayer();
        }
    }
}
