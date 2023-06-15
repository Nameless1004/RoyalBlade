using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();   
    }
    public void UpdateView(int score)
    {
        _text.text = score.ToString();
    }
}