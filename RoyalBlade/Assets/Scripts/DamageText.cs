using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour
{

    [SerializeField] private float _lifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _maxDamageColor;

    private TextPooler _ownerObject;
    private ObjectPool<DamageText> _owner;
    private TMP_Text _text;

    private bool _isReleased;

    public void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void SetOwner(TextPooler ownerObj, ObjectPool<DamageText> owner) 
    {
        _owner = owner;
        _ownerObject = ownerObj;
    }

    public void SetUp(string damage, Vector2 startPosition)
    {
        _text.text = damage;

        if(_ownerObject.DamageStringToInt[damage] != int.MaxValue)
        {
            _text.color = _defaultColor;
        }
        else
        {
            _text.color = _maxDamageColor;
        }
        _text.rectTransform.anchoredPosition = startPosition;
        _text.rectTransform.localScale = _defaultScale;
        DamageTextEffect().Forget();
        _isReleased = false;
    }

    private async UniTaskVoid DamageTextEffect()
    {
        float t = 0f;
        Color col = _text.color;
        float xVel = Random.Range(-1f, 1f);
        while (t < _lifeTime)
        {
            t += Time.deltaTime;
            Vector2 moveVec = Vector2.up * (_speed * Time.deltaTime) + Vector2.right * (xVel * Time.deltaTime);
            _text.rectTransform.anchoredPosition += moveVec;

            float alpha = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / _lifeTime));
            col.a = alpha;
            _text.color = col;

            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
        transform.localScale = Vector3.one;
        if (_isReleased == true)
        {
            return;
        }
        _owner.Release(this);
        _isReleased = true;
    }


    public void ChangedScene()
    {
        if(_isReleased == true )
        {
            return;
        }
        _owner.Release(this);
        _isReleased = true;
    }
}
