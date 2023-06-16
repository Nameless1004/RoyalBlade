using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class WallStackPooler : MonoBehaviour
{
    [SerializeField] private WallStack Prefab;
    [SerializeField] private float spawnTimeInterval;
    [SerializeField] private Vector2 spawnPosition;
    private ObjectPool<WallStack> _pool;
    private CancellationTokenSource _canelToken = new CancellationTokenSource();

    private void Awake()
    {
        _pool = new ObjectPool<WallStack>(CreateFunc, ActionOnGet, ActionOnRelease, null, true, 10, 100);
        CreateWallStack().Forget();
    }

    private void OnDestroy()
    {
        _canelToken.Cancel();
        _pool.Clear();
        _pool.Dispose();
    }

    async UniTaskVoid CreateWallStack()
    {
        float elapsedTime = 0f;

        SpawnWallStack();

        while (true)
        {
            if (_canelToken.IsCancellationRequested) break;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > spawnTimeInterval)
            {
                elapsedTime = 0f;
                SpawnWallStack();
            }
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
    }

    public void SpawnWallStack()
    {
        var WallStack = _pool.Get();
        WallStack.Owner = _pool;
        WallStack.transform.position = spawnPosition;
    }

    public WallStack CreateFunc()
    {
        var obj = Instantiate(Prefab, spawnPosition, Quaternion.identity, transform);
        return obj;
    }
    
    public void ActionOnGet(WallStack ws)
    {
        ws.gameObject.SetActive(true);
    }

    public void ActionOnRelease(WallStack ws)
    {
        ws.gameObject.SetActive(false);
    }
}
