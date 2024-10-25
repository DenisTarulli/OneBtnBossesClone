using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damageToTake)
    {
        _currentHealth -= damageToTake;

        Debug.Log($"Enemy HP: {_currentHealth}");

        if (_currentHealth <= 0f)
            Destroy(gameObject);
    }
}
