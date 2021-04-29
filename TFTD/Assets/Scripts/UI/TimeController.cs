using Assets.Scripts.Messaging;

using System;

using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private Material _globeMaterial;
    private bool _timeChanged;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;
    [SerializeField] private GameObject globe;

    public DateTime CurrentDate { get; private set; }
    public TimeSpeed TimeSpeed { get; set; }

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.GameEventsControllerAlienSubSpawn,
            (controller, dto) =>
            {
                SetCurrentTimeSpeedToSeconds();
            });

        MessagingCenter.Subscribe<GameEventsController, GeoPosition>
        (this, GameEvent.GameEventsControllerXComBaseCreated,
            (controller, position) =>
            {
                SetCurrentTimeSpeedToSeconds();
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.GameEventsControllerAlienSubSpawn);
        MessagingCenter.Unsubscribe<GameEventsController>(this,
            GameEvent.GameEventsControllerXComBaseCreated);
    }

    void Start()
    {
        CurrentDate = DateTime.Parse("01/01/2042 15:00");
        _globeMaterial = globe.GetComponent<Renderer>().material;
        StopTime();
    }

    void Update()
    {
        if (_timeChanged)
        {
            float shadowSpeed = 0.001f;

            switch (TimeSpeed)
            {
                case TimeSpeed.Stop:
                    shadowSpeed = 0f;
                    break;
                case TimeSpeed.OneDay:
                    shadowSpeed = 0.5f;
                    break;
                case TimeSpeed.SixHours:
                    shadowSpeed = 0.1f;
                    break;
                case TimeSpeed.OneHour:
                    shadowSpeed = 0.05f;
                    break;
                case TimeSpeed.OneSecond:
                    shadowSpeed = 0.001f;
                    break;
            }
            _globeMaterial.SetFloat("Vector1_654CB0E4", shadowSpeed);
            _timeChanged = false;
        }

        dateLabel.text = CurrentDate.ToString("d");
        timeLabel.text = CurrentDate.ToString("HH:mm:ss");
        
        CurrentDate = CurrentDate.AddSeconds(Time.deltaTime * (int)TimeSpeed);
    }

    public void StopTime()
    {
        TimeSpeed = TimeSpeed.Stop;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToSeconds()
    {
        TimeSpeed = TimeSpeed.OneSecond;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToHours()
    {
        TimeSpeed = TimeSpeed.OneHour;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToSixHours()
    {
        TimeSpeed = TimeSpeed.SixHours;
        _timeChanged = true;
    }

    public void SetCurrentTimeSpeedToDay()
    {
        TimeSpeed = TimeSpeed.OneDay;
        _timeChanged = true;
    }
}
