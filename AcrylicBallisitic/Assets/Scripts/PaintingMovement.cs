using UnityEngine;
using PrimeTween;
using System.Diagnostics;

public class PaintingMovement : MonoBehaviour
{
    bool isEmerging = false;

    Vector3 targetPosition;
    Vector3 startPosition;

    public bool IsEmerging() { return isEmerging; }

    public void SetSide()
    {
        
    }

    public void Emerge(Vector3 position, Vector3 normal)
    {
        isEmerging = true;

        targetPosition = position;
        startPosition = targetPosition + Vector3.up * 20.0f;
        transform.SetPositionAndRotation(startPosition, Quaternion.LookRotation(-normal));
        Tween.Position(transform, targetPosition, 1.5f, Ease.InOutCubic).OnComplete(() =>
        {
            isEmerging = false;
        });
    }

    public void Disappear()
    {
        Vector3 position = transform.position;
        position.y += 20.0f;
        isEmerging = true;
        Tween.Position(transform, position, 1.5f, Ease.InOutCubic).OnComplete(() =>
        {
            isEmerging = false;
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosition, 1.0f);
    }
}
