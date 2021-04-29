using System.Collections.Generic;

public class XComBase
{
    public List<InterceptorDto> Interceptors { get; } = new List<InterceptorDto>();
    public List<SoldierDto> Soldiers { get; } = new List<SoldierDto>();
}
