using Microsoft.UI.Xaml.Controls;
using Koch.ViewModels.Pages;

namespace Koch.Views.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPageViewModel ViewModel { get; }

        public SettingsPage(SettingsPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }
    }
}
