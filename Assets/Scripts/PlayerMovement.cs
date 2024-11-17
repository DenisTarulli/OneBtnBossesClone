using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _radius;
    private Vector3 _initialPosition;
    private int _rotationDirection;
    private float _timeCounter;
    private float _activeSpeedMultiplier = 1f;
    private bool _speedPowerUpActive;

    private PlayerInputsAsset _playerInputsAsset;

    public float ActiveSpeedMultiplier { get => _activeSpeedMultiplier; set => _activeSpeedMultiplier = value; }
    public bool SpeedPowerUpActive { get => _speedPowerUpActive; set => _speedPowerUpActive = value; }
    public float Radius { get => _radius; }

    private void Awake()
    {
        _playerInputsAsset = new PlayerInputsAsset();
    }

    private void OnEnable()
    {
        _playerInputsAsset.Player.Movement.performed += InvertRotationDirection;
        _playerInputsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputsAsset.Player.Movement.performed -= InvertRotationDirection;        
        _playerInputsAsset.Player.Disable();
    }

    private void Start()
    {
        _initialPosition = transform.position;
        _rotationDirection = 1;
    }

    private void Update()
    {
        CircularMovement();
    }

    private void CircularMovement()
    {
        _timeCounter += Time.deltaTime * _speed * _rotationDirection * ActiveSpeedMultiplier;

        float x = Mathf.Cos(_timeCounter) * _radius;
        float y = Mathf.Sin(_timeCounter) * _radius;

        Vector3 newPosition = new(x, y, 0f);

        transform.position = newPosition + _initialPosition;
    }

    private void InvertRotationDirection(InputAction.CallbackContext obj)
    {
        if (_speedPowerUpActive) return;

        _rotationDirection = -_rotationDirection;
    }
}
