using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private float _zoomSpeed;
    private float _minDistance = 6.0f;
    private float _maxDistance = 12.0f;
    [SerializeField] private GameObject geoscape;

    void Start()
    {
        _zoomSpeed = 10.0f;
    }

    void Update()
    {
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
}
