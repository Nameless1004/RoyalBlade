using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WallHealthViewer : MonoBehaviour
{
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _progressBarBackground;
    private Image _progressBarImage;
    private TMP_Text _hpText;

    private void Awake()
    {
        _progressBarImage = _progressBar.GetComponent<Image>();
        _hpText = GetComponentInChildren<TMP_Text>();
        _progressBar.SetActive(false);
        _progressBarBackground.SetActive(false);
    }

    public void SetTargetObject(Health targetHealth)
    {
        if(targetHealth == null)
        {
            _progressBarBackground.SetActive(false);
            _progressBar.SetActive(false);
        }
        else
        {
            _progressBarBackground.SetActive(true);
            _progressBar.SetActive(true);
            UpdataHealthBar(targetHealth.CurrentHp, targetHealth.MaxHp);
        }
    }

    public void UpdataHealthBar(int currentHp, int maxHp)
    {
        float ratio = currentHp / (float)maxHp;
        _progressBarImage.fillAmount = ratio;
        _hpText.text = $"{currentHp} / {maxHp}";
    }
}