using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<PaintingController> paintings;
    [SerializeField] PaintingMovementArea movementArea;

    public PaintingMovementArea GetMovementArea() { return movementArea; }

    public Vector3 GetPlayerPosition() { return Vector3.zero; } // TODO: implement player tracking
    
    static public GameManager GetManager() { return instance; }
    static GameManager instance;

    int lastSpawnIndex = -1;

    public float GetTotalNetWorth()
    {
        float total = 0.0f;
        foreach (PaintingController painting in paintings)
        {
            total += painting.GetHealth();
        }
        return total;
    }

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
        if (ShouldSpawn())
        {
            int randomIndex = Random.Range(0, paintings.Count);
            while (randomIndex == lastSpawnIndex)
            {
                randomIndex = Random.Range(0, paintings.Count);
            }
            paintings[randomIndex].Spawn();
            lastSpawnIndex = randomIndex;
        }
    }

    bool ShouldSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return true;
        foreach (PaintingController painting in paintings)
        {
            if (painting.GetComponent<PaintingMovement>().GetState() != PaintingMovement.State.None) return false;
        }
        return true;
    }
}