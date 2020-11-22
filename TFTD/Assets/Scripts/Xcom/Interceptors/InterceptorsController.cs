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

    public void StartIntercept()
    {
        GameObject obj = Instantiate(interceptorCraftPrefab);
        var interceptor = obj.GetComponent<Interceptor>();
        if (interceptor != null)
        {

        }
    }
}
