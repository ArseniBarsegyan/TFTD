using System;
using UnityEngine;

public class InterceptorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Vector3 StartPoint { get; set; }
    public InterceptorType InterceptorType { get; set; }
    public float Speed { get; set; }
    public int Health { get; set; }
    public InterceptorWeapon Weapon { get; set; }
    public InterceptorStatus Status { get; set; }
}
