using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    private Image _backGround;
    private Image _lifeImage;

    private void Awake()
    {
        _backGround = transform.GetChild(0).GetComponent<Image>();
        _lifeImage = transform.GetChild(1).GetComponent<Image>();
    }

    public void ShowLifeImage()
    {
        _lifeImage.gameObject.SetActive(true);
    }

    public void HideLifeImage()
    {
        _lifeImage.gameObject.SetActive(false);
    }
}