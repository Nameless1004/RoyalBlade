using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static readonly int ID_IdleBoolean = Animator.StringToHash("Idle");

    public BoxCollider2D _collider;
    private Skill _playerUlt;
    private PlayerGuard _guardState;
    public PlayerGuard GuardState { get { return _guardState; } }

    private WeaponHolder _weaponHolder;
    private AttackInput _attackInput;

    private PlayerStatus _status = new PlayerStatus(1);
    public PlayerStatus Status { get { return _status; } }
    

    private void Awake()
    {
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
        _collider = GetComponent<BoxCollider2D>();
        _attackInput = FindObjectOfType<AttackInput>();
        _guardState = GetComponent<PlayerGuard>();
        _playerUlt = GetComponent<Skill>();
    }
   
    private void OnEnable()
    {
        _attackInput.OnButtonDown += OnAttackButtonDown;
        _attackInput.OnUseUlt += OnUseUlt;
    }
    private void OnDisable()
    {
        _attackInput.OnButtonDown -= OnAttackButtonDown;
        _attackInput.OnUseUlt -= OnUseUlt;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
           _playerUlt.Use();
        }
    }
    private void OnUseUlt()
    {
        _playerUlt.Use();
        CameraEffecter.Instance.PlayChromaticAbberation(2f, 1f);
        CameraEffecter.Instance.PlayScreenShake(2000, 0.5f);
    }

    private void OnAttackButtonDown()
    {
        if (_guardState.IsGuarding == true)
        {
            return;
        }
        else
        {
            _weaponHolder.Use();
        }
    }

}