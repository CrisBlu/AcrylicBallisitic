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

    int lastSpawnIndex = -1;
    int playerHitPoints = 6;

    //Iterator for the bullet we are on
    private int iBullet = 5;


   
    Ammo[] playerAmmo = new Ammo[6];

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