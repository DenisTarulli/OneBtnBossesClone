using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpecialAttacks : MonoBehaviour
{
    #region Members
    [Header("Values")]
    [SerializeField] private float _specialAttackCooldown;
    [SerializeField] private float _specialAttacksStartDelay;
    [SerializeField] private float _specialAttackDelay;
    [SerializeField] private int _specialAttacksAmount;

    [Header("Types")]
    [SerializeField] private PoolObjectType _obstacleType;
    [SerializeField] private PoolObjectType _coneType;
    [SerializeField] private PoolObjectType _specialProjectileType;

    [Header("Prefabs")]
    [SerializeField] private GameObject _obstacleWarningEffect;
    [SerializeField] private GameObject _coneWarningEffect;
    
    private float _playerMovementRadius;
    #endregion

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
        Vector3 newLocation = SetObstacleLocation(newAngle);
        float newRotation = SetObstacleRotation(newLocation);

        AttackWarning(_obstacleWarningEffect, newRotation, newLocation);

        yield return new WaitForSeconds(_specialAttackDelay);

        AttackObject(_obstacleType, newRotation, newLocation);
    }

    private IEnumerator ConeAttack()
    {
        float newAngle = Random.Range(0f, 359f);

        AttackWarning(_coneWarningEffect, newAngle, transform.position);

        yield return new WaitForSeconds(_specialAttackDelay);

        AttackObject(_coneType, newAngle, transform.position);
    }

    private void AttackWarning(GameObject warning, float angle, Vector3 newPosition)
    {
        GameObject warningEffect = Instantiate(warning, newPosition, Quaternion.Euler(0f, 0f, angle));
        Destroy(warningEffect, _specialAttackDelay);
    }

    private void AttackObject(PoolObjectType type, float angle, Vector3 newPosition)
    {
        GameObject newObj = PoolManager.Instance.GetPooledObject(type);
        newObj.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(0f, 0f, angle));
        newObj.SetActive(true);
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
