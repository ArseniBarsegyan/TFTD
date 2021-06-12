using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XComBasesController : MonoBehaviour
{
    [SerializeField] private GameObject xComBasePrefab;
    private static List<XComBaseDto> _xComBases = new List<XComBaseDto>();

    private XComSoldiersController _soldiersController;
    private InterceptorsController _interceptorsController;

    void Start()
    {
        _soldiersController = XComObjectsController.SoldiersController;
        _interceptorsController = XComObjectsController.InterceptorsController;
    }

    public XComBaseDto GetXComBase()
    {
        return _xComBases.FirstOrDefault();
    }

    public void CreateXComBase(GeoPosition position)
    {
        GameObject obj = Instantiate(xComBasePrefab);
        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        obj.transform.position = position.Point;

        var newBaseDto = new XComBaseDto
        {
            Id = Guid.NewGuid(),
            Name = position.Name,
            Location = position.Point
        };

        if (!_xComBases.Any())
        {
            InitializeSoldiersForBase(newBaseDto);
            InitializeInterceptorsForBase(newBaseDto);
        }

        _xComBases.Add(newBaseDto);
    }

    private void InitializeSoldiersForBase(XComBaseDto baseDto)
    {
        baseDto.SoldierDtos = new List<SoldierDto>
            {
                _soldiersController.CreateNewSoldier(
                    SoldierRank.Rookie,
                    SoldierSpecialty.Rookie,
                    baseDto.Id),
                _soldiersController.CreateNewSoldier(
                    SoldierRank.Rookie,
                    SoldierSpecialty.Rookie,
                    baseDto.Id),
                _soldiersController.CreateNewSoldier(
                    SoldierRank.Rookie,
                    SoldierSpecialty.Rookie,
                    baseDto.Id),
                _soldiersController.CreateNewSoldier(
                    SoldierRank.Rookie,
                    SoldierSpecialty.Rookie,
                    baseDto.Id)
            };
    }

    private void InitializeInterceptorsForBase(XComBaseDto baseDto)
    {
        baseDto.InterceptorDtos = new List<InterceptorDto>
        {
            _interceptorsController.CreateNewTriton(baseDto.Id),
            _interceptorsController.CreateNewDefaultInterceptor(baseDto.Id),
            _interceptorsController.CreateNewDefaultInterceptor(baseDto.Id)
        };
    }
}
