using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OllamaWpfClient.Entities;
using OllamaWpfClient.Services;

namespace OllamaWpfClient;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IAsyncBot? _asyncBot;
    private readonly IBot? _bot;
    private Action? _updateScrollViewer;

    // CONTRUCTORS ========================================
    public MainWindowViewModel(){}

    public MainWindowViewModel(IAsyncBot asyncBot, IBot bot)
    {
        _asyncBot = asyncBot;
        _bot = bot;
    }

    public MainWindowViewModel Initialize(Action updateScrollViewer)
    {
        _updateScrollViewer = updateScrollViewer;
        return this;
    }

    // PROPERTIES ========================================
    /// <summary>
    /// La liste des messages de la conversation
    /// </summary>
    [ObservableProperty] private ObservableCollection<ConversationItem> _conversationItems = [];

    /// <summary>
    /// Le message à envoyer
    /// </summary>
    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))] [ObservableProperty]
    private string _message = string.Empty;

    /// <summary>
    /// L'IA est-elle en train de travailler ?
    /// </summary>
    [ObservableProperty] private bool _isBotWorking;
    /// <summary>
    /// Bot de type Semantic Kernel ?
    /// </summary>
    [ObservableProperty] private bool? _isSkSelected = false;
    

    // COMMANDS ========================================
    /// <summary>
    /// Vide la conversation
    /// </summary>
    [RelayCommand]
    private void Reset()
    {
        ConversationItems.Clear();
        _updateScrollViewer?.Invoke();
    }

    /// <summary>
    /// Envoi le message à l'IA
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSendMessageExecute))]
    public async Task SendMessageAsync()
    {
        IsBotWorking = true;

        // Ajout du message de l'utilisateur
        ConversationItems.Add(new ConversationItem(ConversationSource.User, Message));

        // Récupération de la réponse de l'IA
        if (IsSkSelected == true)
        {
            await SendSk().ConfigureAwait(true);
        }
        else
        {
            await SendOllamaClient().ConfigureAwait(true);
        }

        IsBotWorking = false;
        Message = string.Empty;
    }

    private bool CanSendMessageExecute() => !IsBotWorking && !string.IsNullOrWhiteSpace(Message) && _asyncBot != null;

    private async Task SendSk()
    {
        if (_asyncBot == null) throw new InvalidOperationException("Bot is not initialized");

        // Récupération de la réponse de l'IA
        await foreach (var response in _asyncBot.SendMessageAsync(Message).ConfigureAwait(true))
        {
            // Ajout de la réponse de l'IA
            if (!ConversationItems.Any() || ConversationItems[^1].Source != ConversationSource.Bot)
            {
                ConversationItems.Add(new ConversationItem(ConversationSource.Bot, response));
            }
            else
            {
                ConversationItems[^1].Add(response);
            }

            _updateScrollViewer?.Invoke();
        }
    }

    private async Task SendOllamaClient()
    {
        if (_bot == null) throw new InvalidOperationException("Bot is not initialized");

        var response = await _bot.SendMessageAsync(Message).ConfigureAwait(true);
        ConversationItems.Add(new ConversationItem(ConversationSource.Bot, response));
        _updateScrollViewer?.Invoke();
    }
}