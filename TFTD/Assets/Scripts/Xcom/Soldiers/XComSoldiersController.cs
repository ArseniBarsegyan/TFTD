using System;
using System.Collections.Generic;
using UnityEngine;

public class XComSoldiersController : MonoBehaviour
{
    public static List<SoldierDto> _soldierDtos = new List<SoldierDto>();

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
        _soldierDtos.Add(rookie);
        return rookie;
    }
}
