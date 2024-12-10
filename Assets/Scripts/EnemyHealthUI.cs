using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    #region Members
    [SerializeField] private Slider _enemyHealthBar;
    #endregion

    public void SetMaxHealth(float maxHealth, float currentHealth)
    {
        _enemyHealthBar.maxValue = maxHealth;
        _enemyHealthBar.value = currentHealth;
    }

    public void SetHealth(float health)
    {
        _enemyHealthBar.value = health;
    }
}
