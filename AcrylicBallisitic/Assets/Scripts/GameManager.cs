using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioPlayer))]
public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] List<PaintingController> paintings;
    [SerializeField] PaintingMovementArea movementArea;
    [SerializeField] UIManager uiManager;
    [SerializeField] DifficultyProgression difficultyProgression;
    [SerializeField] Movement player;

    [Header("Game Settings")]
    [SerializeField] float maxNetWorth = 900.0f;

    [Header("Audio")]
    [SerializeField] AudioPlayer sfxPlayer;
    [SerializeField] AudioPlayer voiceLinePlayer;

    public PaintingMovementArea GetMovementArea() { return movementArea; }

    public Vector3 GetPlayerPosition() { return player.transform.position; } // TODO: implement player tracking
    public int GetPlayerHitPoints() { return playerHitPoints; } // TODO: implement player hit points tracking
    public Ammo[] GetPlayerAmmo() { return playerAmmo; } 

    public int GetPlayerAmmoCount()
    {
        int count = 0;
        for(int i = 0; i < 6; i ++)
        {
            if (playerAmmo[i] == Ammo.Loaded || playerAmmo[i] == Ammo.PowerUp)
                count++;
        }

        return count;
    }
    
    static public GameManager GetManager() { return instance; }
    static GameManager instance;

    float netWorth = 0.0f;
    float previousNetWorth = 0.0f;
    bool isDamageCleared = true;
    bool isDamageDecaying = false;
    float damageDecayDelay = 1.0f;
    float damageDecayTimer = 0.0f;

    int lastSpawnIndex = -1;
    float spawnInterval = 25.0f;
    float spawnTimer = 0.0f;
    float gracePeriod;

    int playerHitPoints = 6;

    //Iterator for the bullet we are on
    private int iBullet = 5;

    Ammo[] playerAmmo;

    public void SetGracePeriod(float value) { gracePeriod = value; }
    public bool IsGracePeriod() { return gracePeriod > 0.0f; }

    public float GetNetWorth()
    {
        // float total = 0.0f;
        // foreach (PaintingController painting in paintings)
        // {
        //     total += painting.GetHealth();
        // }
        // return total;
        return netWorth;
    }

    public float GetMaxNetWorth()
    {
        // float total = 0.0f;
        // foreach (PaintingController painting in paintings)
        // {
        //     total += painting.GetMaxHealth();
        // }
        // return total;
        return maxNetWorth;
    }
    
    public void NotifyDamageDealt(float damage)
    {
        netWorth = Mathf.Max(0.0f, netWorth - damage);
        if (netWorth <= 0.0f)
        {
            StartCoroutine(EndGame(true));
            return;
        }

        if (isDamageCleared)
        {
            isDamageCleared = false;
            previousNetWorth = GetNetWorth() + damage;
        }
        difficultyProgression.UpdateDifficulty(GetNetWorth() / GetMaxNetWorth());
        uiManager.UpdateNetWorth(GetNetWorth(), previousNetWorth, damage);
        damageDecayTimer = damageDecayDelay;
        isDamageDecaying = false;
    }

    //******************************************************
    // Player tracking and management

    public void DamagePlayer()
    {
        playerHitPoints = Mathf.Max(0, playerHitPoints - 1);
        uiManager.UpdatePlayerHitPoints(playerHitPoints);

        if(playerHitPoints <= 0)
        {
            StartCoroutine(EndGame(false));
        }
        else
        {
            PlaySound("PLAYER_PAIN", 2f);
        }
    }

    IEnumerator EndGame(bool victory)
    {
        if (victory)
        {
            PlayVoiceLine("GHOST_DEATH");
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene(3);
        }
        else   
        {
            // PlayVoiceLine("PLAYER_DEATH");
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(2);
        }
    }

    public void UseBullet(bool hit)
    {
        if (playerAmmo[iBullet] == Ammo.PowerUp) return;
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

    public void AmmoPowerUp()
    {
        iBullet = 5;
        for (int i = 0; i < 6; i++)
        {
            playerAmmo[i] = Ammo.PowerUp;
        }
        uiManager.UpdatePlayerAmmo(playerAmmo);
    }

    public void AmmoPowerDown()
    {
        iBullet = 5;
        for (int i = 0; i < 6; i++)
        {
            playerAmmo[i] = Ammo.Loaded;
        }
        uiManager.UpdatePlayerAmmo(playerAmmo);
    }

    //******************************************************
    // Audio

    public void PlaySound(string name, float volume = 1.0f)
    {
        sfxPlayer?.PlaySound(name, volume);
    }

    public void PlayVoiceLine(string name, float volume = 1.0f)
    {
        print("Playing voice line: " + name);
        voiceLinePlayer?.PlaySound(name, volume);
    }

    bool ShouldSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return true;
        int activeCount = 0;
        foreach (PaintingController painting in paintings)
        {
            if (painting.GetComponent<PaintingMovement>().GetState() != PaintingMovement.State.None)
            {
                activeCount++;
            }
        }
        return activeCount < difficultyProgression.GetSpawnCount();
    }

    void Awake()
    {
        InputSystem.actions.Enable();
        instance = this;

        playerAmmo = new Ammo[6];
    }

    void Start()
    {
        gracePeriod = 8.0f;
        uiManager.UpdatePlayerHitPoints(playerHitPoints);
        uiManager.UpdatePlayerAmmo(playerAmmo);
        difficultyProgression.currentDifficulty = DifficultyProgression.DifficultyLevel.Difficult;
        difficultyProgression.UpdateDifficulty(1f);
        netWorth = GetMaxNetWorth();
        Cursor.visible = false;
        uiManager.UpdateNetWorth(GetNetWorth(), GetNetWorth(), 0.0f);
    }

    void OnDestroy()
    {
        InputSystem.actions.Disable();
        instance = null;
        Tween.StopAll();
        Cursor.visible = true;
    }

    void Update()
    {
        float reticleSize = Mathf.Max(.5f, player.MultiShotPenalty * player.penaltyLevel);
        uiManager.UpdateReticle(reticleSize);

        gracePeriod -= Time.deltaTime;
        if (gracePeriod > 0.0f) return;

        spawnTimer += Time.deltaTime;
        spawnInterval -= Time.deltaTime * 0.05f;

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
                uiManager.UpdateNetWorth(GetNetWorth(), previousNetWorth, 0.0f);
            }
        }

        if (ShouldSpawn())
        {
            spawnTimer = 0.0f;
            int randomIndex = Random.Range(0, paintings.Count);
            while (paintings.Count > 1 && randomIndex == lastSpawnIndex)
            {
                randomIndex = Random.Range(0, paintings.Count);
            }
            paintings[randomIndex].Spawn();
            lastSpawnIndex = randomIndex;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            NotifyDamageDealt(100.0f);
        }
    }

    public void HealPlayer()
    {
        playerHitPoints = Mathf.Max(0, playerHitPoints + 1);
        uiManager.UpdatePlayerHitPoints(playerHitPoints);
    }
}

public enum Ammo
{
    Loaded,
    Hit,
    Miss,
    PowerUp
}