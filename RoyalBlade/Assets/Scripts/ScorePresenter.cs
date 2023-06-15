using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    private GameManager _gameManager;
    private ScoreView _view;

    private void Awake()
    {
        _view = GetComponentInChildren<ScoreView>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameScoreChanged += ModelChanged;
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnGameScoreChanged -= ModelChanged;
        }
    }

    private void ModelChanged(int score)
    {
        _view.UpdateView(score);
    }

}