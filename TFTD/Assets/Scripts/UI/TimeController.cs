using System;
using System.Linq;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private DateTime _gameStartTime;
    private DateTime _currentDate;

    private bool _isCodeExecuted;
    // default time change speed
    private float _currentSpeed = 1.0f;

    private DateTime _lastAlienSubSpawnTime;

    private Material _globeMaterial;
    private bool _timeChanged;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;
    [SerializeField] private GameObject globe;
    [SerializeField] private GameObject alienSubController;
    
    void Start()
    {
        _gameStartTime = DateTime.Parse("01/01/2042 15:00");
        _currentDate = DateTime.Parse("01/01/2042 15:00");
        _globeMaterial = globe.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (_timeChanged)
        {
            float shadowSpeed = 0.001f;

            switch (_currentSpeed)
            {
                case 86400f:
                    shadowSpeed = 0.5f;
                    break;
                case 21600f:
                    shadowSpeed = 0.1f;
                    break;
                case 3600f:
                    shadowSpeed = 0.05f;
                    break;
                case 1.0f:
                    shadowSpeed = 0.001f;
                    break;
            }
            _globeMaterial.SetFloat("Vector1_654CB0E4", shadowSpeed);
            _timeChanged = false;
        }

        dateLabel.text = _currentDate.ToString("d");
        timeLabel.text = _currentDate.ToString("HH:mm:ss");

        if (_currentDate.Hour == 19 && !_isCodeExecuted)
        {
            _isCodeExecuted = true;
            _lastAlienSubSpawnTime = _currentDate;
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

        _currentDate = _currentDate.AddSeconds(Time.deltaTime * _currentSpeed);
    }

    public void SetToSeconds()
    {
        _currentSpeed = 1.0f;
        _timeChanged = true;
    }

    public void SetToHours()
    {
        _currentSpeed = 3600.0f;
        _timeChanged = true;
    }

    public void SetToHalfDay()
    {
        _currentSpeed = 21600.0f;
        _timeChanged = true;
    }

    public void SetToDay()
    {
        _currentSpeed = 86400.0f;
        _timeChanged = true;
    }
}
