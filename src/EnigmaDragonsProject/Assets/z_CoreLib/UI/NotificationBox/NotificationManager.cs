using UnityEngine;

public class NotificationManager : OnMessage<ShowNotification>
{
    [SerializeField] private NotificationBox notificationPrefab;

    private NotificationBox instance;
    
    public void Awake()
    {
        instance = Instantiate(notificationPrefab, transform);
        notificationPrefab.gameObject.SetActive(false);
    }

    protected override void Execute(ShowNotification msg)
    {
        instance.Popup(msg.Title, msg.Content);
    }
}