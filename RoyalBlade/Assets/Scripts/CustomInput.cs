using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected Image _buttonImage;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _pressedColor;

    public bool IsButtonPressed;

    public event Action OnButtonUp;
    public event Action OnButtonPressed;
    public event Action OnButtonDown;

    protected virtual void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _buttonImage.color = _defaultColor;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        IsButtonPressed = true;
        _buttonImage.color = _pressedColor;
        OnButtonDown?.Invoke();
    }

    private void Update()
    {
        if(IsButtonPressed)
        {
            OnButtonPressed?.Invoke();
        }
    }
    //public virtual void OnPointerExit(PointerEventData eventData)
    //{
    //    IsButtonDown = false;
    //    IsButtonUp = false;
    //}

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        IsButtonPressed = false;
        _buttonImage.color = _defaultColor;
        OnButtonUp?.Invoke();
    }

}
