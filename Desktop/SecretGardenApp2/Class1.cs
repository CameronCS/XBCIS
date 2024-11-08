public class Email
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Timestamp { get; set; }
    public bool IsOpened { get; set; } // Add this line
    public int Id { get; set; }
}

public class MediaResource
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string FilePath { get; set; }

    public override string ToString()
    {
        return Title;
    }
}