using Assets.Scripts.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorsController : MonoBehaviour
{
    private const int DefaultInterceptorHealth = 100;
    private const InterceptorType DefaultInterceptorType = InterceptorType.Barracuda;
    private const float DefaultInteceptorSpeed = 2.0f;

    [SerializeField] private GameObject interceptorCraftPrefab;

    public List<InterceptorDto> InterceptorsList { get; } = new List<InterceptorDto>();

    void Start()
    {
        MessagingCenter.Subscribe<SelectInterceptor, TargetSelectedDto>
            (this, GameEvent.InterceptorSelectConfirmed,
            (component, dto) =>
            {
                StartIntercept(dto);
            });

        CreateNewDefaultInterceptor();
        CreateNewDefaultInterceptor();
    }

    public void CreateNewDefaultInterceptor()
    {
        var dto = new InterceptorDto
        {
            Id = Guid.NewGuid(),
            Health = DefaultInterceptorHealth,
            InterceptorType = DefaultInterceptorType,
            Speed = DefaultInteceptorSpeed,
            StartPoint = Vector3.zero,
            Status = InterceptorStatus.New,
            Weapon = InterceptorWeapon.SubRockets
        };
        InterceptorsList.Add(dto);
    }

    public void StartIntercept(TargetSelectedDto dto)
    {
        GameObject obj = Instantiate(interceptorCraftPrefab);
        var interceptor = obj.GetComponent<Interceptor>();
        if (interceptor != null)
        {
            GameObject alienSubTarget = AlienSubsController.GetAlienSubById(dto.TargetId);
            if (alienSubTarget != null)
            {
                var startPoint = XComObjectsController.BasesController.GetXComBase().Location;
                interceptor.InterceptorStatus = InterceptorStatus.Moving;
                interceptor.StartPoint = startPoint;
                interceptor.Id = dto.SpaceCraftId;
                interceptor.alienTarget = alienSubTarget;
            }
        }
    }
}
