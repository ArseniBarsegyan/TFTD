using Assets.Scripts.Messaging;

using System;
using System.Globalization;
using System.Linq;

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
    private TimeController _timeController;

    private bool _subSpawned;

    void Start()
    {
        _gameStartTime = DateTime.Parse("01/01/2042 15:00");
    }

    void Update()
    {
        _timeController = FindObjectOfType<TimeController>();
        DateTime currentDate = _timeController.CurrentDate;

        var stringDate = currentDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

        if (stringDate == "01/01/2042 15:45" && !_subSpawned)
        {
            SpawnAlienSub();
            _subSpawned = true;
        }
    }

    // TODO: spawned sub characteristics should depend on current time and tech level.
    public void SpawnAlienSub()
    {
        _lastAlienSubSpawnTime = _timeController.CurrentDate;

        var dtoModel = new AlienSubDto
        {
            Id = Guid.NewGuid(),
            StartPoint = MissionLocator.AlienSubSpawnPossibleLocations.ElementAt(0).Point,
            DestinationPoint = MissionLocator.AlienSubSpawnPossibleLocations.ElementAt(1).Point,
            Race = AlienRace.Tasoth,
            Speed = 1.0f,
            Weapon = AlienSubWeapon.HeavyPlasma,
            SubType = AlienSubType.Battleship,
            Status = AlienSubStatus.Moving
        };

        MessagingCenter.Instance.Send(this, GameEvent.GameEventsControllerAlienSubSpawn, dtoModel);
    }

    public void XComBaseCreated(string locationName)
    {
        var baseLocation = MissionLocator.XComBasePossibleLocations
           .FirstOrDefault(x => x.Name == locationName);

        if (baseLocation != null)
        {
            MessagingCenter.Send(this, GameEvent.GameEventsControllerXComBaseCreated, baseLocation);
        }
    }
}
