using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Pet : MonoBehaviour
{
    public GameObject Owner;

    public Vector2 OffSetPosition;

    public int BonusAttack;
    [Range(0f, 1f)]
    public float smoothDamping;
    Vector2 velocity;

    public Bullet BulletPrefab;
    ObjectPool<Bullet> _bulletPool;
    public Transform BulletObjectPool;
    [SerializeField]
    private float _bulletSpawnIntervalPerSecond;
    private float _bulletSpawnInterval;
    private float _elapsedTime;


    private void Awake()
    {
        transform.position = Owner.transform.position + (Vector3)OffSetPosition;
        _bulletPool = new ObjectPool<Bullet>(CreateFunc, ActionOnGet, ActionOnRelease, null);
        _bulletSpawnInterval = 1 / (float)_bulletSpawnIntervalPerSecond;
    }

    private void OnEnable()
    {
        Owner = FindObjectOfType<PlayerController>().gameObject;
        var controller = Owner.GetComponentInChildren<PlayerController>();
        controller.Status.additionalAttack += BonusAttack;
    }

    private void OnDisable()
    {
        if (Owner == null) return;

        PlayerStatus ownerStatus = Owner.GetComponent<PlayerController>().Status;
        if (ownerStatus != null)
        {
            ownerStatus.additionalAttack -= BonusAttack;
        }
    }

    private void LateUpdate()
    {
        Vector2 targetPos = (Vector2)Owner.transform.position + OffSetPosition;
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothDamping);
    }


    private Bullet CreateFunc()
    {
        var bullet = Instantiate(BulletPrefab, BulletObjectPool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        Vector3 ownerPos = Owner.transform.position;
        ownerPos.y += 50f;
        Vector3 dir = ownerPos - transform.position;
        int damage = Random.Range(1, BonusAttack + 1);
        bullet.SetBullet(_bulletPool, transform.position, dir.normalized, 30f, damage);
        bullet.Fire();
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _bulletSpawnInterval)
        {
            _elapsedTime = 0f;
            _bulletPool.Get();
        }
    }
}
