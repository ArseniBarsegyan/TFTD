using UnityEngine;

public class EarthGlobeBehavior : MonoBehaviour
{
    Vector3 prevPosition = Vector3.zero;
    Vector3 positionDelta = Vector3.zero;

    //void OnMouseDrag()
    //{
    //    float rotationX = Input.GetAxis("Mouse X");
    //    float rotationY = Input.GetAxis("Mouse Y");

    //    transform.Rotate(new Vector3(-rotationY, -rotationX, 0) * rotationSpeed);
    //}

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            positionDelta = Input.mousePosition - prevPosition;
            if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                transform.Rotate(transform.up, -Vector3.Dot(positionDelta, Camera.main.transform.right), Space.World);
            }
            else
            {
                transform.Rotate(transform.up, Vector3.Dot(positionDelta, Camera.main.transform.right), Space.World);
            }
            transform.Rotate(Camera.main.transform.right, Vector3.Dot(positionDelta, Camera.main.transform.up), Space.World);
        }

        prevPosition = Input.mousePosition;
    }
}
