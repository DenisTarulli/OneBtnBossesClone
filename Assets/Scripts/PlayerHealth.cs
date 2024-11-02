using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    public static event Action GameOver;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageToTake)
    {
        _currentHealth -= damageToTake;

        if (_currentHealth <= 0)
        {
            GameOver?.Invoke();
        }
    }
}
