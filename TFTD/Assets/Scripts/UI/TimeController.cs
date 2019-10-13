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
                SetToSeconds();
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.AlienSubSpawn);
    }

    void Start()
    {
        CurrentDate = DateTime.Parse("01/01/2042 15:00");
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

        dateLabel.text = CurrentDate.ToString("d");
        timeLabel.text = CurrentDate.ToString("HH:mm:ss");
        
        CurrentDate = CurrentDate.AddSeconds(Time.deltaTime * _currentSpeed);
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
