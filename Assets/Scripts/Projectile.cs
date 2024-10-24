using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float shotSpeed;
    private Transform target;
    public Transform Target { get => target; set => target = value; }

    private void Start()
    {
        SetRotation();
    }

    private void Update()
    {
        ProjectileBehaviour();
    }

    private void SetRotation()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = (Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg) - 90;
        transform.eulerAngles = new(0f, 0f, angle);
    }

    private void ProjectileBehaviour()
    {
        transform.Translate(Time.deltaTime * shotSpeed * transform.up, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
