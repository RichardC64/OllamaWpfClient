using OllamaClient;

namespace OllamaWpfClient.Services.Impl;

public class OllamaClientBot : IBot
{
    private readonly IOllamaHttpClient _ollamaHttpClient;

    public OllamaClientBot(IOllamaHttpClient ollamaHttpClient)
    {
        _ollamaHttpClient = ollamaHttpClient ?? throw new ArgumentNullException(nameof(ollamaHttpClient));
    }
    public async IAsyncEnumerable<string> SendMessageAsync(string message)
    {
        // Envoi du message à l'API de Ollama
        var result = _ollamaHttpClient.SendChat(
            new OllamaClient.Models.ChatStreamRequest
            {
                Model = "Llama3",
                Messages =
                [
                    new()
                    {
                        Content = message,
                        Role = "user"
                    }
                ]
            }, CancellationToken.None);

        await foreach (var m in result.ConfigureAwait(false))
        {
            if (m.Message?.Content == null) continue;
            yield return m.Message.Content;
        }
    }
}