using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class PlayerUlt : Skill
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private int _maxSpawnObjectCount;
    private PlayerUltObject[] _spawnObjects;

    public override void Use()
    {
        UltTask().Forget();
        CameraEffecter.Instance.PlayScreenShake(500 * _maxSpawnObjectCount, 0.5f);
    }

    async UniTaskVoid UltTask()
    {
        for (int i = 0; i < _spawnObjects.Length; ++i)
        {
            _spawnObjects[i].Damage = SkillDamage;
            int randomAngle = Random.Range(0, 360);
            float y = Mathf.Cos(randomAngle) * Mathf.Rad2Deg;
            float x = Mathf.Sin(randomAngle) * Mathf.Rad2Deg;
            Vector2 dir = new Vector2(x, y).normalized;

            Vector2 pivot = (Vector2)transform.position + (dir * 15f);
            Vector2 playerOffsetPosition = (Vector2)transform.position + Vector2.up * 3f;
            Vector2 dirToPlayer = (playerOffsetPosition - pivot).normalized;
            _spawnObjects[i].transform.position = pivot;
            _spawnObjects[i].transform.up = dirToPlayer;
            _spawnObjects[i].Spawn();
            SoundManager.Instance.EffectPlay("SpawnNiddle", Vector3.zero);
            await UniTask.Delay(500);
        }
    }

    private void Awake()
    {
        SkillDamage = int.MaxValue;
        _spawnObjects = new PlayerUltObject[_maxSpawnObjectCount];

        var prefab = ResourceCache.GetResource<PlayerUltObject>(Path.Combine("Prefabs", "PlayerUltObject"));

        for(int i = 0; i < _maxSpawnObjectCount; ++i)
        {
            _spawnObjects[i] = Instantiate(prefab, _spawnPosition);
            _spawnObjects[i].gameObject.SetActive(false);
        }
    }
}



