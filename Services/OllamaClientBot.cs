using System.Diagnostics;
using System.Text;
using OllamaClient;

namespace OllamaWpfClient.Services;

public class OllamaClientBot : IBot
{
    private readonly IOllamaHttpClient _ollamaHttpClient;

    public OllamaClientBot(IOllamaHttpClient ollamaHttpClient)
    {
        _ollamaHttpClient = ollamaHttpClient ?? throw new ArgumentNullException(nameof(ollamaHttpClient));
    }
    public async IAsyncEnumerable<string> SendMessageAsync(string message)
    {
        // Send message to Ollama API
        var result = _ollamaHttpClient.SendChat(new OllamaClient.Models.ChatStreamRequest
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

        await foreach (var m in result)
        {
            if (m.Message?.Content == null) continue;
            yield return m.Message.Content;

        }
    }
}