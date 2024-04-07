using System;
using UnityEngine;

public class ShowNotificationOnEnable : MonoBehaviour
{
    public void OnEnable()
    {
        Message.Publish(new ShowNotification("a sweet title", "some content"));
    }
}