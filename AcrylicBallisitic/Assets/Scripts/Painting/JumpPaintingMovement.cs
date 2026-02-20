using UnityEngine;
using PrimeTween;

public class JumpPaintingMovement : PaintingMovement
{
    [SerializeField] int jumpCount = 3;
    [SerializeField] float jumpSpeed = 1.5f;
    [SerializeField] float idleDuration = 2.5f;

    int jumpsDone = 0;
    int extraSpins = 0;
    float idleTimer = 0.0f;

    protected override void Idle()
    {
        base.Idle();
        idleTimer = idleDuration;
    }

    override protected bool ShouldMove()
    {
        return state == State.Idle && idleTimer <= 0.0f && jumpsDone < jumpCount;
    }

    override protected void Move()
    {
        base.Move();
        Vector3 randomPosition = Vector3.zero;
        Vector3 randomNormal = normal;
        while (Vector3.Dot(randomNormal, normal) > 0.2f)
        {
            randomPosition = game.GetRandomPosition(out randomNormal);
        }

        float distance = Vector3.Distance(transform.position, randomPosition);
        if (distance <= 10.0f) extraSpins = 0;
        else if (distance <= 25.0f) extraSpins = 1;
        else if (distance <= 45.0f) extraSpins = 2;
        else extraSpins = 3;

        Tween.PositionAtSpeed(transform, randomPosition, jumpSpeed, Ease.InOutCubic)
        .OnUpdate(transform, (target, tween) =>
        {
            Quaternion currentRot = Quaternion.LookRotation(-normal);
            Quaternion targetRot = Quaternion.LookRotation(-randomNormal);

            float t = tween.interpolationFactor;
            Quaternion baseRot = Quaternion.Slerp(currentRot, targetRot, t);

            float rollDegrees = 360.0f * extraSpins * t;
            Quaternion roll = Quaternion.AngleAxis(rollDegrees, Vector3.up);

            // roll around the facing axis (local forward)
            transform.rotation = baseRot * roll;
        })
        .OnComplete(() =>
        {
            normal = randomNormal;
            jumpsDone++;
            Idle();
        });
        transform.rotation = Quaternion.LookRotation(-randomNormal);
        // Tween.Rotation(transform, Quaternion.LookRotation(-randomNormal), distance / jumpSpeed, Ease.InOutCubic);
    }

    override protected void Update()
    {
        base.Update();
        if (state == State.Idle)
        {
            idleTimer -= Time.deltaTime;
        }
    }

    protected override bool ShouldDisappear()
    {
        return state == State.Idle && idleTimer <= 0.0f && jumpsDone >= jumpCount;
    }

    protected override void Disappear()
    {
        base.Disappear();
        jumpsDone = 0;
    }
}
