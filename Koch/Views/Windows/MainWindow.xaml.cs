using Koch.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Koch.Views.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(
            MainWindowViewModel viewModel,
            ISnackbarService snackbarService)
        {
            InitializeComponent();

            // 设置 ViewModel
            DataContext = viewModel;

            // 配置 Snackbar 服务
            // snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        }
    }
}