using Koch.Services;
using Koch.ViewModels.Windows;
using Koch.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Koch.Views.Windows
{
    public sealed partial class MainWindow : Window
    {
        private readonly IAppearanceService _appearanceService;

        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel, 
            IWindowService windowService,
            IAppearanceService appearanceService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ViewModel = viewModel;
            _appearanceService = appearanceService;
            _serviceProvider = serviceProvider;

            // 订阅主题服务事件
            _appearanceService.ThemeChanged += OnAppearanceChanged;
            _appearanceService.OpacityChanged += OnAppearanceChanged;

            // 隐藏系统默认标题栏
            ExtendsContentIntoTitleBar = true;
            // 用WinUI标题栏替换系统标题栏
            SetTitleBar(AppTitleBar);

            // 设置窗口大小（1200，530）
            windowService.SetFixedWindowSize(this, 1500, 710);

            // 应用外观
            _appearanceService.ApplyAppearance(this);

            // 默认导航栏到练习页面
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
        }

        private void OnAppearanceChanged(AppTheme _)
        {
            _appearanceService.ApplyAppearance(this);
        }

        private void OnAppearanceChanged(double _)
        {
            _appearanceService.ApplyAppearance(this);
        }

        private void NavigationView_SelectionChanged(NavigationView _, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();

                if (tag == null) return;

                Page? page = tag switch
                {
                    "About" => _serviceProvider.GetRequiredService<AboutPage>(),
                    "Settings" => _serviceProvider.GetRequiredService<SettingsPage>(),
                    _ => null
                };

                if (page != null)
                {
                    ContentFrame.Content = page;
                }
            }
        }
    }
}
