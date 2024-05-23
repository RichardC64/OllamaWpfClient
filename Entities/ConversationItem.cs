using CommunityToolkit.Mvvm.ComponentModel;

namespace OllamaWpfClient.Entities;


public class ConversationItem(ConversationSource source, string text = "") : ObservableObject
{
    public string Text { get; private set; } = text;

    public ConversationSource Source { get; private set; } = source;

    public string Add(string text)
    {
        Text += text;
        OnPropertyChanged(nameof(Text));
        return Text;
    }
}