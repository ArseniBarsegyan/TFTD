using UnityEngine;

public struct AlienSubDto
{
    public Vector3 StartPoint { get; set; }
    public Vector3 DestinationPoint { get; set; }
    public AlienRace Race { get; set; }
    public AlienSubType SubType { get; set; }
    public float Speed { get; set; }
    public int Health { get; set; }
    public AlienSubWeapon Weapon { get; set; }
    public AlienSubStatus Status { get; set; }
}
