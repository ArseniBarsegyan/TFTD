using Assets.Scripts.Messaging;
using System;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    private const float FightEnterDistance = 0.05f;

    public Guid Id;
    public InterceptorType InterceptorType;
    public InterceptorWeapon InterceptorWeapon;
    public InterceptorStatus InterceptorStatus;
    public float StartSpeed = 5.0f;
    public float Speed;
    public int Health = 100;
    public Vector3 StartPoint;
    public Vector3 DestinationPoint;
    public GameObject alienTarget;

    private bool _wasEngageAsked;

    void Start()
    {
        transform.position = StartPoint;
        transform.LookAt(Vector3.zero);
        Speed = StartSpeed;
    }

    void Update()
    {
        if (InterceptorStatus == InterceptorStatus.Repairing
            || InterceptorStatus == InterceptorStatus.Refueling
            || InterceptorStatus == InterceptorStatus.Crashed
            || InterceptorStatus == InterceptorStatus.Reloading
            || InterceptorStatus == InterceptorStatus.Transfering)
        {
            return;
        }

        if (InterceptorStatus == InterceptorStatus.Destroyed)
        {
            Destroy(gameObject);
            return;
        }

        if (InterceptorStatus == InterceptorStatus.Moving)
        {
            if (alienTarget != null)
            {
                PursuitTarget();
            }
            else
            {
                MoveToSelectedGlobePosition();
            }
        }
    }

    private void PursuitTarget()
    {
        var alienSub = alienTarget.GetComponent<AlienSub>();
        if (alienSub != null)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                transform.position = Vector3.RotateTowards(transform.position,
                alienSub.transform.position,
                Time.deltaTime * Speed * 0.01f,
                0f);

                transform.LookAt(Vector3.zero);

                TryEngageAlienSub(alienSub);
            }
        }
    }

    private void TryEngageAlienSub(AlienSub alienSub)
    {
        if (_wasEngageAsked)
            return;

        float distance = Vector3.Distance(transform.position, alienSub.transform.position);

        if (distance > FightEnterDistance)
            return;

        MessagingCenter.Send(this, GameEvent.EngageConfirmed, Id);

        _wasEngageAsked = true;
    }

    private void MoveToSelectedGlobePosition()
    {
        transform.position = Vector3.RotateTowards(transform.position,
                DestinationPoint,
                Time.deltaTime * Speed * 0.01f,
                0f);

        transform.LookAt(Vector3.zero);
    }
}
