using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackStartDelay;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _target;
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

        _nextTimeToShoot = Time.time + 1 / _attackSpeed;

        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Target = _target;
    }
}
