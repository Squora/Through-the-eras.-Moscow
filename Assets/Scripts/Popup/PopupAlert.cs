using UnityEngine;

public class PopupAlert : MonoBehaviour
{
    public string Title;
    public string Message;
    public float Duration;

    public PopupAlert(string title, string message, float duration)
    {
        Title = title;
        Message = message;
        Duration = duration;
    }
}
