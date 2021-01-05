using System;
using UnityEngine;

public class AlienSub : MonoBehaviour
{
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
            transform.position = Vector3.RotateTowards(transform.position,
                DestinationPoint,
                Time.deltaTime * Speed * 0.01f,
                0f);
            transform.LookAt(Vector3.zero);
        }
    }
}
