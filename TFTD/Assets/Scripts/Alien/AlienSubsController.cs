using System;
using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

public class AlienSubsController : MonoBehaviour
{
    [SerializeField] private GameObject alienSubPrefab;
    public static List<GameObject> AlienSubList = new List<GameObject>();

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.AlienSubSpawn, 
            (controller, dto) => 
            {
                CreateSub(dto);
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this, 
            GameEvent.AlienSubSpawn);
    }

    public static GameObject GetAlienSubById(Guid id)
    {
        foreach(var obj in AlienSubList)
        {
            var alienSub = obj.GetComponent<AlienSub>();

            if (alienSub.Id == id)
                return obj;
        }
        return null;
    }
    
    private void CreateSub(AlienSubDto dto)
    {
        var obj = Instantiate(alienSubPrefab) as GameObject;
        var alienSub = obj.GetComponent<AlienSub>();
        alienSub.Id = dto.Id;
        alienSub.Health = dto.Health;
        alienSub.Race = dto.Race;
        alienSub.SubType = dto.SubType;
        alienSub.Speed = dto.Speed;
        alienSub.Weapon = dto.Weapon;
        alienSub.SubStatus = dto.Status;
        alienSub.StartPoint = dto.StartPoint;
        alienSub.DestinationPoint = dto.DestinationPoint;

        AlienSubList.Add(obj);
    }
}
