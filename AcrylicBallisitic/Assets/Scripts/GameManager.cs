using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<PaintingController> paintings;
    [SerializeField] PaintingMovementArea movementArea;
    [SerializeField] UIManager uiManager;

    public PaintingMovementArea GetMovementArea() { return movementArea; }

    public Vector3 GetPlayerPosition() { return Vector3.zero; } // TODO: implement player tracking
    public int GetPlayerHitPoints() { return playerHitPoints; } // TODO: implement player hit points tracking
    public int GetPlayerAmmo() { return playerAmmo; } // TODO: implement player ammo tracking
    
    static public GameManager GetManager() { return instance; }
    static GameManager instance;

    int lastSpawnIndex = -1;
    int playerHitPoints = 6;
    int playerAmmo = 6;

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
        uiManager.UpdatePlayerHitPoints(playerHitPoints);
        uiManager.UpdatePlayerAmmo(playerAmmo);
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerHitPoints = Mathf.Max(0, playerHitPoints - 1);
            uiManager.UpdatePlayerHitPoints(playerHitPoints);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAmmo = Mathf.Max(0, playerAmmo - 1);
            uiManager.UpdatePlayerAmmo(playerAmmo);
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