using UnityEngine;
using UnityEngine.UI;

public class GuardInput : CustomInput
{
    public Image GuardCooltimeImage;

    protected override void Awake()
    {
        base.Awake();
        GuardCooltimeImage.fillAmount = 0;
    }

    public void UpdateCoolTime(float ratio)
    {
        float val = 1 - ratio;
        GuardCooltimeImage.fillAmount = val;
    }

}