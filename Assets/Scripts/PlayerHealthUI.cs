using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Transform _heartsContainer;
    [SerializeField] private GameObject[] _hearts;
    private int _heartsCount;

    private void Start()
    {
        _heartsCount = _heartsContainer.childCount;

        _hearts = new GameObject[_heartsCount];

        for (int i = 0; i < _heartsCount; i++)
        {
            _hearts[i] = _heartsContainer.GetChild(i).gameObject;
        }
    }

    private void DecreaseHealth(int playerCurrentHealth)
    {
        _hearts[playerCurrentHealth].SetActive(false);
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHurt += DecreaseHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= DecreaseHealth;        
    }
}
