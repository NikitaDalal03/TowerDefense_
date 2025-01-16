using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    Canvas canvas;
    public GameScreens screen;

    protected void Awake()
    {
        canvas = GetComponent<Canvas>();

    }

    public virtual void ActivateScreen()
    {
        canvas.enabled = true;
    }

    public virtual void DeActivateScreen()
    {
        canvas.enabled = false;
    }

    public virtual void TakeInput()
    {

    }
}