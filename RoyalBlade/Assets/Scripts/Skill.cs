using UnityEngine;

public abstract class Skill : MonoBehaviour, IUseable
{
    public int SkillDamage;

    public abstract void Use();
}




