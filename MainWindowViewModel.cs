using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OllamaWpfClient.Entities;
using OllamaWpfClient.Services;

namespace OllamaWpfClient;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IBot? _bot;
    // CONTRUCTORS ========================================
    public MainWindowViewModel()
    {}

    public MainWindowViewModel(IBot bot)
    {
        _bot = bot;
    }

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
        if (_bot == null) throw new InvalidOperationException("Bot is not initialized");

        IsBotWorking = true;
        // Ajout du message de l'utilisateur
        ConversationItems.Add(new ConversationItem(Message, ConversationSource.User));
        var response = new StringBuilder();
        await foreach (var m in _bot.SendMessageAsync(Message).ConfigureAwait(true))
        {
            response.Append(m);
        }
        ConversationItems.Add(new ConversationItem(response.ToString(), ConversationSource.Bot));
        IsBotWorking = false;
        Message = string.Empty;
    }

    private bool CanSendMessageExecute() => !IsBotWorking && !string.IsNullOrWhiteSpace(Message) && _bot != null;
}