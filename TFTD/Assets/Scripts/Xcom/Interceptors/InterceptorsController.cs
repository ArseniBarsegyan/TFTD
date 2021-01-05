using Assets.Scripts.Messaging;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class InterceptorsController : MonoBehaviour
{
    private const int DefaultInterceptorHealth = 100;
    private const float DefaultInteceptorSpeed = 5.0f;

    private int _interceptorsCount;

    [SerializeField] private GameObject interceptorCraftPrefab;

    public List<InterceptorDto> InterceptorsList { get; } = new List<InterceptorDto>();

    void Awake()
    {
        MessagingCenter.Subscribe<SelectInterceptor, TargetSelectedDto>
            (this, GameEvent.InterceptorSelectConfirmed,
            (component, dto) =>
            {
                StartIntercept(dto);
            });
    }

    void Start()
    {
        CreateNewTriton();
        CreateNewDefaultInterceptor();
        CreateNewDefaultInterceptor();
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<SelectInterceptor>(this, GameEvent.InterceptorSelectConfirmed);
    }

    public void CreateNewDefaultInterceptor()
    {
        _interceptorsCount++;

        var dto = new InterceptorDto
        {
            Id = Guid.NewGuid(),
            Name = $"Barracuda-{_interceptorsCount}",
            Health = DefaultInterceptorHealth,
            InterceptorType = InterceptorType.Barracuda,
            Speed = DefaultInteceptorSpeed,
            StartPoint = Vector3.zero,
            Status = InterceptorStatus.New,
            Weapon = InterceptorWeapon.SubRockets
        };
        InterceptorsList.Add(dto);
    }

    public void CreateNewTriton()
    {
        var dto = new InterceptorDto
        {
            Id = Guid.NewGuid(),
            Name = "Triton-01",
            Health = DefaultInterceptorHealth,
            InterceptorType = InterceptorType.Triton,
            Speed = DefaultInteceptorSpeed,
            StartPoint = Vector3.zero,
            Status = InterceptorStatus.New,
            Weapon = InterceptorWeapon.None
        };
        InterceptorsList.Add(dto);
    }

    public void StartIntercept(TargetSelectedDto dto)
    {
        GameObject alienSubTarget = AlienSubsController.GetAlienSubById(dto.TargetId);

        if (alienSubTarget == null)
            return;

        var interceptorDto = InterceptorsList.FirstOrDefault(x => x.Id == dto.SpaceCraftId);

        if (interceptorDto == null)
            return;

        var startPoint = XComObjectsController.BasesController.GetXComBase().Location;

        if (interceptorDto.Status == InterceptorStatus.Refueling)
        {
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.Crashed)
        {
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.Repairing)
        {
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.Transfering)
        {
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.Refueling)
        {
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.Moving)
        {
            // CenterOnInterceptor();
            // SendMessage - ask what actions should be performed with interceptor();
            return;
        }

        if (interceptorDto.Status == InterceptorStatus.New)
        {
            interceptorDto.Status = InterceptorStatus.Moving;
        }

        GameObject obj = Instantiate(interceptorCraftPrefab);
        var interceptor = obj.GetComponent<Interceptor>();

        if (interceptor == null)
            return;

        interceptor.Name = interceptorDto.Name;
        interceptor.Id = interceptorDto.Id;
        interceptor.InterceptorStatus = interceptorDto.Status;
        interceptor.StartPoint = startPoint;
        interceptor.Health = interceptorDto.Health;
        interceptor.InterceptorType = interceptorDto.InterceptorType;
        interceptor.InterceptorWeapon = interceptorDto.Weapon;
        interceptor.StartSpeed = interceptorDto.Speed;
        interceptor.alienTarget = alienSubTarget;
    }
}
