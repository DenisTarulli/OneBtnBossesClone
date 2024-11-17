using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private GameObject invulerabilityBubble;
    private int _currentHealth;
    private bool _canTakeDamage;

    public static event Action OnPlayerDeath;
    public int CurrentHealth { get => _currentHealth; }
    public bool CanTakeDamage { get => _canTakeDamage; set => _canTakeDamage = value; }

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

        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }
}
