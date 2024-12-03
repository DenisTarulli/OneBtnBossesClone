using UnityEngine;

public class EnemyCombat : MonoBehaviour, IShoot
{
    #region Members
    [SerializeField] private PoolObjectType _projectileType;

    [Header("Combat stats")]
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackStartDelay;
    private float _nextTimeToShoot;
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

        _nextTimeToShoot = Time.time + 1f / _attackSpeed;

        GameObject bullet = PoolManager.Instance.GetPooledObject(_projectileType);
        bullet.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        bullet.SetActive(true);
    }
}
