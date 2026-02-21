using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<PaintingController> paintings;
    [SerializeField] PaintingMovementArea movementArea;
    [SerializeField] UIManager uiManager;
    [SerializeField] Movement player;

    public PaintingMovementArea GetMovementArea() { return movementArea; }

    public Vector3 GetPlayerPosition() { return player.transform.position; } // TODO: implement player tracking
    public int GetPlayerHitPoints() { return playerHitPoints; } // TODO: implement player hit points tracking
    public Ammo[] GetPlayerAmmo() { return playerAmmo; } 

    public int GetPlayerAmmoCount()
    {
        int count = 0;
        for(int i = 0; i < 6; i ++)
        {
            if (playerAmmo[i] == Ammo.Loaded)
                count++;
        }

        return count;
    }
    
    static public GameManager GetManager() { return instance; }
    static GameManager instance;

    float previousNetWorth = 0.0f;
    bool isDamageCleared = true;
    bool isDamageDecaying = false;
    float damageDecayDelay = 1.0f;
    float damageDecayTimer = 0.0f;

    int lastSpawnIndex = -1;
    int playerHitPoints = 6;

    //Iterator for the bullet we are on
    private int iBullet = 5;



    Ammo[] playerAmmo;

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
        InputSystem.actions.Enable();
        instance = this;

        playerAmmo = new Ammo[6];
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
            while (paintings.Count > 1 && randomIndex == lastSpawnIndex)
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

    public void DamagePlayer()
    {
        
        playerHitPoints = Mathf.Max(0, playerHitPoints - 1);
        uiManager.UpdatePlayerHitPoints(playerHitPoints);
    }

    public void UpdateBullets(Ammo[] playerAmmo)
    {
        uiManager.UpdatePlayerAmmo(playerAmmo);
    }

    public void UseBullet(bool hit)
    {
        playerAmmo[iBullet] = hit ? Ammo.Hit : Ammo.Miss;
        iBullet--;
        uiManager.UpdatePlayerAmmo(playerAmmo);
    }

    public void ReloadBullet()
    {
        iBullet++;
        playerAmmo[iBullet] = Ammo.Loaded;
        uiManager.UpdatePlayerAmmo(playerAmmo);

    }


}

public enum Ammo
{
    Loaded,
    Hit,
    Miss
}