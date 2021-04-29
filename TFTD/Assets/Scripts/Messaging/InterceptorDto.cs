using System;
using System.Collections.Generic;

using UnityEngine;

public class InterceptorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Vector3 StartPoint { get; set; }
    public Vector3 CurrentPosition { get; set; }
    public InterceptorType InterceptorType { get; set; }
    public float Speed { get; set; }
    public float Health { get; set; }
    public InterceptorWeapon Weapon { get; set; }
    public InterceptorStatus Status { get; set; }
    public float Fuel { get; set; }
    public List<SoldierDto> Soldiers { get; set; }
}
