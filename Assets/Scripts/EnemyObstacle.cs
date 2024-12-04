using UnityEngine;

public class EnemyObstacle : Object
{
    #region Members
    [SerializeField] private float _lifeTime;
    [SerializeField] private PoolObjectType _type;
    private float _currentLifeTime;

    public override PoolObjectType ObjectType => _type;
    #endregion

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

        bool playerDashing = collision.gameObject.GetComponent<PlayerMovement>().IsDashing;

        if (playerDashing) return;

        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);        
        DisableObstacle();
    }
}
