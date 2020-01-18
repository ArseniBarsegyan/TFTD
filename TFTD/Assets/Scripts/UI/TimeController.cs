using System;
using Assets.Scripts.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    // default time change speed
    private float _currentSpeed = 1.0f;

    private Material _globeMaterial;
    private bool _timeChanged;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;
    [SerializeField] private GameObject globe;

    public DateTime CurrentDate { get; private set; }

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.AlienSubSpawn,
            (controller, dto) =>
            {
                SetCurrentTimeSpeedToSeconds();
            });
        MessagingCenter.Subscribe<GameEventsController, GeoPosition>
        (this, GameEvent.XComBaseCreated,
            (controller, position) =>
            {
                SetCurrentTimeSpeedToSeconds();
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.AlienSubSpawn);
        MessagingCenter.Unsubscribe<GameEventsController>(this,
            GameEvent.XComBaseCreated);
    }

    void Start()
    {
        CurrentDate = DateTime.Parse("01/01/2042 15:00");
        _globeMaterial = globe.GetComponent<Renderer>().material;
        StopTime();
    }

    private const float OneHourSpeed = 3600f;
    private const float SixHoursSpeed = 21600f;
    private const float OneDaySpeed = 86400f;
    private const float OneSecondSpeed = 1f;

    void Update()
    {
        if (_timeChanged)
        {
            float shadowSpeed = 0.001f;

            switch (_currentSpeed)
            {
                case 0:
                    shadowSpeed = 0f;
                    break;
                case OneDaySpeed:
                    shadowSpeed = 0.5f;
                    break;
                case SixHoursSpeed:
                    shadowSpeed = 0.1f;
                    break;
                case OneHourSpeed:
                    shadowSpeed = 0.05f;
                    break;
                case OneSecondSpeed:
                    shadowSpeed = 0.001f;
                    break;
            }
            _globeMaterial.SetFloat("Vector1_654CB0E4", shadowSpeed);
            _timeChanged = false;
        }

        dateLabel.text = CurrentDate.ToString("d");
        timeLabel.text = CurrentDate.ToString("HH:mm:ss");
        
        CurrentDate = CurrentDate.AddSeconds(Time.deltaTime * _currentSpeed);
    }

    public void StopTime()
    {
        _currentSpeed = 0f;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToSeconds()
    {
        _currentSpeed = 1.0f;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToHours()
    {
        _currentSpeed = 3600.0f;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToHalfDay()
    {
        _currentSpeed = 21600.0f;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToDay()
    {
        _currentSpeed = 86400.0f;
        _timeChanged = true;
    }
}
