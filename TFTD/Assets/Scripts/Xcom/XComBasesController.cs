using System;
using System.Collections.Generic;
using UnityEngine;

public class XComBasesController : MonoBehaviour
{
    [SerializeField] private GameObject xComBasePrefab;
    private static List<XComBaseDto> _xComBases = new List<XComBaseDto>();

    public void CreateXComBase(GeoPosition position)
    {
        GameObject obj = Instantiate(xComBasePrefab);
        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        obj.transform.position = position.Point;
        _xComBases.Add(
            new XComBaseDto
            {
                Id = Guid.NewGuid(),
                Name = position.Name,
                Location = position.Point
            });
    }
}
