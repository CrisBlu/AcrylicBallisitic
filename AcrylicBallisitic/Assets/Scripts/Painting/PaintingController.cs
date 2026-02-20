using UnityEngine;

[RequireComponent(typeof(PaintingMovement))]
public class PaintingController : MonoBehaviour
{
    PaintingMovement movement;

    [SerializeField] float maxHealth = 300.0f;
    
    float health;

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
            // TODO: shoot projectiles here
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Hit player");
        }
    }
}
