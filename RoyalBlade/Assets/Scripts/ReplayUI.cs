using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayUI : MonoBehaviour
{
    [SerializeField] 
    private float _fadeTime;

    private Button _replayButton;
    private CanvasGroup _group;

    private void Awake()
    {
        _replayButton = GetComponentInChildren<Button>();
        _group = GetComponent<CanvasGroup>();
        _group.interactable = false;
        _group.blocksRaycasts = false;
        _group.alpha = 0f;
    }

    private void OnEnable()
    {
        _replayButton.onClick.AddListener(OnClick_Replay);
    }

    private void OnDisable()
    {
        _replayButton.onClick.RemoveListener(OnClick_Replay);
    }

    public void OnClick_Replay()
    {
        SceneManager.LoadScene(0);
    }

    public async UniTask ShowReplayUI()
    {
        Time.timeScale = 0f;
        await FadeIn();
    }

    async UniTask FadeIn()
    {
        float elapsedTime = 0;
        Color color = Color.white;
        color.a = 0f;

        _group.blocksRaycasts = true;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            _group.alpha = Mathf.Lerp(0, 1, elapsedTime / _fadeTime);
            await UniTask.Yield();
        }

        _group.interactable = true;
    }
}