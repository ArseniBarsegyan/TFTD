using System;

using UnityEngine;

public class AlienSub : MonoBehaviour
{
    private const float InteractionEnterDistance = 0.05f;
    private TimeController _timeController;

    public Guid Id;
    public AlienSubType SubType;
    public AlienSubStatus SubStatus;
    public AlienRace Race;
    public AlienSubWeapon Weapon;
    public float StartSpeed = 2.0f;
    public float Speed;
    public int Health = 100;
    public Vector3 StartPoint;
    public Vector3 DestinationPoint;

    void Start()
    {
        transform.position = StartPoint;
        transform.LookAt(Vector3.zero);
        Speed = StartSpeed;
        _timeController = FindObjectOfType<TimeController>();
    }

    void Update()
    {
        if (SubStatus == AlienSubStatus.Destroyed)
        {
            Destroy(gameObject);
            return;
        }

        if (SubStatus == AlienSubStatus.Crashed)
        {
            return;
        }

        if (SubStatus == AlienSubStatus.Landed)
        {
            return;
        }

        if (SubStatus == AlienSubStatus.Moving)
        {
            MoveToPosition(DestinationPoint);
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        const float speedMultiplier = 0.01f;
        const float timeSpeedMultiplier = 0.05f;

        transform.position = Vector3.RotateTowards(transform.position,
                position,
                Time.deltaTime * Speed * speedMultiplier * (int)_timeController.TimeSpeed * timeSpeedMultiplier,
                0f);

        transform.LookAt(Vector3.zero);
        
        float distance = Vector3.Distance(transform.position, position);
        
        if (distance > InteractionEnterDistance)
            return;

        SubStatus = AlienSubStatus.Landed;
    }
}
