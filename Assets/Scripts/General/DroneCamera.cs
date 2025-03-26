using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    [Header("Atributes")]
    [SerializeField] float _initSpeed;
    [SerializeField] float _rotationSpeed = 50f;
    [SerializeField] Camera _cam;
    float _runSpeed, _speed;

    public bool _canRotate, _isActive;

    private void Start()
    {
        if (_isActive) 
            _cam.orthographic = false;

        _speed = _initSpeed;
        _runSpeed = _initSpeed * 2;
    }

    private void Update()
    {
        if (!_isActive) return;

        if (Input.GetKeyDown(KeyCode.G)) 
            _canRotate = !_canRotate;

        HandleRotation();
        HandlePan();
        HandleYPos();

        _speed = (Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _initSpeed);
    }
    private void HandleRotation()
    {
        if (_canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.Self);
        }
    }

    private void HandlePan()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
        if (Input.GetKey(KeyCode.A)) direction -= transform.right;
        if (Input.GetKey(KeyCode.D)) direction += transform.right;

        transform.position += direction * _speed * Time.deltaTime;
    }

    private void HandleYPos()
    {
        if (Input.GetKey(KeyCode.E))
            transform.position += Vector3.up * _speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            transform.position -= Vector3.up * _speed * Time.deltaTime;
    }
}
