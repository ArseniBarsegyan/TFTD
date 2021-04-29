using System;
using System.Collections.Generic;
using UnityEngine;

public class XComBaseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Vector3 Location { get; set; }
    public List<InterceptorDto> InterceptorDtos { get; set; }
    public List<SoldierDto> SoldierDtos { get; set; }
}
