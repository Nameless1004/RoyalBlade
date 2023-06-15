using UnityEngine;

public class PlayerLifeView : MonoBehaviour
{
    private LifeUI[] _life;

    private void Awake()
    {
        _life = GetComponentsInChildren<LifeUI>();
    }

    public void ViewUpdate(int lifeCount)
    {
        for(int i = 0; i < _life.Length; i++)
        {
            if(i < lifeCount)
            {
                _life[i].ShowLifeImage();
            }
            else
            {
                _life[i].HideLifeImage();
            }
        }
    }
}