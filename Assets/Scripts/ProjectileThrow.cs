using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackStartDelay;
    [SerializeField] private PoolObjectType _projectileType;
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

        GameObject projectile = PoolManager.Instance.GetPooledObject(_projectileType);
        projectile.transform.position = transform.position;
        projectile.SetActive(true);
    }
}
