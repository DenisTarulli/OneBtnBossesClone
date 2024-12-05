using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    public static event Action OnEnemyKill;
    #endregion

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damageToTake)
    {
        _currentHealth -= damageToTake;

        if (_currentHealth <= 0f)
        {
            Destroy(gameObject);
            OnEnemyKill?.Invoke();
        }
    }
}
