using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private float _lifeTime;
    private Transform _target;
    public Transform Target { get => _target; set => _target = value; }

    private void Start()
    {
        SetRotation();
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        ProjectileBehaviour();
    }

    private void SetRotation()
    {
        Vector3 directionToTarget;

        if (_target != null)
            directionToTarget = (_target.position - transform.position).normalized;
        else
            directionToTarget = (Vector3.zero - transform.position).normalized;

        float angle = (Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg) - 90;
        transform.eulerAngles = new(0f, 0f, angle);
    }

    private void ProjectileBehaviour()
    {
        transform.Translate(Time.deltaTime * _shotSpeed * transform.up, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            enemyHealth.TakeDamage(_projectileDamage);

        Destroy(gameObject);
    }
}
