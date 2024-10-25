using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackStartDelay;
    private float _nextTimeToShoot;

    private void Start()
    {
        _nextTimeToShoot = Time.time + _attackStartDelay;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (_nextTimeToShoot > Time.time) return;

        _nextTimeToShoot = Time.time + 1f / _attackSpeed;

        Instantiate(_projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
}
