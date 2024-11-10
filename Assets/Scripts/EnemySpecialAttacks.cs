using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttacks : MonoBehaviour
{
    [SerializeField] private float _specialAttackCooldown;
    [SerializeField] private float _specialAttacksStartDelay;
    [SerializeField] private float _specialAttackDelay;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject _obstacleWarningEffect;
    [SerializeField] private GameObject _conePrefab;
    [SerializeField] private GameObject _coneWarningEffect;
    [SerializeField] private GameObject _projectilePrefab;
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
        Instantiate(_obstaclePrefab, obstaclePosition, Quaternion.Euler(0f, 0f, obstacleRotation));
    }

    private IEnumerator ConeAttack()
    {
        float newAngle = Random.Range(0f, 359f);

        GameObject warningEffect = Instantiate(_coneWarningEffect, transform.position, Quaternion.Euler(0f, 0f, newAngle));

        yield return new WaitForSeconds(_specialAttackDelay);

        Destroy(warningEffect);
        Instantiate(_conePrefab, transform.position, Quaternion.Euler(0f, 0f, newAngle));
    }

    private void ProjectileAttack()
    {
        Instantiate(_projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 359f)));
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
