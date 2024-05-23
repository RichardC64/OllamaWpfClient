using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp1;

[ObservableObject]
public partial class MainWindowViewModel
{
    [ObservableProperty]
    private ObservableCollection<ConversationItem> _conversationItems = new();
}

public record ConversationItem(string Text, ConversationSource Source);

public enum ConversationSource
{
    User,
    Bot
}