namespace Data.Newsletter;

internal class NewsletterMessage : INewsletterMessage
{
    public override int Id { get; }
    public override string Text { get; }

    public NewsletterMessage(int id, string text)
    {
        Id = id;
        Text = text;
    }
}