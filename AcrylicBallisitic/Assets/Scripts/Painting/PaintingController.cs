using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(PaintingMovement))]
public class PaintingController : MonoBehaviour
{
    PaintingMovement movement;

    public void Spawn()
    {
        if (movement.GetState() != PaintingMovement.State.None) return;
        movement.Emerge();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PaintingMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: shoot projectiles here
    }
}
