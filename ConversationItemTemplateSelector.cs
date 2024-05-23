using System.Windows;
using System.Windows.Controls;
using OllamaWpfClient.Entities;

namespace OllamaWpfClient;

public class ConversationItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? User { get; set; }
    public DataTemplate? Bot { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is not ConversationItem conversationItem)
            throw new ArgumentException("Item is not a ConversationItem");

        return conversationItem.Source switch
        {
            ConversationSource.Bot => Bot,
            ConversationSource.User => User,
            _ => throw new ArgumentException("Invalid ConversationSource")
        };
    }
}