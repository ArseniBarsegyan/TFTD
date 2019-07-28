using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private DateTime _currentDate;
    // default time change speed
    private float _currentSpeed = 1.0f;
    private Material _globeMaterial;

    [SerializeField] private Text dateLabel;
    [SerializeField] private Text timeLabel;
    [SerializeField] private GameObject globe;

    void Start()
    {
        _currentDate = DateTime.Parse("01/01/2042 15:00");
        var mater = Resources.Load<Material>("Earth");

        _globeMaterial = globe.GetComponent<Renderer>().material;
        _globeMaterial = globe.GetComponent<Renderer>().material;
        _globeMaterial.SetVector("Vector2_41817D62", new Vector2(0.05f, 0));
    }

    void Update()
    {
        switch (_currentSpeed)
        {
            case 86400f:
                _globeMaterial.SetVector("Vector2_41817D62", new Vector2(0.5f, 0));
                break;
            case 21600f:
                _globeMaterial.SetVector("Vector2_41817D62", new Vector2(0.1f, 0));
                break;
            case 3600f:
                _globeMaterial.SetVector("Vector2_41817D62", new Vector2(0.05f, 0));
                break;
            case 1.0f:
                _globeMaterial.SetVector("Vector2_41817D62", new Vector2(0.001f, 0));
                break;
        }
        //globe.transform.RotateAround(Vector3.zero, globe.transform.up, -rotationAngle * Time.deltaTime);
        //Camera.main.transform.RotateAround(Vector3.zero, globe.transform.up, -rotationAngle * Time.deltaTime);

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
