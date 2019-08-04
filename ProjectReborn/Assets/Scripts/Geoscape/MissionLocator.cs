using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// This script holds points with all game locations.
/// </summary>
public class MissionLocator : MonoBehaviour
{
    private Material _alienUnitsMaterial;
    private Material _xComUnitsMaterial;

    /// <summary>
    /// Alien subs can spawn almost everywhere around the globe, but only in oceans.
    /// </summary>
    public static List<GeoPosition> AlienSubSpawnPossibleLocations = new List<GeoPosition>
    {
    };
    /// <summary>
    /// Terror missions can be in oceans (on ships) or port-cities
    /// </summary>
    public static List<GeoPosition> TerrorPossibleLocations = new List<GeoPosition>();
    public static List<GeoPosition> AlienBasesPossibleLocations = new List<GeoPosition>
    {
        // Near the Japan
        new GeoPosition { Name = "AlienBase_Japan", Point = new Vector3(4.2f, 1.6f, 2.3f)},
        // To the east of India
        new GeoPosition { Name = "AlienBase_East_Indian", Point = new Vector3(4.8f, -0.2f, -1.5f)},
        // To the east of Africa
        new GeoPosition { Name = "AlienBase_East_Africa", Point = new Vector3(3.6f, 0, -3.4f)},
        // To the west of India and east of Africa
        new GeoPosition { Name = "AlienBase_West_India", Point = new Vector3(3.4f, -2.1f, -3.1f)},
        // Between Africa and South America
        new GeoPosition { Name = "AlienBase_East_SouthAmerica", Point = new Vector3(-2.8f, -2.5f, -3.3f)},
        // To the west of Europe
        new GeoPosition { Name = "AlienBase_West_Europe", Point = new Vector3(-2.4f, 2.7f, -3.4f)},
        // To the east of the USA
        new GeoPosition { Name = "AlienBase_East_USA", Point = new Vector3(-4.1f, 2.5f, -1.2f)},
        // To the west of USA and SouthAmerica
        new GeoPosition { Name = "AlienBase_West_USA", Point = new Vector3(-3.7f, 1.4f, 3.0f)},
        // Between USA and Russia, Pacific Ocean
        new GeoPosition { Name = "AlienBase_USA_Russia_Pacific", Point = new Vector3(1.0f, 3.9f, 2.9f)},
        // To the north of Europe
        new GeoPosition { Name = "AlienBase_NorthEurope", Point = new Vector3(-0.5f, 4.4f,-2.3f)},
        // Near Antarctic
        new GeoPosition { Name = "AlienBase_Antarctic", Point = new Vector3(3.1f, -3.7f, -1.3f)},
        // To the east of Australia
        new GeoPosition { Name = "AlienBase_East_Australia", Point = new Vector3(2.5f, -0.8f, 4.2f)},
        // In the middle of the pacific ocean
        new GeoPosition { Name = "AlienBase_Middle_Pacific", Point = new Vector3(-1.4f, 0.2f, 4.8f)},
        // To the west of South America
        new GeoPosition { Name = "AlienBase_West_SouthAmerica", Point = new Vector3(-4.2f, -2.2f, 1.7f)},
        // Between Australia and Antarctic
        new GeoPosition { Name = "AlienBase_South_Australia", Point = new Vector3(2.1f, -3.7f, 2.6f)}
    };

    public static List<GeoPosition> AlienActivityPossibleLocations = new List<GeoPosition>()
    {
    };

    public static List<GeoPosition> XComBasePossibleLocations = new List<GeoPosition>
    {
        // Arctic ocean
        new GeoPosition {Name = "XComBase_Arctic", Point = new Vector3(0.6f, 4.6f, -1.8f)},
        // North Atlantic Ocean
        new GeoPosition {Name = "XComBase_North_Atlantic", Point = new Vector3(-3.5f, 2.3f, -2.7f)},
        // South Atlantic Ocean
        new GeoPosition {Name = "XComBase_South_Atlantic", Point = new Vector3(-2.5f, -1.4f, -4.1f)},
        // Indian Ocean
        new GeoPosition {Name = "XComBase_Indian", Point = new Vector3(4.2f, -0.4f, -2.7f)},
        // North Pacific Ocean
        new GeoPosition {Name = "XComBase_North_Pacific", Point = new Vector3(2.4f, 3.2f, 3.0f)},
        // South Pacific Ocean
        new GeoPosition {Name = "XComBase_South_Pacific", Point = new Vector3(0.1f, -2.8f, 4.1f)}
    };

    void Start()
    {
        _alienUnitsMaterial = Resources.Load("AlienSubMaterial", typeof(Material)) as Material;
        
        foreach (var position in XComBasePossibleLocations)
        {
            var alienSpawn = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            alienSpawn.GetComponent<Renderer>().material = _alienUnitsMaterial;
            alienSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            alienSpawn.transform.position = position.Point;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.point);
            }
        }
    }
}
