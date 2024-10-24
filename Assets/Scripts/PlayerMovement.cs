using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float radius;
    private Vector3 initialPosition;
    private int rotationDirection;
    private float timeCounter;

    private void Start()
    {
        initialPosition = transform.position;
        rotationDirection = 1;
    }

    private void Update()
    {
        CircularMovement();

        if (Input.GetKeyDown(KeyCode.Space))
            InvertRotationDirection();
    }

    private void CircularMovement()
    {
        timeCounter += Time.deltaTime * speed * rotationDirection;

        float x = Mathf.Cos(timeCounter) * radius;
        float y = Mathf.Sin(timeCounter) * radius;

        Vector3 newPosition = new(x, y, 0f);

        transform.position = newPosition + initialPosition;
    }

    private void InvertRotationDirection()
    {
        rotationDirection = -rotationDirection;
    }
}
