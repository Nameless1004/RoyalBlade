using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ComboView : MonoBehaviour
{
    TMP_Text _text;
    [SerializeField] private float _comboFadeTime;

    private static readonly string s_comboString = "Combo";

    private Dictionary<int, Color> _comboColor = new Dictionary<int, Color>();
    private Dictionary<int, string> _caching = new Dictionary<int, string>();

    private Color _textCurrentColor;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>(true);
        _text.gameObject.SetActive(false);

        _comboColor[10] = new Color(0.86f, 0.77f, 0.66f);
        _comboColor[30] = new Color(0.64f, 0.29f, 0.87f);
        _comboColor[50] = new Color(0.13f, 0.65f, 0.96f);
    }

    public void UpdateCombo(int combo)
    {
        _text.color = combo switch
        {
            >= 0 and <= 10 => Color.white,
            > 10 and <= 30 => _comboColor[10],
            > 30 and <= 50 => _comboColor[30],
            < 50 => _comboColor[50],
            _ => Color.white
        };
        _textCurrentColor = _text.color;

        if (combo == 0)
        {
            _text.gameObject.SetActive(false);
            _text.text = string.Empty;
            return;
        }
        else
        {
            _text.gameObject.SetActive(true);
        }

        if (_caching.TryGetValue(combo, out var comboStr) == false)
        {
            comboStr = combo.ToString() + s_comboString;
            _caching[combo] = comboStr;
        }
        _text.text = comboStr;
        FadeOutComboCount(combo).Forget();
    }

    private async UniTask FadeOutComboCount(int combo)
    {
        float elapsedTime = 0;
        int currentCombo = combo;
        Color col = _textCurrentColor;

        while (elapsedTime < _comboFadeTime)
        {
            if(currentCombo != ComboCounter.ComboCount)
            {
                return;
            }

            elapsedTime += Time.deltaTime;
            col.a = Mathf.Lerp(1, 0, elapsedTime / _comboFadeTime);
            _text.color = col;
            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }
    }
}
