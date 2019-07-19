using System.Collections;
using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private float _zoomSpeed = 2.0f;
    private float _rotateSpeed = 4.0f;
    private float _minDistance = 6.0f;
    private float _maxDistance = 12.0f;
    [SerializeField] private GameObject geoscape;

    private float _rotX;
    private float _rotY;
    private Vector3 _offset;

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
                Vector3 targetDestination = transform.position + transform.forward * _zoomSpeed;
                StartCoroutine(ZoomSmoothly(targetDestination));
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
                Vector3 targetDestination = transform.position - transform.forward * _zoomSpeed;

                StartCoroutine(ZoomSmoothly(targetDestination));
            }
        }
    }

    private IEnumerator ZoomSmoothly(Vector3 destination)
    {
        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, Mathf.SmoothStep(0f, 0.5f, t));
            transform.LookAt(geoscape.transform);
            yield return null;
        }
    }

    void LateUpdate()
    {
        // Rotation and follow the globe
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

            if (verticalInput == 0 && horInput == 0)
            {
                return;
            }

            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
            Vector3 targetPosition = geoscape.transform.position - (rotation * _offset);
            StartCoroutine(Rotate_Routine(targetPosition));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Quaternion targetRotation = Quaternion.LookRotation(transform.position, hit.transform.forward);

                Vector3 newCameraPosition = hit.point + (targetRotation * _offset * 0.4f);
                StartCoroutine(Move_Routine(hit.transform.right, newCameraPosition));
            }
        }
    }

    private IEnumerator Move_Routine(Vector3 observableTarget, Vector3 targetPosition)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.SmoothStep(0f, 1f, t));
            transform.LookAt(observableTarget);
            yield return null;
        }
    }

    private IEnumerator Rotate_Routine(Vector3 targetPosition)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.SmoothStep(0f, 1f, t));
            transform.LookAt(geoscape.transform);
            yield return null;
        }
    }

    // TODO: Later replace with Alien Sub spawn
    private IEnumerator EmptyObject(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }
}
