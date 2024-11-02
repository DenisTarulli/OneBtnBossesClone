using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _lifeTime;
    private const string IS_PLAYER = "Player";

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        ProjectileBehaviour();
    }

    private void ProjectileBehaviour()
    {
        transform.Translate(Time.deltaTime * _shotSpeed * transform.up, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(IS_PLAYER))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
