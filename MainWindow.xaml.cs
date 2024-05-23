using Microsoft.Extensions.DependencyInjection;

namespace OllamaWpfClient;


public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext =  App.Current.Services.GetService<MainWindowViewModel>();
    }
}