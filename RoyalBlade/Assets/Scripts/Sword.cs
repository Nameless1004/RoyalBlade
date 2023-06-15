using UnityEngine;

public class Sword : WeaponBase
{
    private float _angle;
    public bool _isFlipped = false;
    private Collider2D[] hits = new Collider2D[20];
    private PlayerStatus _ownerStatus;
    private AttackInput _attackInput;
    private GameManager _gameManager;

    public override void Initialize()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Data.DefaultAngle);
        _angle = Data.DefaultAngle;
        _ownerStatus = owner.GetComponent<PlayerController>().Status;
        _attackInput = FindObjectOfType<AttackInput>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public override void Attack()
    {
        SoundManager.Instance.EffectPlay("Swing", Vector3.zero);
        _angle *= -1f;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
        Vector2 spawnPosition = (Vector2)transform.position + Vector2.up * 1.3f;
        _gameManager.FxPooler.GetFx("Swing", spawnPosition, Quaternion.identity);
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, 2.3f, hits, LayerMask.GetMask("Obstacle"));
        
        if(hitCount > 0)
        {
            _attackInput?.SuccessAttack();
            for (int i = 0; i < hitCount; i++)
            {
                ITakeDamageable damageable = hits[i].GetComponent<ITakeDamageable>();
                int minDamage = Data.AttackPower + _ownerStatus.Attack;
                int maxDamage = minDamage * 2;
                int damage = Random.Range(minDamage, maxDamage);
                damageable.TakeDamage(damage);
            }
        }
    }
}
