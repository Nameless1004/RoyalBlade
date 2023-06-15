using UnityEngine;

public class FxObject : MonoBehaviour
{
    public FxPooler PoolOwner { private get; set; }
    private AnimatorOverrideController overrideController;
    private Material _defaultFxMaterial;

    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
        _defaultFxMaterial = _spriteRenderer.material;
    }

    public void SetAnimationClip(AnimationClip clip)
    {
        if(null == overrideController)
        {
            overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = overrideController;
        }

        overrideController["Effect"] = clip;
    }

    private void OnEnable()
    {
        _animator.Play("Effect");
    }

    private void Update()
    {
        if(_animator?.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            PoolOwner?.Release(this);
        }
    }

    private void OnDestroy()
    {
        overrideController = null;
    }

    public void ChangeLayerOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public void DestroyFx()
    {
        Destroy(gameObject);
    }    
    public void DestroyParent()
    {
        Destroy(transform.gameObject);
    }

    public void DestroyRealParent()
    {
        Destroy(transform.parent.gameObject);
    }

    public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        if(parent != null)
        {
            transform.SetParent(parent);
        }
    }

    public void SetTransform(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        transform.position = position;
        transform.rotation = rotation;
        if (parent != null)
        {
            transform.SetParent(parent);
        }
    }

    public void Reset()
    {
        SetTransform(Vector3.zero, Quaternion.identity, Vector3.one);
        _spriteRenderer.material = _defaultFxMaterial;
    }

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
}
