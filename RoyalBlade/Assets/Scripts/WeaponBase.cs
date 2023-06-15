using UnityEngine;

public interface IAttackable
{
    public void Attack();
}

public abstract class WeaponBase : MonoBehaviour, IAttackable
{
    public WeaponData Data;
    protected GameObject owner;

    public void SetWeapon(GameObject owner)
    {
        this.owner = owner;
        Initialize();
    }
    public abstract void Initialize();
    public abstract void Attack();
}