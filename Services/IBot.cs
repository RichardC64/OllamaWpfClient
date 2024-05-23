namespace OllamaWpfClient.Services;

public interface IBot
{
    IAsyncEnumerable<string> SendMessageAsync(string message);
}