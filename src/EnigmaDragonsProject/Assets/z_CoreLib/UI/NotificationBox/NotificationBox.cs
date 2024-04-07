using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private Button close;

    public void Awake() => close.onClick.AddListener(() => gameObject.SetActive(false));
    
    public void Popup(string title, string content)
    {
        this.title.text = title;
        this.content.text = content;
        gameObject.SetActive(true);
    }
}