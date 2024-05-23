using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OllamaWpfClient.Entities;
using OllamaWpfClient.Services;

namespace OllamaWpfClient;

[ObservableObject]
public partial class MainWindowViewModel(IBot bot)
{
    // PROPERTIES ========================================
    [ObservableProperty]
    private ObservableCollection<ConversationItem> _conversationItems = new();

    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
    [ObservableProperty] private string _message = string.Empty;

    [ObservableProperty] private bool _isBotWorking;

    // COMMANDS ========================================
    [RelayCommand]
    private void Reset()
    {
        ConversationItems.Clear();
    }

    [RelayCommand(CanExecute = nameof(CanSendMessageExecute))]
    public async Task SendMessageAsync()
    {
        IsBotWorking = true;
        ConversationItems.Add(new ConversationItem(Message, ConversationSource.User));
        var response = new StringBuilder();
        await foreach (var m in bot.SendMessageAsync(Message))
        {
            response.Append(m);
        }
        ConversationItems.Add(new ConversationItem(response.ToString(), ConversationSource.Bot));
        IsBotWorking = false;
        Message = string.Empty;
    }

    private bool CanSendMessageExecute() => !_isBotWorking && !string.IsNullOrWhiteSpace(Message);
    
}