using System.Collections;
using UnityEngine;

public class EarthGlobeBehavior : MonoBehaviour
{
    Vector3 prevPosition = Vector3.zero;
    Vector3 positionDelta = Vector3.zero;
    private float _rotationSpeed = 0.2f;

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    positionDelta = Input.mousePosition - prevPosition;
        //    if (Vector3.Dot(transform.up, Vector3.up) >= 0)
        //    {
        //        transform.Rotate(transform.up, -Vector3.Dot(positionDelta, Camera.main.transform.right) * _rotationSpeed, Space.World);
        //    }
        //    else
        //    {
        //        transform.Rotate(transform.up, Vector3.Dot(positionDelta, Camera.main.transform.right) * _rotationSpeed, Space.World);
        //    }
        //    transform.Rotate(Camera.main.transform.right, Vector3.Dot(positionDelta, Camera.main.transform.up) * _rotationSpeed, Space.World);
        //}
        // prevPosition = Input.mousePosition;
    }
}
