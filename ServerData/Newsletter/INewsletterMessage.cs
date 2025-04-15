namespace Data.Newsletter;

public abstract class INewsletterMessage
{
    public abstract int Id { get; }
    public abstract string Text { get; }
}