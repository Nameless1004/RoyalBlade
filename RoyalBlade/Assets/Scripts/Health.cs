using System;
using UnityEngine;

public interface ITakeDamageable
{
    public void TakeDamage(int damage);
}

public class Health : MonoBehaviour, ITakeDamageable
{

    public int MaxHp;
    private int _currentHp;
    public int CurrentHp { get { return _currentHp; } }
    private float _ratio;
    public event Action OnDie;
    public event Action OnHit;
    public event Action<float> OnHealthChanged;

    private void OnEnable()
    {
        _currentHp = MaxHp;
        _ratio = _currentHp / MaxHp;
        OnHealthChanged?.Invoke(_ratio);
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_ratio);
    }

    public void TakeDamage(int damage)
    {
        _currentHp = Mathf.Clamp(_currentHp - damage, 0, MaxHp);
        _ratio = _currentHp / (float)MaxHp;
        OnHealthChanged?.Invoke(_ratio);
        OnHit?.Invoke();
        TextPooler.Instance.PopupDamage(damage, transform.position);
        if (_currentHp == 0) 
        {
            OnDie?.Invoke();
        }

    }
}
