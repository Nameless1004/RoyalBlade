using UnityEngine;

public class SpawnObject : MonoBehaviour, IDestroyable
{
    public int Damage;

    public virtual void Destroy()
    {
    }

    public virtual void Spawn()
    {
    }

    public virtual void Behaviour()
    {

    }
}
