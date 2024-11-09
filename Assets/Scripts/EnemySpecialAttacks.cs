using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySpecialAttacks : MonoBehaviour
{
    [SerializeField] private float _specialAttackCooldown;
    [SerializeField] private float _specialAttacksStartDelay;
    [SerializeField] private float _obstacleAttackDelay;
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject _obstacleWarningEffect;
    private float _playerMovementRadius;

    private void Start()
    {
        _playerMovementRadius = FindObjectOfType<PlayerMovement>().Radius;

        InvokeRepeating(nameof(TriggerSpecialAttack), _specialAttacksStartDelay, _specialAttackCooldown);
    }

    private void TriggerSpecialAttack()
    {
        StartCoroutine(ObstacleAttack());
    }

    private IEnumerator ObstacleAttack()
    {
        float newAngle = Random.Range(0f, 359f);
        Vector3 obstaclePosition = SetObstacleLocation(newAngle);
        float obstacleRotation = SetObstacleRotation(obstaclePosition);

        GameObject warningEffect = Instantiate(_obstacleWarningEffect, obstaclePosition, Quaternion.Euler(0f, 0f, obstacleRotation));

        yield return new WaitForSeconds(_obstacleAttackDelay);

        Destroy(warningEffect);
        Instantiate(_obstaclePrefab, obstaclePosition, Quaternion.Euler(0f, 0f, obstacleRotation));
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
