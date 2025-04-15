using System.Collections.ObjectModel;

namespace Data.Newsletter;

public abstract class INewsletterRepository
{
    public abstract ObservableCollection<INewsletterMessage> GetAllMessages();
    public abstract INewsletterMessage? GetMessageById(int id);
    
    public abstract void AddMessage(INewsletterMessage message);
}