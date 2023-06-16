using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    public Transform ObjectPoolingTransform;
    private FxPooler _fxPooler;
    public FxPooler FxPooler { get { return _fxPooler; } }

    public int GameScore;
    public event Action<int> OnGameScoreChanged;

    private void Awake()
    {
        Application.targetFrameRate = 60;
       // SoundManager.Instance.BGMPlay("Bgm",true, 0.7f);
    }

    private void Start()
    {
        _fxPooler = new FxPooler(ObjectPoolingTransform);
        Time.timeScale = 1f;
    }

    public void AddGameScore(int addedScore)
    {
        GameScore += addedScore;
        OnGameScoreChanged?.Invoke(GameScore);
    }
}