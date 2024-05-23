using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace OllamaWpfClient.Services.Impl;

public class SkBot : IAsyncBot
{
    private readonly IChatCompletionService _ai;
    private readonly Kernel _kernel;

    [Experimental("SKEXP0010")]
    public SkBot()
    {
        var kernelBuilder = Kernel.CreateBuilder();
        _kernel = kernelBuilder
            .AddOpenAIChatCompletion( 
                modelId: "Llama3",
                apiKey: null,
                endpoint: new Uri("http://localhost:11434")) 
            .Build();

        // Create a new chat
        _ai = _kernel.GetRequiredService<IChatCompletionService>();
    }

    public async IAsyncEnumerable<string> SendMessageAsync(string message)
    {
        ChatHistory chat = new("Vous êtes un AI assistant qui aide les gens à trouver des informations. Vous répondez en français.");
        chat.AddUserMessage(message);

        await foreach (var response in _ai.GetStreamingChatMessageContentsAsync(chat, kernel: _kernel).ConfigureAwait(false))
        {
            if (response.Content == null) continue;
            yield return response.Content;

        }
    }
}