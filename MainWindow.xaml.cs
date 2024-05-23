using Microsoft.Extensions.DependencyInjection;

namespace OllamaWpfClient;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<MainWindowViewModel>()?.Initialize(UpdateScrollViewer) ??
                      throw new InvalidOperationException("Impossible to find ViewModel");
    }

    public void UpdateScrollViewer()
    {
        ScrollViewer.UpdateLayout();
        ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ScrollableHeight);
    }
}
