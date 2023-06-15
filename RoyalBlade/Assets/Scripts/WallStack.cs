using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class WallStack : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D _rig2D;
    [SerializeField] private Wall Prefab;
    [SerializeField] private int _wallCount;
    [SerializeField, Range(0, 2)] private float _spawnDistInterval;

    private ObjectPool<Wall> _pool;

    public ObjectPool<WallStack> Owner;

    private void Awake()
    {
        _rig2D = GetComponent<Rigidbody2D>();
        _pool = new ObjectPool<Wall>(CreateFunc, OnGet, OnRelease, null, true, 10, 100);
    }

    private void OnDestroy()
    {
        _pool.Clear();
        _pool.Dispose();
        Owner = null;
    }

    private void OnEnable()
    {
        SetWallStacks(_wallCount);
    }

    private void OnDisable()
    {
        
    }


    private void SetWallStacks(int count)
    {
        Wall[] walls = new Wall[count];
        Vector3 originPos = transform.position;
        for (int i = 0; i < count; ++i)
        {
            walls[i] = _pool.Get();
            float distInterval = walls[0].Collider2D.bounds.size.y + _spawnDistInterval;
            Vector2 newPosition = originPos;
            newPosition.y = originPos.y + (i * distInterval);
            walls[i].transform.position = newPosition;
        }
    }

    public void Knockback(Vector2 force)
    {
        _rig2D.velocity = Vector2.zero;
        _rig2D.AddForce(force, ForceMode2D.Impulse);
    }

    public Wall CreateFunc()
    {
        var wall = Instantiate(Prefab, transform);
        wall.OwnerPool = this._pool;
        return wall;
    }

    public void OnGet(Wall wall)
    {
        wall.gameObject.SetActive(true);
    }

    public void OnRelease(Wall wall)
    {
        wall.gameObject.SetActive(false);
        if (_pool.CountActive == 1)
            Owner.Release(this);
    }

}
