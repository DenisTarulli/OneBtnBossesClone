using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _radius;
    private Vector3 _initialPosition;
    private int _rotationDirection;
    private float _timeCounter;

    private void Start()
    {
        _initialPosition = transform.position;
        _rotationDirection = 1;
    }

    private void Update()
    {
        CircularMovement();

        if (Input.GetKeyDown(KeyCode.Space))
            InvertRotationDirection();
    }

    private void CircularMovement()
    {
        _timeCounter += Time.deltaTime * _speed * _rotationDirection;

        float x = Mathf.Cos(_timeCounter) * _radius;
        float y = Mathf.Sin(_timeCounter) * _radius;

        Vector3 newPosition = new(x, y, 0f);

        transform.position = newPosition + _initialPosition;
    }

    private void InvertRotationDirection()
    {
        _rotationDirection = -_rotationDirection;
    }
}
