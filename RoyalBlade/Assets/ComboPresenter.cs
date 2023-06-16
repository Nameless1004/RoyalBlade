using UnityEngine;

public class ComboPresenter : MonoBehaviour
{
    [SerializeField] private ComboView _view;

    private void OnEnable()
    {
        ComboCounter.OnComboChanged += UpdatedModel;
    }

    private void OnDisable()
    {
        ComboCounter.OnComboChanged -= UpdatedModel;
        ComboCounter.ResetCombo();
    }

    public void UpdatedModel(int combo)
    {
        _view.UpdateCombo(combo);
    }
}
