using System.Collections.Generic;
using Assets.Scripts.Messaging;
using UnityEngine;

public class AlienSubController : MonoBehaviour
{
    [SerializeField] private GameObject alienSubPrefab;
    private List<GameObject> _alienSubList = new List<GameObject>();

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
    
    private void CreateSub(AlienSubDto dto)
    {
        var obj = Instantiate(alienSubPrefab) as GameObject;
        var alienSub = obj.GetComponent<AlienSub>();
        alienSub.Health = dto.Health;
        alienSub.Race = dto.Race;
        alienSub.SubType = dto.SubType;
        alienSub.Speed = dto.Speed;
        alienSub.Weapon = dto.Weapon;
        alienSub.SubStatus = dto.Status;
        alienSub.StartPoint = dto.StartPoint;
        alienSub.DestinationPoint = dto.DestinationPoint;

        _alienSubList.Add(obj);
    }
}
