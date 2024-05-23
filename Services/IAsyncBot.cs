namespace OllamaWpfClient.Services;

public interface IAsyncBot
{
    IAsyncEnumerable<string> SendMessageAsync(string message);
}