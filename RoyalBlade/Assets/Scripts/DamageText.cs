using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class DamageText : MonoBehaviour
{

    [SerializeField] private float _lifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Color _defaultColor;

    private CancellationTokenSource source = new CancellationTokenSource();

    private ObjectPool<DamageText> _owner;
    private TMP_Text _text;

    public void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetOwner(ObjectPool<DamageText> owner) => _owner = owner;

    public void SetUp(string damage, Vector2 startPosition)
    {
        _text.text = damage;

        _text.color = _defaultColor;
        _text.rectTransform.anchoredPosition = startPosition;
        _text.rectTransform.localScale = _defaultScale;
        DamageTextEffect().Forget();
    }

    private async UniTaskVoid DamageTextEffect()
    {
        float t = 0f;
        Color col = _text.color;
        float xVel = Random.Range(-1f, 1f);
        while (t < _lifeTime)
        {
            if (source.IsCancellationRequested) return;
            if (_text == null) break;
            t += Time.deltaTime;
            Vector2 moveVec = Vector2.up * (_speed * Time.deltaTime) + Vector2.right * (xVel * Time.deltaTime);
            _text.rectTransform.anchoredPosition += moveVec;

            float alpha = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / _lifeTime));
            col.a = alpha;
            _text.color = col;

            await UniTask.Yield();
        }
        transform.localScale = Vector3.one;
        _owner.Release(this);
    }

    void OnSceneLoaded(Scene s, LoadSceneMode lm)
    {
        source.Cancel();
    }

}
