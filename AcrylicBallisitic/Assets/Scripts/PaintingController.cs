using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(PaintingMovement))]
public class PaintingController : MonoBehaviour
{
    PaintingMovement movement;
    PaintingMovementArea movementArea;

    bool isAttacking = false;

    public bool IsAttacking() { return isAttacking; }

    public IEnumerator Spawn()
    {
        Vector3 position = movementArea.GetRandomPosition(out Vector3 normal);
        movement.Emerge(position, normal);
        while (movement.IsEmerging())
        {
            yield return null;
        }
        isAttacking = true;
    }

    public void Despawn()
    {
        isAttacking = false;
        movement.Disappear();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PaintingMovement>();
        movementArea = GameManager.GetManager().GetMovementArea();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
