using Assets.Scripts.Messaging;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Interceptor : MonoBehaviour
{
    private const float InteractionEnterDistance = 0.05f;
    private TimeController _timeController;

    public string Name;
    public Guid Id;
    public InterceptorType InterceptorType;
    public InterceptorWeapon InterceptorWeapon;
    public InterceptorStatus InterceptorStatus;
    public float StartSpeed = 1.0f;
    public float Speed;
    public float Health = 100f;
    public Vector3 StartPoint;
    public Vector3 DestinationPoint;
    public GameObject alienTarget;
    public List<SoldierDto> Soldiers;

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
            if (alienSub.SubStatus == AlienSubStatus.Landed 
                || alienSub.SubStatus == AlienSubStatus.Crashed)
            {
                TryDeployTroops();
            }
            return;
        }
        
        if (InterceptorType == InterceptorType.Barracuda)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                TryStartBattle();
            }
            return;
        }

        if (InterceptorType == InterceptorType.Manta)
        {
            if (alienSub.SubStatus == AlienSubStatus.Moving)
            {
                TryStartBattle();
            }
            else if (alienSub.SubStatus == AlienSubStatus.Landed 
                || alienSub.SubStatus == AlienSubStatus.Crashed)
            {
                TryDeployTroops();
            }
        }
    }

    private void TryDeployTroops()
    {
        // TODO: pass mission soldiers to battlescape
        Debug.Log("Deploying troops");
        var soldiersToSend = XComObjectsController.SoldiersController.MissionSoldiers;
        SceneManager.LoadScene("Battlescape");
    }

    private void TryStartBattle()
    {
        Debug.Log("Starting battle");
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
        _timeController = FindObjectOfType<TimeController>();
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
            MoveToPosition(DestinationPoint);
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
                MoveToPosition(DestinationPoint);
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
                MoveToPosition(alienSub.transform.position);
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

    private void MoveToPosition(Vector3 position)
    {
        const float speedMultiplier = 0.01f;
        const float timeSpeedMultiplier = 0.05f;

        transform.position = Vector3.RotateTowards(transform.position,
                position,
                Time.deltaTime * Speed * speedMultiplier * (int)_timeController.TimeSpeed * timeSpeedMultiplier,
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
