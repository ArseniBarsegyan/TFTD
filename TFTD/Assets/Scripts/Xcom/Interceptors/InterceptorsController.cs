using Assets.Scripts.Messaging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class InterceptorsController : MonoBehaviour
{
    private const float DefaultInterceptorHealth = 100;
    private const float DefaultInteceptorSpeed = 1.0f;
    private const float MinFuelToStartInterception = 10f;

    private TimeController _timeController;
    private int _interceptorsCount;

    [SerializeField] private GameObject interceptorCraftPrefab;

    public List<InterceptorDto> InterceptorsList { get; } = new List<InterceptorDto>();

    void Start()
    {
        _timeController = FindObjectOfType<TimeController>();
    }

    void Update()
    {
        StartCoroutine(RefuelInterceptorsIfNecessary());
        StartCoroutine(RepairInterceptorsIfNecessary());
    }

    public void ReturnInterceptorToBase(Interceptor interceptor)
    {
        SynchronizeDtoData(interceptor);
        Destroy(interceptor.gameObject);
    }

    public InterceptorDto CreateNewDefaultInterceptor(Guid baseId)
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
            Fuel = 100,
            BaseId = baseId
        };
        InterceptorsList.Add(dto);
        return dto;
    }

    public InterceptorDto CreateNewTriton(Guid baseId)
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
            Fuel = 100,
            BaseId = baseId
        };
        InterceptorsList.Add(dto);
        return dto;
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
        foreach (var dto in InterceptorsList.Where(x => x.Status == InterceptorStatus.Repairing))
        {
            while (dto.Health < 100f)
            {
                dto.Health += (float)(int)_timeController.TimeSpeed / 1000000;

                if (dto.Health > 100f)
                {
                    dto.Health = 100f;
                }

                Debug.Log($"{dto.Name} repairing, health {dto.Health}");
                yield return null;
            }
            dto.Status = InterceptorStatus.Ready;
            _timeController.SetCurrentTimeSpeedToSeconds();
            Debug.Log($"{dto.Name} repaired");
        }
        yield return null;
    }

    private IEnumerator RefuelInterceptorsIfNecessary()
    {
        foreach (var dto in InterceptorsList.Where(x => x.Status == InterceptorStatus.Ready))
        {
            if (dto.Health < 100f)
            {
                yield return null;
            }

            while (dto.Fuel < 100f)
            {
                dto.Fuel += Time.deltaTime * (float)(int)_timeController.TimeSpeed / 10;

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
