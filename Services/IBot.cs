namespace OllamaWpfClient.Services;

public interface IBot
{
    Task<string> SendMessageAsync(string message);
}