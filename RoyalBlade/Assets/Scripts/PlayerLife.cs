using System;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public const int PLAYER_LIFE_COUNT = 3;
    public int CurretPlayerLifeCount;

    private void Awake()
    {
        CurretPlayerLifeCount = PLAYER_LIFE_COUNT;
    }

    public event Action OnPlayerDie;
    public event Action<int> OnPlayerLifeChanged;
    public event Action OnGroundCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int a = 1 << collision.gameObject.layer;
        int b = LayerMask.GetMask("Obstacle");
        if((a & b) != 0)
        {
            SoundManager.Instance.EffectPlay("Ground",Vector3.zero);
            CurretPlayerLifeCount = Math.Max(0, CurretPlayerLifeCount - 1);
            OnPlayerLifeChanged?.Invoke(CurretPlayerLifeCount);
            OnGroundCollided?.Invoke();
            CameraEffecter.Instance.PlayScreenBlink();
            CameraEffecter.Instance.PlayScreenShake(200, 3f);
            ComboCounter.ResetCombo();
            if (CurretPlayerLifeCount == 0)
            {
                OnPlayerDie?.Invoke();
            }

        }
    }

}