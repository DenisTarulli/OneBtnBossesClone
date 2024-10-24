using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackStartDelay;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;
    private float nextTimeToShoot;

    private void Start()
    {
        nextTimeToShoot = Time.time + attackStartDelay;
    }

    private void Update()
    {
        Shoot();
    }    

    private void Shoot()
    {
        if (nextTimeToShoot > Time.time) return;

        nextTimeToShoot = Time.time + 1 / attackSpeed;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Target = target;
    }
}
