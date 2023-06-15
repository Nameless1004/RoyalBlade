using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public PlayerController Owner;
    public WeaponBase _currentWeapon;
    public WeaponData _currentWeaponData;

    private void Awake()
    {
        Owner = GetComponentInParent<PlayerController>();
        if(_currentWeapon == null)
        {
            EquipWeapon(Instantiate(Resources.Load<WeaponBase>("Sword"), transform));
        }
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (_currentWeapon != null) Destroy(_currentWeapon.gameObject);
        _currentWeapon = newWeapon;
        _currentWeaponData = newWeapon.Data;
        _currentWeapon.SetWeapon(Owner.gameObject);
    }

    public void Use()
    {
        _currentWeapon.Attack();
    }
}