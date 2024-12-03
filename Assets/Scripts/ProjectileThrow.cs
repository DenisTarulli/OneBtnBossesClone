using UnityEngine;

public class ProjectileThrow : MonoBehaviour, IShoot
{
    #region Members
    [Header("Stats")]
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackStartDelay;
    private float _nextTimeToShoot;

    [Header("Values")]
    [SerializeField] private PoolObjectType _projectileType;
    [SerializeField] private Transform _target;
    #endregion

    private void Start()
    {
        _nextTimeToShoot = Time.time + _attackStartDelay;
    }

    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (_nextTimeToShoot > Time.time) return;

        _nextTimeToShoot = Time.time + 1 / _attackSpeed;

        GameObject projectile = PoolManager.Instance.GetPooledObject(_projectileType);
        projectile.transform.position = transform.position;
        projectile.SetActive(true);
    }
}
