using System;
using System.Linq;
using Assets.Scripts.Messaging;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    // TODO: Calculate alien subs spawn logic
    // It should depend on how much time passed since player
    // started his game. Also it should depend on current research
    // technology level. Calculate chances that alien sub will spawn in current
    // moment of time.

    private DateTime _gameStartTime;
    private DateTime _lastAlienSubSpawnTime;

    [SerializeField] private GameObject timeController;

    void Start()
    {
        _gameStartTime = DateTime.Parse("01/01/2042 15:00");
    }

    void Update()
    {
    }

    public void SpawnAlienSub()
    {
        _lastAlienSubSpawnTime = timeController.GetComponent<TimeController>().CurrentDate;
        var dtoModel = new AlienSubDto
        {
            StartPoint = MissionLocator.AlienBasesPossibleLocations.ElementAt(0).Point,
            DestinationPoint = MissionLocator.AlienBasesPossibleLocations.ElementAt(2).Point,
            Race = AlienRace.Tasoth,
            Speed = 1.0f,
            Weapon = AlienSubWeapon.HeavyPlasma,
            SubType = AlienSubType.Battleship,
            Status = AlienSubStatus.Moving
        };
        MessagingCenter.Instance.Send(this, GameEvent.AlienSubSpawn, dtoModel);
    }
}
