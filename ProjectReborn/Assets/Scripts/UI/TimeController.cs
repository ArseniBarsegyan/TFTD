using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private DateTime _currentDate;
    // default time change speed
    private float _currentSpeed = 1.0f;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;

    void Start()
    {
        _currentDate = DateTime.Parse("01/01/2042 12:00");
    }

    void Update()
    {
        dateLabel.text = _currentDate.ToString("d");
        timeLabel.text = _currentDate.ToString("HH:mm:ss");
        _currentDate = _currentDate.AddSeconds(Time.deltaTime * _currentSpeed);
    }

    public void SetToSeconds()
    {
        _currentSpeed = 1.0f;
    }

    public void SetToHours()
    {
        _currentSpeed = 3600.0f;
    }

    public void SetToHalfDay()
    {
        _currentSpeed = 21600.0f;
    }

    public void SetToDay()
    {
        _currentSpeed = 86400.0f;
    }
}
