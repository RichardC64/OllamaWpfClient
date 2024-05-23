using System.Text;
using OllamaClient;

namespace OllamaWpfClient.Services.Impl;

public class OllamaClientBot(IOllamaHttpClient ollamaHttpClient) : IBot
{
    private readonly IOllamaHttpClient _ollamaHttpClient =
        ollamaHttpClient ?? throw new ArgumentNullException(nameof(ollamaHttpClient));

    public async Task<string> SendMessageAsync(string message)
    {
        // Envoi du message à l'API de Ollama
        var result = _ollamaHttpClient.SendChat(
            new OllamaClient.Models.ChatStreamRequest
            {
                Model = "Llama3",
                Messages =
                [
                    new () { Content = "Vous êtes un AI assistant qui aide les gens à trouver des informations. Vous répondez en français.", Role = "system", },
                    new() { Content = message, Role = "user", },
                ],
            }, CancellationToken.None).ConfigureAwait(false);

        var sb = new StringBuilder();
        await foreach (var m in result)
        {
            if (m.Message?.Content == null) continue;
            sb.Append(m.Message.Content);
        }

        return sb.ToString();
    }
}