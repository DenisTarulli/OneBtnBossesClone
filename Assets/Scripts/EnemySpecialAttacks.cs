using System.Collections;
using UnityEngine;

public class EnemySpecialAttacks : MonoBehaviour
{
    [SerializeField] private float _specialAttackCooldown;
    [SerializeField] private float _specialAttacksStartDelay;
    [SerializeField] private float _specialAttackDelay;
    [SerializeField] private PoolObjectType _obstacleType;
    [SerializeField] private GameObject _obstacleWarningEffect;
    [SerializeField] private PoolObjectType _coneType;
    [SerializeField] private GameObject _coneWarningEffect;
    [SerializeField] private PoolObjectType _specialProjectileType;
    [SerializeField] private int _specialAttacksAmount;
    private float _playerMovementRadius;

    private void Start()
    {
        _playerMovementRadius = FindObjectOfType<PlayerMovement>().Radius;

        InvokeRepeating(nameof(TriggerSpecialAttack), _specialAttacksStartDelay, _specialAttackCooldown);
    }

    private void TriggerSpecialAttack()
    {
        int newAttack = Random.Range(1, _specialAttacksAmount + 1);

        switch (newAttack)
        {
            case 1:
                StartCoroutine(ObstacleAttack());
                break;
            case 2:
                StartCoroutine(ConeAttack());
                break;
            case 3:
                ProjectileAttack();
                break;
        }
    }

    private IEnumerator ObstacleAttack()
    {
        float newAngle = Random.Range(0f, 359f);
        Vector3 obstaclePosition = SetObstacleLocation(newAngle);
        float obstacleRotation = SetObstacleRotation(obstaclePosition);

        GameObject warningEffect = Instantiate(_obstacleWarningEffect, obstaclePosition, Quaternion.Euler(0f, 0f, obstacleRotation));

        yield return new WaitForSeconds(_specialAttackDelay);

        Destroy(warningEffect);

        GameObject obstacle = PoolManager.Instance.GetPooledObject(_obstacleType);
        obstacle.transform.SetPositionAndRotation(obstaclePosition, Quaternion.Euler(0f, 0f, obstacleRotation));
        obstacle.SetActive(true);
    }

    private IEnumerator ConeAttack()
    {
        float newAngle = Random.Range(0f, 359f);

        GameObject warningEffect = Instantiate(_coneWarningEffect, transform.position, Quaternion.Euler(0f, 0f, newAngle));

        yield return new WaitForSeconds(_specialAttackDelay);

        Destroy(warningEffect);

        GameObject cone = PoolManager.Instance.GetPooledObject(_coneType);
        cone.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, newAngle));
        cone.SetActive(true);
    }

    private void ProjectileAttack()
    {
        GameObject projectile = PoolManager.Instance.GetPooledObject(_specialProjectileType);
        projectile.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 359f)));
        projectile.SetActive(true);
    }

    private Vector3 SetObstacleLocation(float angle)
    {
        float x = Mathf.Cos(angle) * _playerMovementRadius;
        float y = Mathf.Sin(angle) * _playerMovementRadius;

        Vector3 newPosition = new(x, y, 0f);

        return newPosition;
    }

    private float SetObstacleRotation(Vector3 newPosition)
    {
        Vector3 obstacleDirectionToZero = (Vector3.zero - newPosition).normalized;
        float obstacleNewRotation = (Mathf.Atan2(obstacleDirectionToZero.y, obstacleDirectionToZero.x) * Mathf.Rad2Deg) - 90f;

        return obstacleNewRotation;
    }
}
