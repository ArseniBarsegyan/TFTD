using System;

public class SoldierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Health { get; set; }
    public SoldierRank Rank { get; set; }
    public SoldierSpecialty SoldierSpecialty { get; set; }
    public Guid BaseId { get; set; }
}
