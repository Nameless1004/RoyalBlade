using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float LifeTime = 5f;
    private float _elapsedTime = 0f;
    private float _bulletSpeed;
    private int _damage;
    private bool _isFire = false;
    private Vector3 _dir;

    private bool _isReleased;
    private ObjectPool<Bullet> _owner;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void SetBullet(ObjectPool<Bullet> owner, Vector3 position, Vector3 dir, float speed, int damage)
    {
        _owner = owner;
        _isFire = false;
        _damage = damage;
        _bulletSpeed = speed;
        _elapsedTime = 0f;
        transform.position = position;
        _dir = dir;
        _isReleased = false;
    }

    public void Fire()
    {
        _isFire = true;
    }

    public void Update()
    {
        if (_isReleased == true) return;

        if (_isFire)
        {
            _elapsedTime += Time.deltaTime;
            transform.position += _dir * _bulletSpeed * Time.deltaTime;
            if(_elapsedTime > LifeTime)
            {
                _elapsedTime = 0f;
                _isFire = false;
                _owner.Release(this);
                _isReleased = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isReleased == true) return;

        int a = 1 << collision.gameObject.layer;
        int b = LayerMask.GetMask("Obstacle");

        if((a&b) != 0)
        {
            ITakeDamageable damageable = collision.gameObject.GetComponent<ITakeDamageable>();
            damageable.TakeDamage(_damage);
            _gameManager.FxPooler.GetFx("BulletHit", collision.ClosestPoint(transform.position), Quaternion.identity);
            _owner.Release(this);
            _isReleased = true;
        }
    }
}
