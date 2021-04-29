using Assets.Scripts.Messaging;

using System;
using System.Collections;

using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private const string IsNewGame = nameof(IsNewGame);
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
    private float _cameraDistance = 5.0f;

    [SerializeField] private GameObject globe;
    [SerializeField] private GameObject baseController;

    void Awake()
    {
        MessagingCenter.Subscribe<GameEventsController, AlienSubDto>
        (this, GameEvent.GameEventsControllerAlienSubSpawn,
            (controller, dto) =>
            {
                StartCoroutine(MoveCameraOverPoint(dto.StartPoint));
            });
        MessagingCenter.Subscribe<GameEventsController, GeoPosition>
            (this, GameEvent.GameEventsControllerXComBaseCreated,
            (controller, geoPosition) =>
            {
                SetXComBaseLocation(geoPosition);
            });
        MessagingCenter.Subscribe<ClickableAlienTarget, Guid>
            (this, GameEvent.ClickableAlienTargetAlienTargetClicked,
            (target, id) =>
            {
                var alienSub = AlienSubsController.GetAlienSubById(id);
                StartCoroutine(MoveCameraOverPoint(alienSub.transform.position));
            });
        MessagingCenter.Subscribe<InterceptorsController, Vector3>
            (this, GameEvent.InterceptorControllerRequestCameraFocus,
            (target, position) =>
            {
                StartCoroutine(MoveCameraOverPoint(position));
            });
    }

    void Destroy()
    {
        MessagingCenter.Unsubscribe<GameEventsController, AlienSubDto>(this,
            GameEvent.GameEventsControllerAlienSubSpawn);
        MessagingCenter.Unsubscribe<GameEventsController, GeoPosition>(this,
            GameEvent.GameEventsControllerXComBaseCreated);
        MessagingCenter.Unsubscribe<ClickableAlienTarget, AlienSubDto>(this,
            GameEvent.ClickableAlienTargetAlienTargetClicked);
        MessagingCenter.Unsubscribe<InterceptorsController, Vector3>(this,
            GameEvent.InterceptorControllerRequestCameraFocus);
    }

    void Start()
    {
        _isNewGame = PlayerPrefs.GetInt(IsNewGame, 0) == 1;
        _newBaseController = baseController.GetComponent<NewBaseController>();
        if (_isNewGame)
        {
            _newBaseController.ShowNewBasePanel();
        }
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
                StartCoroutine(MoveCameraOverPoint(hit.point));
            }
        }
    }

    // Create camera observe point and move camera to that point smoothly
    private IEnumerator MoveCameraOverPoint(Vector3 targetPoint)
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

    public void SetXComBaseLocation(GeoPosition geoPosition)
    {
        _newBaseController.HideNewBasePanel();
        StartCoroutine(MoveCameraOverPoint(geoPosition.Point));
    }
}
