﻿using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField]
    private float _pushForce;

    [SerializeField]
    private float _guardCoolTime;

    private bool _isCoolTime;

    [SerializeField]
    private GameObject _shieldObject;

    private GroundChecker _groundChecker;

    // 가드 성공 시 이벤트
    public event Action OnGuardSuccess;

    private GuardCollider _guardCollider;
    private WeaponHolder _weaponHolder;
    private bool _guardReady = true;
    public bool IsGuarding { get; set; }
    private bool _guardSuccess = false;

    private GuardInput _guardInput;

    private void Awake()
    {
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
        _guardCollider = GetComponentInChildren<GuardCollider>();
        _shieldObject.gameObject.SetActive(false);
        _guardInput = FindObjectOfType<GuardInput>();
        _groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void OnEnable()
    {
        _guardCollider.OnTriggerStay += OnTriggerStayGuardCollider;
        _guardInput.OnButtonPressed += OnGuardButtonPressed;
        _guardInput.OnButtonUp += OnGuardButtonUp;
    }

    private void OnDisable()
    {
        _guardInput.OnButtonUp -= OnGuardButtonUp;
        _guardInput.OnButtonPressed -= OnGuardButtonPressed;
        _guardCollider.OnTriggerStay -= OnTriggerStayGuardCollider;
    }

    private void OnGuardButtonPressed()
    {
        if (_guardReady == true && _guardSuccess == false)
        {
            _shieldObject.SetActive(true);
            IsGuarding = true;
            _weaponHolder.gameObject.SetActive(false);
        }
    }



    private void OnGuardButtonUp()
    {
        _guardSuccess = false;
        IsGuarding = false;
        _shieldObject.SetActive(false);
        _weaponHolder.gameObject.SetActive(true);
        if(_isCoolTime == false)
        {
            ShieldCoolTimer().Forget();
        }
    }

    public void OnTriggerStayGuardCollider(Collider2D col)
    {
        if (_guardInput.IsButtonPressed && _guardReady == true && _guardSuccess == false)
        {
            IKnockbackable knockbackable = col.GetComponentInParent<IKnockbackable>();
            Vector3 knockbackForce = Vector3.up * _pushForce;
            knockbackable?.Knockback(knockbackForce);
            
            if(knockbackable != null)
            {
                if (_groundChecker.IsGrounded == true)
                {
                    ComboCounter.ResetCombo();
                }
                _guardSuccess = true;
                SoundManager.Instance.EffectPlay("Shield", Vector3.zero);
                OnGuardSuccess?.Invoke();
            }

            _guardReady = false;
            _shieldObject.SetActive(true);
            _weaponHolder.gameObject.SetActive(false);
            if(_isCoolTime == false)
            {
                ShieldCoolTimer().Forget();
            }
        }
    }

    async UniTaskVoid ShieldCoolTimer()
    {
        if (_isCoolTime == true) return;

        _isCoolTime = true;
        float elapsedTime = 0;
        while (elapsedTime - 0.1f < _guardCoolTime)
        {
            elapsedTime += Time.deltaTime;
            _guardInput.UpdateCoolTime(elapsedTime / _guardCoolTime);
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        _guardReady = true;
        _isCoolTime = false;
    }
}
