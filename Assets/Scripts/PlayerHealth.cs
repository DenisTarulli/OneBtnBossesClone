using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    public static event Action OnPlayerDeath;
    public int CurrentHealth { get { return _currentHealth; } }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageToTake)
    {
        _currentHealth -= damageToTake;
        Debug.Log($"Player HP: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }
}
