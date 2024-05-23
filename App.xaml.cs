using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OllamaClient.Extensions;
using OllamaWpfClient.Services;
using OllamaWpfClient.Services.Impl;

namespace OllamaWpfClient;

public partial class App
{
    public App()
    {
        Services = ConfigureServices();
    }

    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; }


    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // Services
        services.AddOllamaClient();
        services.AddSingleton<IBot, OllamaClientBot>();

        // ViewModels
        services.AddTransient<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }

}