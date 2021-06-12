using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XComSoldiersController : MonoBehaviour
{
    private List<SoldierDto> AllSoldiers = new List<SoldierDto>();

    public List<SoldierDto> AvailableSoldiers = new List<SoldierDto>();
    public List<SoldierDto> MissionSoldiers => AvailableSoldiers;
    public List<SoldierDto> WoundedSoldiers = new List<SoldierDto>();
    public List<SoldierDto> KilledSoldiers = new List<SoldierDto>();

    public SoldierDto CreateNewSoldier(SoldierRank rank, SoldierSpecialty specialty, Guid baseId)
    {
        var rookie = new SoldierDto
        {
            Id = Guid.NewGuid(),
            Health = 100f,
            Name = SoldiersNameGenerator.NewName(),
            Rank = rank,
            SoldierSpecialty = specialty,
            BaseId = baseId
        };
        AllSoldiers.Add(rookie);
        AvailableSoldiers.Add(rookie);
        return rookie;
    }

    public void SynchronizeSoldiersAfterMission(List<SoldierDto> soldiers)
    {
        var killedSoldiers = soldiers.Where(x => x.Health <= 0f).ToList();
        var woundedSoldiers = soldiers.Where(x => x.Health < 100f && x.Health >= 1f).ToList();
        
        KilledSoldiers.AddRange(killedSoldiers);
        WoundedSoldiers.AddRange(woundedSoldiers);

        AvailableSoldiers = AllSoldiers.Where(x => x.Health >= 99f).ToList();
    }
}
