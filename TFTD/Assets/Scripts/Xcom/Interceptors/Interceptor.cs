using Assets.Scripts.Messaging;
using System;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    private const float FightEnterDistance = 0.05f;

    public string Name;
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
    private bool _isReturningToBase;

    void Awake()
    {
        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.InterceptorEngageRequestConfirmed,
            (engage) =>
            {
                TryEngage();
            });

        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.InterceptorEngageContinuePursuit,
            (engage) =>
            {
                TryContinuePursuit();
            });

        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.InterceptorEngageReturnToBase,
            (engage) =>
            {
                TryReturnToBase();
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.InterceptorEngageRequestConfirmed);
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.InterceptorEngageContinuePursuit);
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.InterceptorEngageReturnToBase);
    }

    private void TryEngage()
    {
        var alienSub = alienTarget.GetComponent<AlienSub>();
        if (alienSub == null)
            return;

        if (InterceptorType == InterceptorType.Triton)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                Debug.Log("Triton can't deploy troops while target is moving");
            }
            else
            {
                Debug.Log("Triton deploying troops");
            }
            return;
        }
        
        if (InterceptorType == InterceptorType.Barracuda)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                Debug.Log("Barracuda starting battle");
            }
            else
            {
                Debug.Log("Barracuda can't deploy troops");
            }
            return;
        }

        if (InterceptorType == InterceptorType.Manta)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                Debug.Log("Manta starting battle");
            }
            else
            {
                Debug.Log("Manta deploying troops");
            }
            return;
        }
    }

    private void TryContinuePursuit()
    {
    }

    private void TryReturnToBase()
    {
        DestinationPoint = StartPoint;
        _isReturningToBase = true;
    }

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

        if (InterceptorStatus == InterceptorStatus.Moving
            && _isReturningToBase)
        {
            MoveToSelectedGlobePosition();
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

                TryRequestForEngagingAlienSub(alienSub);
            }
        }
    }

    private void TryRequestForEngagingAlienSub(AlienSub alienSub)
    {
        if (_wasEngageAsked)
            return;

        float distance = Vector3.Distance(transform.position, alienSub.transform.position);

        if (distance > FightEnterDistance)
            return;

        var interceptorDto = new InterceptorDto
        {
            Id = this.Id,
            Name = this.Name,
            InterceptorType = this.InterceptorType
        };

        MessagingCenter.Send(this, GameEvent.InterceptorEngageRequest, interceptorDto);

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
