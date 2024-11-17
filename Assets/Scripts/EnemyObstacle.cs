using UnityEngine;

public class EnemyObstacle : Object
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private PoolObjectType _type;
    private float _currentLifeTime;

    public override PoolObjectType ObjectType => _type;

    private void OnEnable()
    {
        _currentLifeTime = 0f;
    }

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= _lifeTime)
            DisableObstacle();
    }

    private void DisableObstacle()
    {
        PoolManager.Instance.CoolObject(gameObject, _type);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);        
        DisableObstacle();
    }
}
