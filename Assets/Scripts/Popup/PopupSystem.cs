using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private GameObject _popupAlert;
    [SerializeField] private TMP_Text _popupText;
    [SerializeField] private TMP_Text _popupTitle;
    [SerializeField] private Queue<PopupAlert> _popupAlertQueue = new Queue<PopupAlert>();

    [SerializeField] private bool _alertIsShowing = false;
    [SerializeField] private float _delayBetweenAlerts = 2f;

    void Start()
    {
        _popupAlert.SetActive(false);
    }

    void Update()
    {
        ShowNextNotification();
    }

    public void AddPopupAlert(string title, string message, float duration)
    {
        PopupAlert popupAlert = new PopupAlert(title, message, duration);
        _popupAlertQueue.Enqueue(popupAlert);
    }

    public void ShowNextNotification()
    {
        if (_popupAlertQueue.Count > 0 && _alertIsShowing == false)
        {
            _popupAlert.SetActive(true);
            PopupAlert popupAlert = _popupAlertQueue.Dequeue();
            _popupText.text = popupAlert.Message;
            _popupTitle.text = popupAlert.Title;
            _alertIsShowing = true;
            StartCoroutine(HideNotificationAfterDelay(popupAlert.Duration));
        }
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _popupAlert.SetActive(false);
        ShowNextNotification();
        yield return new WaitForSeconds(_delayBetweenAlerts);
        _alertIsShowing = false;
    }
}
