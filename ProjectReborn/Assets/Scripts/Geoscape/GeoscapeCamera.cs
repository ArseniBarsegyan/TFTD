using System.Collections;
using UnityEngine;

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


    Vector3 prevPosition = Vector3.zero;
    Vector3 positionDelta = Vector3.zero;

    void Start()
    {
        _offset = geoscape.transform.position - transform.position;
    }

    void Update()
    {
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Vector3 cameraPosition = transform.position;
            Vector3 geoscapePosition = geoscape.transform.position;
            float distance = Vector3.Distance(cameraPosition, geoscapePosition);
            if (distance > _minDistance)
            {
                transform.position += transform.forward * Time.deltaTime * _zoomSpeed;
            }
        }

        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Vector3 cameraPosition = transform.position;
            Vector3 geoscapePosition = geoscape.transform.position;
            float distance = Vector3.Distance(cameraPosition, geoscapePosition);

            if (distance < _maxDistance)
            {
                transform.position -= transform.forward * Time.deltaTime * _zoomSpeed;
            }
        }
    }

    void LateUpdate()
    {
        // Rotation and follow the globe
        if (Input.GetMouseButton(0))
        {
            positionDelta = Input.mousePosition - prevPosition;

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
        prevPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(1))
        {
            // TODO: it works properly but need to rotate and center camera on specific point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                StartCoroutine(EmptyObject(hit.point));

                Quaternion targetRotation = Quaternion.LookRotation(transform.position, hit.transform.forward);
                //Quaternion rotation = Quaternion.RotateTowards(transform.rotation, hit.transform.rotation, Time.deltaTime * 10);

                Vector3 newCameraPosition = hit.point + (targetRotation * _offset * 0.5f);
                //transform.position = hit.point + (rotation * _offset * 0.5f);
                //transform.LookAt(geoscape.transform);

                transform.position = newCameraPosition;
                transform.LookAt(geoscape.transform);
            }
        }
    }

    private IEnumerator EmptyObject(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
