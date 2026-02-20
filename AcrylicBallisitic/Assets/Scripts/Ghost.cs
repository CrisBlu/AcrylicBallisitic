using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [Header("Timing")]
    public float attackInterval = 4f;
    public float fuseTime = 1.5f;

    [Header("Area Settings")]
    public float blastRadius = 5f;
    public float minSpawnDist = 3f;
    public float maxSpawnDist = 6f;

    private Transform playerTransform;

    void Start()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
       // if (pObj) player = pObj.transform;

        // Hide the telegraph visual initially
      //  if (telegraphVisual) telegraphVisual.SetActive(false);
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            if (playerTransform != null)
            {
                // 1. Calculate Random Position around player
                Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnDist, maxSpawnDist);
                Vector3 targetPos = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            }
        }
    }
}

