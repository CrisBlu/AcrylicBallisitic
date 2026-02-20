using UnityEngine;
using PrimeTween;
using System.Diagnostics;
using Unity.VisualScripting;

public class PaintingMovement : MonoBehaviour
{
    public enum State
    {
        None,
        Emerging,
        Idle,
        Moving,
        Disappearing
    }

    Vector3 targetPosition;
    Vector3 startPosition;

    protected State state = State.None;
    protected Vector3 normal;

    protected GameManager game;

    public State GetState() { return state; }

    public void Emerge()
    {
        state = State.Emerging;

        Vector3 position = game.GetRandomPosition(out normal);

        targetPosition = position;
        startPosition = targetPosition + Vector3.up * 20.0f;
        transform.SetPositionAndRotation(startPosition, Quaternion.LookRotation(-normal));
        Tween.Position(transform, targetPosition, 1.5f, Ease.InOutCubic).OnComplete(() =>
        {
            state = State.Idle;
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = GameManager.GetManager();
        print(game);
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Emerging:
                break;
            case State.Idle:
                if (ShouldMove())
                {
                    Move();
                }
                break;
            case State.Moving:
                if (ShouldIdle())
                {
                    Idle();
                }
                break;
            case State.Disappearing:
                break;
        }

        if (ShouldDisappear())
        {
            Disappear();
        }
    }   

    virtual protected bool ShouldMove() { return true; }
    virtual protected void Move() { state = State.Moving; }

    virtual protected bool ShouldIdle() { return false; }
    virtual protected void Idle() { state = State.Idle; }

    virtual protected bool ShouldDisappear() { return Input.GetKeyDown(KeyCode.Z); }
    public void Disappear()
    {
        state = State.Disappearing;

        Vector3 position = transform.position;
        position.y += 20.0f;
        Tween.Position(transform, position, 1.5f, Ease.InOutCubic).OnComplete(() =>
        {
            state = State.None;
        });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosition, 1.0f);
    }
}
