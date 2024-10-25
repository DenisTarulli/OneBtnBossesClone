using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _lifeTime;

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
        Destroy(gameObject);
    }
}
