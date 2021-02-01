using Assets.Scripts.Messaging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class InterceptorsController : MonoBehaviour
{
    private const float DefaultInterceptorHealth = 100;
    private const float DefaultInteceptorSpeed = 5.0f;
    private const float MinFuelToStartInterception = 10f;

    private int _interceptorsCount;

    [SerializeField] private GameObject interceptorCraftPrefab;

    public List<InterceptorDto> InterceptorsList { get; } = new List<InterceptorDto>();

    void Awake()
    {
        MessagingCenter.Subscribe<SelectInterceptor, TargetSelectedDto>
            (this, GameEvent.SelectInterceptorSelectConfirmed,
            (component, dto) =>
            {
                StartIntercept(dto);
            });

        MessagingCenter.Subscribe<Interceptor>
            (this, GameEvent.InterceptorReturnedToBase,
            (interceptor) => 
            {
                SynchronizeDtoData(interceptor);
                Destroy(interceptor.gameObject);
                Debug.Log($"{interceptor.Name} game object destroyed");
            });
    }

    void Start()
    {
        CreateNewTriton();
        CreateNewDefaultInterceptor();
        CreateNewDefaultInterceptor();
    }

    void Update()
    {
        StartCoroutine(RefuelInterceptorsIfNecessary());
        StartCoroutine(RepairInterceptorsIfNecessary());
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<SelectInterceptor>(this, GameEvent.SelectInterceptorSelectConfirmed);
        MessagingCenter.Unsubscribe<Interceptor>(this, GameEvent.InterceptorReturnedToBase);
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
            Status = InterceptorStatus.Ready,
            Weapon = InterceptorWeapon.SubRockets,
            Fuel = 100
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
            Status = InterceptorStatus.Ready,
            Weapon = InterceptorWeapon.None,
            Fuel = 100
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

        if (interceptorDto.Status == InterceptorStatus.Moving)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            Interceptor[] interceptors = FindObjectsOfType<Interceptor>();
            if (interceptors != null)
            {
                GameObject interceptorGameObject = interceptors
                    .FirstOrDefault(x => x.Id == interceptorDto.Id)?.gameObject;

                if (interceptorGameObject != null)
                {
                    MessagingCenter.Send(
                        this,
                        GameEvent.InterceptorControllerRequestCameraFocus,
                        interceptorGameObject.transform.position);
                }
            }

            MessagingCenter.Send(
                this,
                GameEvent.InterceptorControllerRequestContextAction,
                interceptorDto);

            sw.Stop();
            Debug.LogWarning($"Time for executing coordinates send message {sw.ElapsedMilliseconds} ms");

            return;
        }
        
        if (interceptorDto.Fuel <= MinFuelToStartInterception)
        {
            return;
        }

        GameObject obj = Instantiate(interceptorCraftPrefab);
        Interceptor interceptor = obj.GetComponent<Interceptor>();

        if (interceptor == null)
            return;

        if (interceptorDto.Status == InterceptorStatus.Ready)
        {
            interceptorDto.Status = InterceptorStatus.Moving;
        }

        SynchronizeInterceptorData(interceptorDto, interceptor, alienSubTarget);
    }

    private void SynchronizeInterceptorData(InterceptorDto dto, Interceptor interceptor, GameObject alienSubTarget)
    {
        interceptor.Name = dto.Name;
        interceptor.Id = dto.Id;
        interceptor.InterceptorStatus = dto.Status;
        interceptor.StartPoint = XComObjectsController.BasesController.GetXComBase().Location;
        interceptor.Health = dto.Health;
        interceptor.InterceptorType = dto.InterceptorType;
        interceptor.InterceptorWeapon = dto.Weapon;
        interceptor.StartSpeed = dto.Speed;
        interceptor.alienTarget = alienSubTarget;
        interceptor.InterceptorStatus = dto.Status;
        interceptor.Fuel = dto.Fuel;
    }

    private void SynchronizeDtoData(Interceptor interceptor)
    {
        InterceptorDto dto = InterceptorsList.FirstOrDefault(x => x.Id == interceptor.Id);
        if (dto != null)
        {
            dto.Health = interceptor.Health;
            dto.Health = 90f;
            dto.Fuel = interceptor.Fuel;

            if (dto.Health < 100f)
            {
                dto.Status = InterceptorStatus.Repairing;
                Debug.Log($"{interceptor.Name} health: {interceptor.Health}");
            }
            else
            {
                dto.Status = InterceptorStatus.Ready;
            }
        }
    }

    private IEnumerator RepairInterceptorsIfNecessary()
    {
        var timeController = FindObjectOfType<TimeController>();
        var timeSpeed = timeController.TimeSpeed;

        foreach (var dto in InterceptorsList.Where(x => x.Status == InterceptorStatus.Repairing))
        {
            while (dto.Health < 100f)
            {
                dto.Health += (float)(int)timeSpeed / 1000000;

                if (dto.Health > 100f)
                {
                    dto.Health = 100f;
                }

                Debug.Log($"{dto.Name} repairing, health {dto.Health}");
                yield return null;
            }
            dto.Status = InterceptorStatus.Ready;
            timeController.SetCurrentTimeSpeedToSeconds();
            Debug.Log($"{dto.Name} repaired");
        }
        yield return null;
    }

    private IEnumerator RefuelInterceptorsIfNecessary()
    {
        var timeController = FindObjectOfType<TimeController>();

        foreach (var dto in InterceptorsList.Where(x => x.Status == InterceptorStatus.Ready))
        {
            if (dto.Health < 100f)
            {
                yield return null;
            }

            while (dto.Fuel < 100f)
            {
                dto.Fuel += Time.deltaTime * (float)(int)timeController.TimeSpeed / 10;

                if (dto.Fuel > 100f)
                {
                    dto.Fuel = 100f;
                }

                Debug.Log($"{dto.Name} refueling, amount left: {dto.Fuel}");
            }
        }
        yield return null;
    }
}
