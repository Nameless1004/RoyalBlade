using UnityEngine;

public class PlayerUltObject : SpawnObject
{
    private BoxCollider2D _collider;
    private Collider2D[] _hits = new Collider2D[20];

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void Destroy()
    {
        gameObject.SetActive(false);
    }

    public override void Spawn()
    {
        Behaviour();
    }

    public override void Behaviour()
    {
        gameObject.SetActive(true);
    }

    public void HitCheck()
    {
        Bounds bounds = _collider.bounds;
        int hitCount = Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, _hits, LayerMask.GetMask("Obstacle"));
        if(hitCount > 0)
        {
            for(int i = 0; i < hitCount; i++)
            {
                ITakeDamageable damageable = _hits[i].GetComponent<ITakeDamageable>();
                damageable.TakeDamage(Damage);
            }
        }
    }
}
