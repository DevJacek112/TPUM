using System.Collections.ObjectModel;

namespace Data.Newsletter;

internal class NewsletterRepository : INewsletterRepository
{
    public NewsletterRepository()
    {
        AddMessage(new NewsletterMessage(1, "1. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."));
        AddMessage(new NewsletterMessage(2, "2. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."));
        AddMessage(new NewsletterMessage(3, "3. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."));
        AddMessage(new NewsletterMessage(4, "4. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
    }
    
    private ObservableCollection<INewsletterMessage> messages = new();
    
    public override ObservableCollection<INewsletterMessage> GetAllMessages()
    {
        return messages;
    }

    public override INewsletterMessage? GetMessageById(int id)
    {
        return messages.FirstOrDefault(b => b.Id == id);
    }

    public override void AddMessage(INewsletterMessage message)
    {
        messages.Add(message);
    }
}