using System.Collections;
using System.Linq;
using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private const string IsNewGame = "IsNewGame";
    private bool _isNewGame;
    private NewBaseController _newBaseController;

    private float _rotateSpeed = 4.0f;
    [SerializeField] private GameObject globe;
    [SerializeField] private GameObject baseController;

    private Material _baseMaterial;

    private float _cameraDistance = 5.0f;

    void Start()
    {
        _baseMaterial = Resources.Load("UIHologram", typeof(Material)) as Material;
        _isNewGame = PlayerPrefs.GetInt(IsNewGame, 0) == 1;
        _newBaseController = baseController.GetComponent<NewBaseController>();
        if (_isNewGame)
        {
            _newBaseController.ShowNewBasePanel();
        }
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(- Vector3.forward * Time.deltaTime * 5.0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _cameraDistance = Vector3.Distance(transform.position, hit.point);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate( Vector3.forward * Time.deltaTime * 5.0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _cameraDistance = Vector3.Distance(transform.position, hit.point);
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

        if (Input.GetMouseButtonDown(1))
        {
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
            transform.position = Vector3.Lerp(transform.position, observePosition, 
                Mathf.SmoothStep(0f, 1f, t));
            transform.LookAt(globe.transform);
            yield return null;
        }

        yield return null;
    }
    
    // TODO: Later replace with Alien Sub spawn
    private IEnumerator EmptyObject(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(1);
        Destroy(sphere);
    }

    public void SetLocation(string locationName)
    {
#if DEBUG
        Debug.Log(locationName);
#endif
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
