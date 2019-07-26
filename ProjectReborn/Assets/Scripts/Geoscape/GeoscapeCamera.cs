﻿using System.Collections;
using UnityEngine;

public class GeoscapeCamera : MonoBehaviour
{
    private float _rotateSpeed = 4.0f;
    [SerializeField] private GameObject geoscape;

    private float _rotX;
    private float _rotY;
    private Vector3 _offset;
    private float _cameraDistance = 5.0f;

    void Start()
    {
        _offset = geoscape.transform.position - transform.position;
    }

    void Update()
    {
        //transform.RotateAround(geoscape.transform.position, Vector3.up, 45 * Time.deltaTime);
        //transform.RotateAround(geoscape.transform.position, Vector3.right, 45 * Time.deltaTime);

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
        // Rotation and follow the globe
        if (Input.GetMouseButton(0))
        {
            float horInput = Input.GetAxis("Mouse X");
            if (horInput != 0)
            {
                _rotY += Input.GetAxis("Mouse X") * _rotateSpeed;
            }

            float verticalInput = Input.GetAxis("Mouse Y");
            if (verticalInput != 0)
            {
                _rotX -= verticalInput * _rotateSpeed;
            }

            if (verticalInput == 0 && horInput == 0)
            {
                return;
            }

            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, _rotX);
            Vector3 targetPosition = geoscape.transform.position - rotation * _offset;

            transform.position = targetPosition;
            transform.LookAt(geoscape.transform);
        }
    }

    // Create camera observe point and move move camera to that point smoothly
    private IEnumerator MoveCamera(Vector3 targetPoint)
    {
        GameObject observePoint = new GameObject();
        observePoint.transform.position = targetPoint;
        observePoint.transform.LookAt(geoscape.transform);
        observePoint.transform.position -= observePoint.transform.forward * _cameraDistance;
        Destroy(observePoint);

        Vector3 observePosition = observePoint.transform.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, observePosition, 
                Mathf.SmoothStep(0f, 1f, t));
            transform.LookAt(geoscape.transform);
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
}
