using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class TextPooler : Singleton<TextPooler>
{
    [SerializeField] private GameObject _origin;
    private ObjectPool<DamageText> _pool;
    [SerializeField] private Canvas _canvas;

    private Dictionary<int, string> _damageIntToStrings;
    private Dictionary<string, int> _damageStringToInt;
    public Dictionary<string ,int> DamageStringToInt { get { return _damageStringToInt; } }
    protected override void Awake()
    {
        base.Awake();
        _origin = ResourceCache.GetResource<GameObject>(Path.Combine("Prefabs","DefaultDamageText"));
        _pool = new ObjectPool<DamageText>(CreateAction, GetAction, ReleaseAction, DestroyAction, true, 10, 100);
        _damageIntToStrings = new Dictionary<int, string>();
        _damageStringToInt = new Dictionary<string, int>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode lm)
    {
        _origin = ResourceCache.GetResource<GameObject>(Path.Combine("Prefabs", "DefaultDamageText"));
        foreach(Transform child in _canvas.transform)
        {
            child.gameObject.GetComponent<DamageText>().ChangedScene();
        }
    }

    public void PopupDamage(int damage, Vector2 startPosition)
    {
        DamageText damageText = _pool.Get();
        damageText.SetUp(GetDamageString(damage), startPosition);
    }

    private string GetDamageString(int damage)
    {
        if (_damageIntToStrings.TryGetValue(damage, out string str) == false)
        {
            if(damage == int.MaxValue)
            {
                str = "Max";
            }
            else
            {
                str = damage.ToString();
            }

            _damageStringToInt[str] = damage;
            return str;
        }

        return str;
    }


    public DamageText CreateAction()
    {
        var pooledObj = Instantiate(_origin, _canvas.transform).GetComponent<DamageText>();
        pooledObj.SetOwner(this, _pool);
        pooledObj.gameObject.SetActive(false);
        return pooledObj;
    }

    public void GetAction (DamageText text)
    {
        text.gameObject.SetActive(true);
    }

    public void ReleaseAction(DamageText text)
    {
        text.gameObject.SetActive(false);
    }

    public void DestroyAction(DamageText text)
    {
        Destroy(text.gameObject);
    }
}
