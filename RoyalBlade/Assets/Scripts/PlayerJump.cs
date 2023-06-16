using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private float _ultModeTime;

    private float _additionalJumpForce;
    [SerializeField]
    private float _maxAdditionalJumpForce;

    [SerializeField]
    private bool _isJumping;
    private bool _canJump;
    private bool _isUsingUlt;
    private float _defaultOrtho;

    private GroundChecker _groundChecker;
    private Rigidbody2D _rig2D;
    private BoxCollider2D _collider;
    private CinemachineVirtualCamera _vCam;
    private Animator _anim;

    private JumpInput _jumpInput;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rig2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _groundChecker = GetComponentInChildren<GroundChecker>();
        _vCam = FindObjectOfType<CinemachineVirtualCamera>();
        _jumpInput = FindObjectOfType<JumpInput>();

        _defaultOrtho = _vCam.m_Lens.OrthographicSize;
    }

    private void OnEnable()
    {
        _jumpInput.OnButtonUp += OnJumpButtonUp;
        _jumpInput.OnButtonPressed += OnJumpButtonPressed;
        _jumpInput.OnUseUlt += UseUlt;
    }

    private void OnDisable()
    {
        _jumpInput.OnButtonPressed -= OnJumpButtonPressed;
        _jumpInput.OnButtonUp -= OnJumpButtonUp;
        _jumpInput.OnUseUlt -= UseUlt;
    }

    // 벽과 충돌됐을 때는 점프 안됌
    private void Update()
    {
        if(_isUsingUlt == true) return;
        // 캐싱
        bool isGrounded = _groundChecker.IsGrounded;
        bool isCollidedObstacle = CollidedObstacle();

        if (_isJumping == true && isGrounded == true)
        {
            if (_anim.GetBool(PlayerController.ID_IdleBoolean) == false)
            {
                if(_rig2D.velocity.y <= 0)
                 _anim.SetBool(PlayerController.ID_IdleBoolean, true);
            }
            SoundManager.Instance.EffectPlay("Land", Vector3.zero);
            _isJumping = false;
        }
        if (isGrounded == false)
        {
            _isJumping = true;
        }

        _canJump = !isCollidedObstacle && isGrounded == true;
    }

    private void OnJumpButtonPressed()
    {
        if (_isUsingUlt == true) return;

        if(_isJumping == false)
        {
            _additionalJumpForce = Mathf.Min(_additionalJumpForce + Time.deltaTime, _maxAdditionalJumpForce);
            _vCam.m_Lens.OrthographicSize = _defaultOrtho - _additionalJumpForce;
        }
    }

    private void OnJumpButtonUp()
    {
        if (_isUsingUlt == true) return;
        Vector2 jumpForce = Vector2.up * (_jumpForce + _jumpForce * _additionalJumpForce);
        _additionalJumpForce = 0f;
        if (CollidedObstacle() == false && _canJump)
        {
            _rig2D.velocity = Vector3.zero;
            _isJumping = true;
             _jumpInput.SuccessJump();
            if(_anim.GetBool(PlayerController.ID_IdleBoolean) == true)
            {
                _anim.SetBool(PlayerController.ID_IdleBoolean, false);
            }
            SoundManager.Instance.EffectPlay("Jump", Vector3.zero);
            _rig2D.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        _vCam.m_Lens.OrthographicSize = _defaultOrtho;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isUsingUlt == true) return;

        int a = 1 << collision.gameObject.layer;
        if ((a & LayerMask.GetMask("Obstacle")) != 0)
        {
             _rig2D.velocity = collision.GetComponentInParent<Rigidbody2D>().velocity;
        }
    }

    public bool CollidedObstacle()
    {
        Bounds bounds = _collider.bounds;
        var hit = Physics2D.OverlapBox(bounds.center, bounds.size, 0, LayerMask.GetMask("Obstacle"));

        if (hit != null)
        {
            if(_isUsingUlt == true)
            {
                var damageable = hit.GetComponent<ITakeDamageable>();
                damageable?.TakeDamage(int.MaxValue);
            }
            return true;
        }

        return false;
    }

    void UseUlt()
    {
        UltMode().Forget();
        int dur = (int)(1000 * _ultModeTime);
        CameraEffecter.Instance.PlayScreenShake(dur, 3f);
    }

    async UniTaskVoid UltMode()
    {
        if (_isUsingUlt == true) return;

        _isUsingUlt = true;
        float elapsedTime = 0f;
        while(elapsedTime < _ultModeTime)
        {
            elapsedTime += Time.deltaTime;
            CollidedObstacle();
            _rig2D.velocity = new Vector2(0, 25f);
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        _isUsingUlt = false;
    }
}