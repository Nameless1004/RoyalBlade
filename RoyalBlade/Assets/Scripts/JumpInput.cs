using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpInput : CustomInput
{
    public Image GuageImage { get; set; }
    public int MaxSuccessJumpCount;
    private int CurrentSuccessJumpCount;

    public bool IsGuageFull { get; set; }

    public event Action OnUseUlt;

    protected override void Awake()
    {
        base.Awake();
        GuageImage = transform.GetChild(0).GetComponent<Image>();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if(IsGuageFull)
        {
            OnUseUlt?.Invoke();
            UseJumpUltSkill();
        }
    }

    public void SuccessJump()
    {
        CurrentSuccessJumpCount = Math.Min(CurrentSuccessJumpCount + 1, MaxSuccessJumpCount);
        float ratio = CurrentSuccessJumpCount / (float)MaxSuccessJumpCount;
        GuageImage.fillAmount = ratio;
        if(CurrentSuccessJumpCount == MaxSuccessJumpCount) IsGuageFull = true;
    }

    public void UseJumpUltSkill()
    {
        CurrentSuccessJumpCount = 0;
        GuageImage.fillAmount = 0;
        IsGuageFull = false;
    }

}