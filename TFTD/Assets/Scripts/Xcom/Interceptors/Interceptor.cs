using Assets.Scripts.Messaging;

using System;

using UnityEngine;

public class Interceptor : MonoBehaviour
{
    private const float InteractionEnterDistance = 0.05f;

    public string Name;
    public Guid Id;
    public InterceptorType InterceptorType;
    public InterceptorWeapon InterceptorWeapon;
    public InterceptorStatus InterceptorStatus;
    public float StartSpeed = 5.0f;
    public float Speed;
    public float Health = 100;
    public Vector3 StartPoint;
    public Vector3 DestinationPoint;
    public GameObject alienTarget;

    private bool _wasEngageAsked;
    private bool _isReturningToBase;

    public float Fuel { get; set; }

    void Awake()
    {
        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.EngageConfirmRequestConfirmed,
            (engage) =>
            {
                TryEngage();
            });

        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.EngageConfirmContinuePursuit,
            (engage) =>
            {
                TryContinuePursuit();
            });

        MessagingCenter.Subscribe<EngageConfirm>
            (this, GameEvent.EngageConfirmReturnToBase,
            (engage) =>
            {
                SetDistanationPointAsBase();
            });

        MessagingCenter.Subscribe<InterceptorContextMenu>
            (this, GameEvent.InterceptorContextMenuEngageConfirmed,
            (contextMenu) =>
            {
                TryEngage();
            });

        MessagingCenter.Subscribe<InterceptorContextMenu>
            (this, GameEvent.InterceptorContextMenuContinueAction,
            (contextMenu) =>
            {
            });

        MessagingCenter.Subscribe<InterceptorContextMenu>
            (this, GameEvent.InterceptorContextMenuReturnToBaseAction,
            (contextMenu) =>
            {
                SetDistanationPointAsBase();
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.EngageConfirmRequestConfirmed);
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.EngageConfirmContinuePursuit);
        MessagingCenter.Unsubscribe<EngageConfirm>(this, GameEvent.EngageConfirmReturnToBase);

        MessagingCenter.Unsubscribe<InterceptorContextMenu>(this, GameEvent.InterceptorContextMenuEngageConfirmed);
        MessagingCenter.Unsubscribe<InterceptorContextMenu>(this, GameEvent.InterceptorContextMenuContinueAction);
        MessagingCenter.Unsubscribe<InterceptorContextMenu>(this, GameEvent.InterceptorContextMenuReturnToBaseAction);
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

    private void SetDistanationPointAsBase()
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

        ConsumeFuel();
        if (Fuel <= 0)
        {
            SetDistanationPointAsBase();
        }

        if (InterceptorStatus == InterceptorStatus.Moving
            && _isReturningToBase)
        {
            MoveToDestinationPoint();
            TryReturnToHangar();
            return;
        }

        if (InterceptorStatus == InterceptorStatus.Moving)
        {            
            if (Fuel <= 0)
            {
                SetDistanationPointAsBase();
            }

            if (alienTarget != null)
            {
                PursuitTarget();
            }
            else
            {
                MoveToDestinationPoint();
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

        if (distance > InteractionEnterDistance)
            return;

        var interceptorDto = new InterceptorDto
        {
            Id = this.Id,
            Name = this.Name,
            InterceptorType = this.InterceptorType
        };

        MessagingCenter.Send(this, GameEvent.EngageConfirmInterceptorEngageRequest, interceptorDto);

        _wasEngageAsked = true;
    }

    private void MoveToDestinationPoint()
    {
        transform.position = Vector3.RotateTowards(transform.position,
                DestinationPoint,
                Time.deltaTime * Speed * 0.01f,
                0f);

        transform.LookAt(Vector3.zero);
    }

    private void ConsumeFuel()
    {
        if (Fuel <= 0)
            return;

        Fuel -= Time.deltaTime;
    }

    private void TryReturnToHangar()
    {
        if (_isReturningToBase)
        {
            float distance = Vector3.Distance(transform.position, DestinationPoint);

            if (distance > InteractionEnterDistance)
                return;
        }

        _isReturningToBase = false;

        MessagingCenter.Send(this, GameEvent.InterceptorReturnedToBase);
    }
}
