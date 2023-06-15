using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public GameObject Prefab;
    public int Id;
    public string Name;
    public int AttackPower;
    public int DefaultAngle;
}