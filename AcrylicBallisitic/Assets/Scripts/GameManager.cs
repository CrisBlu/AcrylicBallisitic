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

    float previousNetWorth = 0.0f;
    bool isDamageCleared = true;
    bool isDamageDecaying = false;
    float damageDecayDelay = 1.0f;
    float damageDecayTimer = 0.0f;

    int lastSpawnIndex = -1;
    int playerHitPoints = 6;
    int playerAmmo = 6;

    public float GetNetWorth()
    {
        float total = 0.0f;
        foreach (PaintingController painting in paintings)
        {
            total += painting.GetHealth();
        }
        return total;
    }

    public float GetMaxNetWorth()
    {
        float total = 0.0f;
        foreach (PaintingController painting in paintings)
        {
            total += painting.GetMaxHealth();
        }
        return total;
    }
    
    public void NotifyDamageDealt(float damage)
    {
        if (isDamageCleared)
        {
            isDamageCleared = false;
            previousNetWorth = GetNetWorth() + damage;
        }
        uiManager.UpdateNetWorth(GetNetWorth(), previousNetWorth, damage);
        damageDecayTimer = damageDecayDelay;
        isDamageDecaying = false;
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
        if (previousNetWorth > GetNetWorth())
        {
            if (damageDecayTimer > 0.0f)
            {
                damageDecayTimer -= Time.deltaTime;
            }
            else
            {
                isDamageDecaying = true;
            }

            if (isDamageDecaying)
            {
                previousNetWorth -= 50.0f * Time.deltaTime;
                if (previousNetWorth <= GetNetWorth())
                {
                    previousNetWorth = GetNetWorth();
                    isDamageDecaying = false;
                    isDamageCleared = true;
                }
                else
                {
                    uiManager.UpdateNetWorth(GetNetWorth(), previousNetWorth, 0.0f);
                }
            }
        }

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

        // Debug
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

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            paintings[0].DoDamage(50.0f);
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