using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Members
    [Header("Stats")]
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    [Header("Prefab")]
    [SerializeField] private GameObject invulerabilityBubble;

    private bool _canTakeDamage;

    public static event Action OnPlayerDeath;
    public static event Action<int> OnPlayerHurt;
    public int CurrentHealth { get => _currentHealth; }
    public bool CanTakeDamage { get => _canTakeDamage; set => _canTakeDamage = value; }
    #endregion

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        _canTakeDamage = true;
    }

    public void SetInvulnerabilityBubble(bool newState)
    {
        invulerabilityBubble.SetActive(newState);
    }

    public void TakeDamage(int damageToTake)
    {
        if (!_canTakeDamage) return;

        _currentHealth -= damageToTake;
        OnPlayerHurt?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }
}
