using UnityEngine;

public class PaintingMovementArea : MonoBehaviour
{
    [SerializeField] Vector3 bounds; // full size of the area
    [SerializeField] float padding = 3.0f;

    Vector3 center;

    // Vector3 randomPosition;
    // Vector3 randomNormal;
    // float timer = 0.5f;

    public Vector3 GetRandomPosition(out Vector3 outNormal)
    {
        outNormal = Vector3.zero;
        Vector3 randomPosition = center + new Vector3(
            Random.Range(-bounds.x / 2 + padding, bounds.x / 2 - padding),
            Random.Range(-bounds.y / 2 + padding, bounds.y / 2 - padding),
            Random.Range(-bounds.z / 2 + padding, bounds.z / 2 - padding)
        );
        int randomAxisIndex = Random.Range(0, 2);
        if (randomAxisIndex == 1) // project to z plane
        {
            outNormal = Vector3.forward;
            randomAxisIndex = 2;
        }
        else
        {
            outNormal = Vector3.right;
        }
        int randomBoundIndex = Random.Range(0, 2);
        randomPosition[randomAxisIndex] = randomBoundIndex == 0 ? -bounds[randomAxisIndex] / 2 : bounds[randomAxisIndex] / 2;
        outNormal = randomBoundIndex == 0 ? outNormal : -outNormal;
        return randomPosition;
    }

    public Vector3 GetCrossingPositionFromDirection(Vector3 direction, out Vector3 outNormal)
    {
        outNormal = Vector3.zero;
        Vector3 localDir = direction.normalized;
        float xDist = (bounds.x / 2 - padding) / Mathf.Abs(localDir.x);
        float zDist = (bounds.z / 2 - padding) / Mathf.Abs(localDir.z);
        if (xDist < zDist)
        {
            outNormal = localDir.x > 0 ? Vector3.right : Vector3.left;
            return center + new Vector3(Mathf.Sign(localDir.x) * bounds.x / 2, 0.0f, localDir.z * xDist);
        }
        else
        {
            outNormal = localDir.z > 0 ? Vector3.forward : Vector3.back;
            return center + new Vector3(localDir.x * zDist, 0.0f, Mathf.Sign(localDir.z) * bounds.z / 2);
        }
    }

    public bool IsOutOfBounds(Vector3 position)
    {
        Vector3 localPos = position - center;
        return Mathf.Abs(localPos.x) > bounds.x / 2 - padding && Mathf.Abs(localPos.z) > bounds.z / 2 - padding;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // timer -= Time.deltaTime;
        // if (timer <= 0.0f)
        // {
        //     randomPosition = GetRandomPosition(out randomNormal);
        //     timer = 0.5f;
        // }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bounds);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(randomPosition, 1.0f);
        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(randomPosition, randomPosition + randomNormal * 3.0f);
    }
}