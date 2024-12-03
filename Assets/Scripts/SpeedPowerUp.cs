using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpeedPowerUp : MonoBehaviour
{
    #region Members
    [Header("Values")]
    [SerializeField, Range(3f, 10f)] private float _powerUpDuration;
    [SerializeField, Range(8f, 20f)] private float _powerUpCooldown;
    [SerializeField, Range(1.2f, 2.5f)] private float _speedMultiplier;
    [SerializeField] private float _maxCharge;
    private float _currentCharge;
    private bool _powerUpActive;

    [Header("References")]
    [SerializeField] private Slider _chargeSlider;
    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;

    private PlayerInputsAsset _playerInputsAsset;
    #endregion

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        SetMaxSliderValue();
    }
    private void Awake()
    {
        _playerInputsAsset = new PlayerInputsAsset();
    }

    private void OnEnable()
    {
        _playerInputsAsset.Player.Movement.performed += ActivatePowerUp;
        _playerInputsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputsAsset.Player.Movement.performed -= ActivatePowerUp;
        _playerInputsAsset.Player.Disable();
    }

    private void Update()
    {
        UpdateSliderValue();

        if (_powerUpActive)
            ConsumeCharge();

        if (_currentCharge < _maxCharge && !_powerUpActive)
            Recharge();
    }

    private void ActivatePowerUp(InputAction.CallbackContext obj)
    {
        if (!(_currentCharge >= _maxCharge)) return;

        StartCoroutine(PowerUp());
    }

    private IEnumerator PowerUp()
    {
        _powerUpActive = true;
        _playerHealth.CanTakeDamage = false;
        _playerMovement.ActiveSpeedMultiplier = _speedMultiplier;
        _playerMovement.SpeedPowerUpActive = true;
        _playerHealth.SetInvulnerabilityBubble(true);

        yield return new WaitUntil(() => _currentCharge <= 0f);

        _playerMovement.ActiveSpeedMultiplier = 1f;
        _powerUpActive = false;
        _playerHealth.CanTakeDamage = true;
        _playerMovement.SpeedPowerUpActive = false;
        _playerHealth.SetInvulnerabilityBubble(false);
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
        _currentCharge -= Time.deltaTime * (_maxCharge / _powerUpDuration);
    }

    private void Recharge()
    {
        _currentCharge += Time.deltaTime * (_maxCharge / _powerUpCooldown);
        Mathf.Clamp(_currentCharge, 0f, _maxCharge);
    }
}
