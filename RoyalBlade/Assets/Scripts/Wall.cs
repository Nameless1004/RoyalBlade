using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Wall : MonoBehaviour, IDestroyable
{
    [SerializeField]
    private ParticleSystem _destroyParticle;

    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;
    public BoxCollider2D Collider2D { get { return _collider; } }

    private Health _health;

    private bool _isReleased;
    public event Action OnDestroyWall;

    public ObjectPool<Wall> OwnerPool;

    private GameManager _gameManager;
    private WallHealthViewer _healthViewer;

    private void Awake()
    {
        _renderer= GetComponent<SpriteRenderer>();
        _destroyParticle = GetComponentInChildren<ParticleSystem>();
        _collider = GetComponent<BoxCollider2D>();
        _health = GetComponent<Health>();

        _gameManager = FindObjectOfType<GameManager>();
        _healthViewer = FindObjectOfType<WallHealthViewer>();
    }

    private void OnEnable()
    {
        _health.OnDie += Destroy;
        _health.OnHit += OnHit;
        _collider.enabled = true;
        _renderer.enabled = true;
        _isReleased = false;
    }

    private void OnDisable()
    {
        _health.OnHit -= OnHit;
        _health.OnDie -= Destroy;
    }

    private void OnHit()
    {
        if (_isReleased == true) return;
        _healthViewer.SetTargetObject(_health);
        ComboCounter.AddCombo();
        SoundManager.Instance.EffectPlay("Hit", Vector3.zero);
    }

    public async void Destroy()
    {
        if (_isReleased == true) return;
        _destroyParticle.Play();
        _collider.enabled = false;
        _renderer.enabled = false;
        _healthViewer.SetTargetObject(null);
        _gameManager.AddGameScore(UnityEngine.Random.Range(10, 30));
        await UniTask.Delay(3000);
        if (this == null) return;
        OnDestroyWall?.Invoke();
        OwnerPool?.Release(this);
        _isReleased = true;
    }
}
