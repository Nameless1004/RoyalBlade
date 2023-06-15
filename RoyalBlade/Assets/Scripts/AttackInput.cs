using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackInput : CustomInput
{
    [SerializeField] private Image _attackUltBackground;
    [SerializeField] private Button _ultButton;
    [SerializeField] private Image _ultButtonImage;
    public Image GuageImage { get; set; }
    public int MaxSuccessAttackCount;
    private int CurrentSuccessAttackCount;
    public bool IsGuageFull { get; set; }

    public event Action OnUseUlt;

    protected override void Awake()
    {
        base.Awake();
        GuageImage = transform.GetChild(0).GetComponent<Image>();
        _attackUltBackground.gameObject.SetActive(false);
        _ultButton.interactable = false;
        _ultButtonImage.fillAmount = 0;
    }

    public void SuccessAttack()
    {
        CurrentSuccessAttackCount = Math.Min(CurrentSuccessAttackCount + 1, MaxSuccessAttackCount);
        float ratio = CurrentSuccessAttackCount / (float)MaxSuccessAttackCount;
        _ultButtonImage.fillAmount = ratio;

        if (CurrentSuccessAttackCount == MaxSuccessAttackCount)
        {
            _ultButton.interactable = true;
            IsGuageFull = true;
        }
    }

    public void UseAttakUltSkill()
    {
        OnUseUlt?.Invoke();
        FadeInUltBackGround().Forget();
        GuageImage.fillAmount = 0;
        CurrentSuccessAttackCount = 0;
        IsGuageFull = false;
        _ultButton.interactable = false;
        _ultButtonImage.fillAmount = 0;
    }

    public async UniTaskVoid FadeInUltBackGround()
    {
        float elapsedTime = 0f;
        _attackUltBackground.gameObject.SetActive(true);
        Color color = Color.black;
        color.a = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / 1f);
            _attackUltBackground.color = color;
            await UniTask.Yield();
        }
        await UniTask.Delay(4000);
        FadeOutUltBackGround().Forget();
    }

    async UniTaskVoid FadeOutUltBackGround()
    {
        float elapsedTime = 0f;
        Color color = Color.black;
        color.a = 1f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / 1f);
            _attackUltBackground.color = color;
            await UniTask.Yield();
        }
        _attackUltBackground.gameObject.SetActive(false);
    }
}