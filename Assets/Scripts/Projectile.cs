using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private PoolObjectType _type;

    private void OnEnable()
    {
        SetRotation();
        Invoke(nameof(DisableProjectile), _lifeTime);
    }

    private void Update()
    {
        ProjectileBehaviour();
    }

    private void SetRotation()
    {
        Vector3 directionToTarget;

        directionToTarget = (Vector3.zero - transform.position).normalized;

        float angle = (Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg) - 90;
        transform.eulerAngles = new(0f, 0f, angle);
    }

    private void ProjectileBehaviour()
    {
        transform.Translate(Time.deltaTime * _shotSpeed * transform.up, Space.World);
    }

    private void DisableProjectile()
    {
        PoolManager.Instance.CoolObject(gameObject, _type);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            enemyHealth.TakeDamage(_projectileDamage);

        CancelInvoke();
        DisableProjectile();
    }
}
