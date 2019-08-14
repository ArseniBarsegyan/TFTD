using System.Collections;
using System.Linq;
using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private const string IsNewGame = "IsNewGame";
    private const float InitialCameraDistance = 5.0f;
    private const float ZoomSpeed = 2.0f;
    private const float MinCameraDistance = 3.0f;
    private const float MaxCameraDistance = 8.0f;

    private bool _isMoving;
    private bool _isNewGame;
    private NewBaseController _newBaseController;
    private float _initialRotateSpeed = 4.0f;
    // Rotate speed change with zoom.
    private float _rotateSpeed = 4.0f;
    private Material _baseMaterial;
    private float _cameraDistance = 5.0f;

    [SerializeField] private GameObject globe;
    [SerializeField] private GameObject baseController;

    void Start()
    {
        _baseMaterial = Resources.Load("UIHologram", typeof(Material)) as Material;
        _isNewGame = PlayerPrefs.GetInt(IsNewGame, 0) == 1;
        _newBaseController = baseController.GetComponent<NewBaseController>();
        if (_isNewGame)
        {
            _newBaseController.ShowNewBasePanel();
        }

        //GameObject alienSub = CreateAlienSub(MissionLocator.AlienBasesPossibleLocations.ElementAt(0).Point);
        //StartCoroutine(MoveAlienSub(alienSub, MissionLocator.AlienBasesPossibleLocations.ElementAt(0).Point,
        //    MissionLocator.AlienBasesPossibleLocations.ElementAt(3).Point));
    }

    private IEnumerator ZoomSmoothly(Vector3 globePoint, bool zoomIn)
    {
        var distance = Vector3.Distance(transform.position, globePoint);
        if (zoomIn)
        {
            if (distance > MinCameraDistance)
            {
                Vector3 targetDestination = transform.position + transform.forward * ZoomSpeed;

                float t = 0f;
                while (t < 0.3f)
                {
                    t += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, targetDestination, Mathf.SmoothStep(0f, 0.3f, t));
                    yield return null;
                }
            }
            _cameraDistance = distance;
        }
        else
        {
            if (distance < MaxCameraDistance)
            {
                Vector3 targetDestination = transform.position - transform.forward * ZoomSpeed;

                float t = 0f;
                while (t < 0.3f)
                {
                    t += Time.deltaTime;
                    transform.position = Vector3.Lerp(transform.position, targetDestination, Mathf.SmoothStep(0f, 0.3f, t));
                    yield return null;
                }
            }
            _cameraDistance = distance;
        }
        yield return null;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartCoroutine(ZoomSmoothly(hit.point, false));
                _rotateSpeed = _initialRotateSpeed * (_cameraDistance / InitialCameraDistance);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartCoroutine(ZoomSmoothly(hit.point, true));
                _rotateSpeed = _initialRotateSpeed * (_cameraDistance / InitialCameraDistance);
            }
        }

        // Rotation and follow the globe
        if (Input.GetMouseButton(0))
        {
            float horInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            if (verticalInput == 0 && horInput == 0)
            {
                return;
            }

            float verticalAngle = - verticalInput * _rotateSpeed;

            transform.RotateAround(Vector3.zero, transform.up, horInput * _rotateSpeed);
            transform.RotateAround(Vector3.zero, transform.right, verticalAngle);

            transform.LookAt(globe.transform);
        }

        // Rotate to camera to selected point
        if (Input.GetMouseButtonUp(1) && !_isMoving)
        {
            _isMoving = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                StartCoroutine(MoveCamera(hit.point));
            }
        }
    }   

    void LateUpdate()
    {
    }

    // Create camera observe point and move move camera to that point smoothly
    private IEnumerator MoveCamera(Vector3 targetPoint)
    {
        GameObject observePoint = new GameObject();
        observePoint.transform.position = targetPoint;
        observePoint.transform.LookAt(globe.transform);
        observePoint.transform.position -= observePoint.transform.forward * _cameraDistance;
        Destroy(observePoint);

        Vector3 observePosition = observePoint.transform.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.RotateTowards(transform.position,
                observePosition,
                Mathf.SmoothStep(0f, 1f, t),
                0f);
            transform.LookAt(globe.transform);
            yield return null;
        }

        _isMoving = false;
        yield return null;
    }
    
    // TODO: Later replace with Alien Sub spawn
    private GameObject CreateAlienSub(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.position = pos;
        return sphere;
    }

    private IEnumerator MoveAlienSub(GameObject alienSub, Vector3 startPoint, Vector3 endPoint)
    {
        //float t = 0;

        //while (t < 1f)
        //{
        //    t += Time.deltaTime * 0.5f;
        //    alienSub.transform.position = Vector3.RotateTowards(alienSub.transform.position,
        //        DestinationPoint,
        //        Mathf.SmoothStep(0f, 1f, t),
        //        0f);
        //    yield return new WaitForSeconds(0.5f);
        //}
        alienSub.transform.position = Vector3.RotateTowards(alienSub.transform.position,
            endPoint,
            0f,
            0f);
        yield return null;
    }

    public void SetLocation(string locationName)
    {
        var arcticBaseLocation = MissionLocator.XComBasePossibleLocations
            .FirstOrDefault(x => x.Name == locationName);
        if (arcticBaseLocation != null)
        {
            _newBaseController.HideNewBasePanel();
            StartCoroutine(CreateXComBase(arcticBaseLocation.Point));
        }
    }

    private IEnumerator CreateXComBase(Vector3 baseLocation)
    {
        var newBase = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newBase.GetComponent<Renderer>().material = _baseMaterial;
        newBase.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        newBase.transform.position = baseLocation;
        StartCoroutine(MoveCamera(baseLocation));
        yield return null;
    }
}
