using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerLifePresenter : MonoBehaviour
{
    public PlayerLife PlayerLifeModel;
    public PlayerLifeView PlayerLifeView;
    public ReplayUI ReplayUI;

    private void OnEnable()
    {
        PlayerLifeModel.OnPlayerLifeChanged += OnModelUpdated;
        PlayerLifeModel.OnPlayerDie += OnPlayerDie;
    }

    private void OnDisable()
    {
        PlayerLifeModel.OnPlayerLifeChanged -= OnModelUpdated;
        PlayerLifeModel.OnPlayerDie -= OnPlayerDie;
    }

    public void OnPlayerDie()
    {
        ReplayUI.ShowReplayUI().Forget();
    }

    public void OnModelUpdated(int lifeCount)
    {
        PlayerLifeView.ViewUpdate(lifeCount);
    }
}