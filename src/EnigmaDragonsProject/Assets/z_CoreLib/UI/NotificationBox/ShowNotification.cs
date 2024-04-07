public class ShowNotification
{
    public ShowNotification(string title, string content)
    {
        Title = title;
        Content = content;
    }
    
    public string Title { get; }
    public string Content { get; }
}