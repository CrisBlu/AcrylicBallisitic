using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<PaintingController> paintings;
    [SerializeField] PaintingMovementArea movementArea;

    public Vector3 GetRandomPosition(out Vector3 outNormal) { return movementArea.GetRandomPosition(out outNormal); }
    public bool IsOutOfBounds(Vector3 position) { return movementArea.IsOutOfBounds(position); }

    static public GameManager GetManager() { return instance;}

    static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (PaintingController painting in paintings)
            {
                painting.Spawn();
            }
        }
    }
}