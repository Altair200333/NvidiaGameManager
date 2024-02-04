using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NvidiaManager.Views
{
    public partial class AddConfigDialog : Window
    {
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public AddConfigDialog()
        {
            InitializeComponent();
        }
    }
}
