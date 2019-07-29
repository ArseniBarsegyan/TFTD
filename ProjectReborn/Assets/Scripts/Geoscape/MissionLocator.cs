using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// This script holds points with possible missions locations
/// and alien subs spawn locations.
/// </summary>
public class MissionLocator : MonoBehaviour
{
    /// <summary>
    /// Alien subs can spawn almost everywhere around the globe, but only in oceans.
    /// </summary>
    private readonly List<Vector3> _alienSubSpawnPossibleLocations = new List<Vector3>
    {
        new Vector3(-3.9f, 3.2f, -0.3f),
        new Vector3(-1.2f, 4.2f, -2.4f),
        new Vector3(0.3f, 4.4f, -2.4f),
        new Vector3(-3.3f, 2.9f, -2.3f),
        new Vector3(-3.2f, 1.8f, -3.4f),
        new Vector3(-4.4f, 0.9f, -2.2f),
        new Vector3(-3.8f, -1.2f, -3.0f),
        new Vector3(-2.2f, -0.9f, -4.4f),
        new Vector3(-3.0f, -1.8f, -3.6f),
        new Vector3(-3.9f, -2.9f, -1.1f),
        new Vector3(-2.4f, -4.3f, -0.9f),
        new Vector3(-1.0f, -4.2f, -2.5f),
        new Vector3(0.9f, -3.3f, -3.7f),
        new Vector3(1.2f, -4.4f, -2.1f),
        new Vector3(1.9f, -4.6f, -0.9f),
        new Vector3(2.1f, -4.0f, -2.1f),
        new Vector3(3.1f, -3.9f, -0.6f),
        new Vector3(2.3f, -4.4f, 0.2f),
        new Vector3(2.4f, -4.0f, 1.7f),
        new Vector3(4.1f, -2.8f, -0.4f),
        new Vector3(3.7f, -2.8f, -2.0f),
        new Vector3(2.8f, -1.9f, -3.7f),
        new Vector3(3.5f, -0.6f, -3.5f),
        new Vector3(4.1f, -1.5f, -2.4f),
        new Vector3(4.6f, -1.3f, -1.4f),
        new Vector3(4.6f, -1.9f, 0.0f),
        new Vector3(4.4f, 0.8f, 2.3f),
        new Vector3(3.7f, 2.3f, 2.5f),
        new Vector3(2.2f, 3.7f, 2.6f),
        new Vector3(1.7f, -3.9f, 2.7f),
        new Vector3(-0.1f, -4.0f, 3.0f),
        new Vector3(-1.8f, -3.6f, 3.0f),
        new Vector3(-2.9f, -3.6f, 1.9f),
        new Vector3(-3.7f, -2.3f, 2.4f),
        new Vector3(-4.5f, -1.2f, 1.8f),
        new Vector3(-4.6f, 0.2f, 1.9f),
        new Vector3(-4.2f, 1.5f, 2.3f),
        new Vector3(-3.0f, 2.4f, 3.2f),
        new Vector3(-1.1f, 3.4f, 3.5f),
        new Vector3(0.7f, 3.8f, 3.2f),
        new Vector3(2.1f, 3.1f, 3.3f),
        new Vector3(3.6f, 1.7f, 3.0f),
        new Vector3(0.4f, -1.6f, 4.7f),
        new Vector3(-1.1f, -2.5f, 4.2f),
        new Vector3(-2.0f, -1.4f, 4.4f),
        new Vector3(-2.6f, 0.5f, 4.3f),
        new Vector3(-1.1f, 1.4f, 4.7f),
        new Vector3(-1.1f, 0.0f, 4.9f),
        new Vector3(0.6f, 0.2f, 5.0f),
        new Vector3(4.6f, 1.1f, 1.7f)
    };
    /// <summary>
    /// Terror missions can be in oceans (on ships) or port-cities
    /// </summary>
    private readonly List<Vector3> _terrorPossibleLocations = new List<Vector3>();
    private readonly List<Vector3> _alienBasesPossibleLocations = new List<Vector3>
    {
        // Near the Japan
        new Vector3(4.4f, 1.6f, 1.8f),
        // To the east of India
        new Vector3(4.8f, -0.2f, -1.5f),
        // To the east of Africa
        new Vector3(2.3f, -1.5f, -4.2f),
        // To the west of India and east of Africa
        new Vector3(3.5f, 0.1f, -3.5f),
        // Between Africa and South America
        new Vector3(-2.8f, -2.5f, -3.3f),
        // To the west of Europe
        new Vector3(-2.4f, 2.7f, -3.4f),
        // To the east of the USA
        new Vector3(-3.8f, 3.2f, -0.8f),
        // To the west of USA and SouthAmerica
        new Vector3(-3.7f, 1.4f, 3.0f),
        // Between USA and Russia, Pacific Ocean
        new Vector3(1.0f, 3.9f, 2.9f),
        // To the north of Europe
        new Vector3(0.2f, 4.5f,-2.2f),
        // Near Antarctic
        new Vector3(2.2f, -4.4f, -1.0f)
    };
    private readonly List<Vector3> _alienActivityPossibleLocations = new List<Vector3>()
    {
        // 
    };

    private Material _alienSubMaterial;

    void Start()
    {
        _alienSubMaterial = Resources.Load("AlienSubMaterial", typeof(Material)) as Material;


        foreach (var location in _alienSubSpawnPossibleLocations)
        {
            var alienSpawn = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            alienSpawn.GetComponent<Renderer>().material = _alienSubMaterial;
            alienSpawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            alienSpawn.transform.position = location;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _alienActivityPossibleLocations.Add(hit.point);
                Debug.Log(hit.point);
            }
        }
    }
}
