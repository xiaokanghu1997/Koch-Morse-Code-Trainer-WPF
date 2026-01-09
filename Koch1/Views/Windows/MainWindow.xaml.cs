using System.Windows;
using Koch.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace Koch.Views.Windows
{
    /// <summary>
    /// 主窗口交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            // 设置 ViewModel
            ViewModel = viewModel;
            DataContext = this;
        }

        /// <summary>
        /// 引发已结束的事件
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // 确保关闭此窗口将开始关闭应用程序的过程
            Application.Current.Shutdown();
        }
    }
}