using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NvidiaManager.Views;

public partial class MainView : UserControl
{
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MainView()
    {
        InitializeComponent();
    }
}
