using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GeoscapeCamera : MonoBehaviour
{
    private float _zoomSpeed = 10.0f;
    private float _rotateSpeed = 4.0f;
    private float _minDistance = 6.0f;
    private float _maxDistance = 12.0f;
    [SerializeField] private GameObject geoscape;

    private float _rotX;
    private float _rotY;
    private Vector3 _offset;

    void Start()
    {
        _offset = geoscape.gameObject.transform.position - transform.position;
    }

    void Update()
    {
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 geoscapePosition = geoscape.gameObject.transform.position;
            float distance = Vector3.Distance(cameraPosition, geoscapePosition);
            if (distance > _minDistance)
            {
                Camera.main.transform.position += Camera.main.transform.forward
                                                  * Time.deltaTime
                                                  * _zoomSpeed;
            }
        }

        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 geoscapePosition = geoscape.gameObject.transform.position;
            float distance = Vector3.Distance(cameraPosition, geoscapePosition);

            if (distance < _maxDistance)
            {
                Camera.main.transform.position -= Camera.main.transform.forward
                                                  * Time.deltaTime
                                                  * _zoomSpeed;
            }
        }
    }

    // TODO: zoom changes _offset
    void LateUpdate()
    {
        // Rotation and follow for the globe
        if (Input.GetMouseButton(0))
        {
            float horInput = Input.GetAxis("Horizontal");
            if (horInput != 0)
            {
                _rotY += horInput * _rotateSpeed;
            }
            else
            {
                _rotY += Input.GetAxis("Mouse X") * _rotateSpeed;
            }

            var verticalInput = Input.GetAxis("Mouse Y");
            if (verticalInput != 0)
            {
                _rotX -= verticalInput * _rotateSpeed;
            }

            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
            transform.position = geoscape.transform.position - (rotation * _offset); // const delta that change its position with camera rotation
            transform.LookAt(geoscape.transform); // camera always look at it's target
        }

        // TODO: Fix rotation.
        if (Input.GetMouseButtonDown(1))
        {
            Ray initialRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
            RaycastHit initialHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Physics.Raycast(initialRay, out initialHit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (initialHit.transform.gameObject == hitObject)
                    {
                        if (hitObject == geoscape)
                        {
                            Vector3 initialPoint = initialHit.point;
                            Vector3 targetPoint = hit.normal;
                            _rotX -= (targetPoint.x - initialPoint.x);
                            _rotY -= (targetPoint.y - initialPoint.y);
                            float rotZ = targetPoint.z - initialPoint.z;

                            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, rotZ);
                            transform.position = geoscape.transform.position - (rotation * _offset);
                            transform.LookAt(geoscape.transform);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator RotateCameraToGlobePoint()
    {
        yield return null;
    }
}
