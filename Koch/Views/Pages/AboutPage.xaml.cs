using Microsoft.UI.Xaml.Controls;
using Koch.ViewModels.Pages;

namespace Koch.Views.Pages
{
    public sealed partial class AboutPage : Page
    {
        public AboutPageViewModel ViewModel { get; }

        public AboutPage(AboutPageViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }
    }
}
