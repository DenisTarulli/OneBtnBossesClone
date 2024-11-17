using UnityEngine;

public class EnemyProjectile : Object
{
    [SerializeField] private float _shotSpeed;
    [SerializeField] private float _lifeTime;
    private float _currentLifeTime;
    [SerializeField] private PoolObjectType _type;
    private const string IS_PLAYER = "Player";

    public override PoolObjectType ObjectType => _type;

    private void OnEnable()
    {
        _currentLifeTime = 0f;
    }

    private void Update()
    {
        ProjectileBehaviour();

        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= _lifeTime)
            DisableProjectile();
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
        if (!collision.gameObject.CompareTag(IS_PLAYER)) return;
        
        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);

        DisableProjectile();
    }
}
