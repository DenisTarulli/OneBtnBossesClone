using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum MovementType
{
    Invert,
    Dash
}

public class PlayerMovement : MonoBehaviour
{
    #region Members
    [Header("Movement stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _radius;
    [SerializeField] private MovementType _movementType;
    private int _rotationDirection;
    private Vector3 _initialPosition;

    [Header("Dash movement")]
    [SerializeField] private Slider _chargeSlider;
    [SerializeField] private float _maxCharge;
    [SerializeField] private float _chargeMultiplier;
    [SerializeField] private float _rechargeMultiplier;
    private float _currentCharge;
    private PlayerHealth _playerHealth;

    [Header("UI elements")]
    [SerializeField] private GameObject _invertSelectedIndicator;
    [SerializeField] private GameObject _dashSelectedIndicator;
    [SerializeField] private GameObject _chargeBar;
    private float _activeSpeedMultiplier = 1f;
    private float _timeCounter;
    private bool _isDashing;

    private PlayerInputsAsset _playerInputsAsset;

    public bool IsDashing { get => _isDashing; }
    public MovementType MovementType { get => _movementType; set => _movementType = value; }
    public float Radius { get => _radius; }
    public float CurrentCharge { get => _currentCharge; }
    public float MaxCharge { get => _maxCharge; }
    public GameObject ChargeBar { get => _chargeBar; set => _chargeBar = value; }
    #endregion

    private void Awake()
    {
        _playerInputsAsset = new PlayerInputsAsset();
    }

    private void OnEnable()
    {
        _playerInputsAsset.Player.Movement.performed += InvertRotationDirection;
        _playerInputsAsset.Player.Movement.performed += StartDash;
        _playerInputsAsset.Player.Movement.canceled += StopDash;
        _playerInputsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputsAsset.Player.Movement.performed -= InvertRotationDirection;   
        _playerInputsAsset.Player.Movement.performed -= StartDash;        
        _playerInputsAsset.Player.Movement.canceled -= StopDash;        
        _playerInputsAsset.Player.Disable();
    }

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();

        if (_movementType == MovementType.Invert)
            _chargeBar.SetActive(false);

        SetMaxSliderValue();

        _initialPosition = transform.position;
        _rotationDirection = 1;
    }

    private void Update()
    {
        CircularMovement();

        UpdateSliderValue();

        if (_movementType == MovementType.Dash)
            DashBehaviour();        
    }

    public void SelectMovementType(string type)
    {
        switch (type)
        {
            case "Invert":
                _movementType = MovementType.Invert;
                _invertSelectedIndicator.SetActive(true);
                _dashSelectedIndicator.SetActive(false);
                break;
            case "Dash":
                _movementType = MovementType.Dash;
                _dashSelectedIndicator.SetActive(true);
                _invertSelectedIndicator.SetActive(false);
                break;
        }
    }

    private void CircularMovement()
    {
        _timeCounter += Time.deltaTime * _speed * _rotationDirection * _activeSpeedMultiplier;

        float x = Mathf.Cos(_timeCounter) * _radius;
        float y = Mathf.Sin(_timeCounter) * _radius;

        Vector3 newPosition = new(x, y, 0f);

        transform.position = newPosition + _initialPosition;
    }

    private void DashBehaviour()
    {
        if (GetDashState())
            ConsumeCharge();
        else
            Recharge();

        if (_currentCharge <= 0f)
            AlternativeStopDash();
    }

    private void StartDash(InputAction.CallbackContext obj)
    {
        if (_movementType != MovementType.Dash || _currentCharge <= 0f) return;

        _isDashing = true;
        _playerHealth.CanTakeDamage = false;
        _playerHealth.SetInvulnerabilityBubble(true);

        _activeSpeedMultiplier = 3f;
    }

    private void StopDash(InputAction.CallbackContext obj)
    {
        if (_movementType != MovementType.Dash) return;

        _isDashing = false;
        _playerHealth.CanTakeDamage = true;
        _playerHealth.SetInvulnerabilityBubble(false);
        _activeSpeedMultiplier = 1f;
    }

    private void AlternativeStopDash()
    {
        _isDashing = false;
        _playerHealth.CanTakeDamage = true;
        _playerHealth.SetInvulnerabilityBubble(false);
        _activeSpeedMultiplier = 1f;
    }

    private bool GetDashState()
    {
        return _isDashing;
    }
    private void SetMaxSliderValue()
    {
        _chargeSlider.maxValue = _maxCharge;
        _chargeSlider.value = _currentCharge;
    }

    private void UpdateSliderValue()
    {
        _chargeSlider.value = _currentCharge;
    }

    private void ConsumeCharge()
    {
        _currentCharge -= Time.deltaTime * _chargeMultiplier;
    }

    private void Recharge()
    {
        if (_currentCharge == _maxCharge) return;

        _currentCharge += Time.deltaTime * _rechargeMultiplier;
        _currentCharge = Mathf.Clamp(_currentCharge, 0f, _maxCharge);
    }

    private void InvertRotationDirection(InputAction.CallbackContext obj)
    {
        if (_movementType != MovementType.Invert) return;

        _rotationDirection = -_rotationDirection;
    }
}
