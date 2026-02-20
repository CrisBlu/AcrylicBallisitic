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

    public GameObject telegraphVisual; // A flat disk or sphere child object

    private Transform playerTransform;

    void Start()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj) playerTransform = pObj.transform;
        if (telegraphVisual) telegraphVisual.SetActive(false);
        StartCoroutine(AttackLoop());
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

                telegraphVisual.transform.position = targetPos - Vector3.up * 0.5f; // Slight floor offset
                telegraphVisual.SetActive(true);
                telegraphVisual.transform.localScale = Vector3.one * 0.1f;

                //float up
                float elapsed = 0;
                while (elapsed < fuseTime)
                {
                    elapsed += Time.deltaTime;
                    telegraphVisual.transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one * blastRadius, elapsed / fuseTime);
                    telegraphVisual.transform.position += Vector3.up * (Time.deltaTime / fuseTime);
                    yield return null;
                }
                Explode(targetPos);
            }
        }

        void Explode(Vector3 center)
        {
            Debug.Log("Boom");
            Collider[] hits = Physics.OverlapSphere(center, blastRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Debug.Log("Player Caught in Blast!");
                }
            }
        }
    }
}

