using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    private EnemyHealthUI _enemyHealthUI;
    
    public static event Action OnEnemyKill;
    public float MaxHealth { get => _maxHealth; }
    public float CurrentHealth { get => _currentHealth; }
    #endregion

    private void Start()
    {
        _currentHealth = _maxHealth;
        _enemyHealthUI = GetComponent<EnemyHealthUI>();

        _enemyHealthUI.SetMaxHealth(_maxHealth, _currentHealth);
    }

    public void TakeDamage(float damageToTake)
    {
        _currentHealth -= damageToTake;

        _enemyHealthUI.SetHealth(_currentHealth);

        if (_currentHealth <= 0f)
        {
            Destroy(gameObject);
            OnEnemyKill?.Invoke();
        }
    }
}
