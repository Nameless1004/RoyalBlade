using System;

public static class ComboCounter
{
    private static int _comboCount;
    public static int ComboCount { get { return _comboCount; } }
    public static event Action<int> OnComboChanged;

    public static void AddCombo()
    {
        ++_comboCount;
        OnComboChanged?.Invoke(_comboCount);
    }

    public static void ResetCombo()
    {
        _comboCount = 0;
        OnComboChanged?.Invoke(_comboCount);
    }
}