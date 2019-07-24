using System;
using TMPro;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private DateTime _currentDate;
    // default time change speed
    private float _currentSpeed = 1.0f;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;
    [SerializeField] private GameObject globe;

    void Start()
    {
        _currentDate = DateTime.Parse("01/01/2042 12:00");
    }

    void Update()
    {
        float rotationAngle = 0.0f;
        switch (_currentSpeed)
        {
            case 86400f:
                rotationAngle = 360f;
                break;
            case 21600f:
                rotationAngle = 180f;
                break;
            case 3600f:
                rotationAngle = 15f;
                break;
            case 1.0f:
                rotationAngle = 0.00416666666f;
                break;
        }
        globe.transform.RotateAround(Vector3.zero, globe.transform.up, rotationAngle * Time.deltaTime);
        Camera.main.transform.RotateAround(Vector3.zero, globe.transform.up, rotationAngle * Time.deltaTime);

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
