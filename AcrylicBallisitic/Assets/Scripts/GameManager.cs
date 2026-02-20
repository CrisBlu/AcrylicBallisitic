using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PaintingController painting;
    [SerializeField] PaintingMovementArea movementArea;

    public PaintingMovementArea GetMovementArea() { return movementArea; }

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
            StartCoroutine(painting.Spawn());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            painting.Despawn();
        }
    }
}