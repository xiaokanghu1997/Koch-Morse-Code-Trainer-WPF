using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Koch.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private string _title = "我的 WPF UI 应用";
        private string _welcomeMessage = "欢迎使用 WPF UI + MVVM! ";
        private int _clickCount = 0;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        public int ClickCount
        {
            get => _clickCount;
            set => SetProperty(ref _clickCount, value);
        }

        // 使用 CommunityToolkit.Mvvm 的 RelayCommand（更简洁）
        public ICommand ClickCommand { get; }
        public ICommand ShowNotificationCommand { get; }

        public MainWindowViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;

            ClickCommand = new RelayCommand(OnButtonClick);
            ShowNotificationCommand = new RelayCommand(OnShowNotification);
        }

        private void OnButtonClick()
        {
            ClickCount++;
            WelcomeMessage = $"你已经点击了 {ClickCount} 次按钮！";
        }

        private void OnShowNotification()
        {
            _snackbarService.Show(
                "通知",
                $"这是一条通知消息！当前点击次数: {ClickCount}",
                ControlAppearance.Success,
                null,
                TimeSpan.FromSeconds(3)
            );
        }
    }
}