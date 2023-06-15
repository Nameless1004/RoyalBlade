using UnityEngine;

public class PlayerStatus
{
    public PlayerStatus(int Attack)
    {
        this._originalAttack = Attack;
    }

    private int _originalAttack;
    public int  additionalAttack;
    public int Attack { get { return _originalAttack + additionalAttack;  } }
}