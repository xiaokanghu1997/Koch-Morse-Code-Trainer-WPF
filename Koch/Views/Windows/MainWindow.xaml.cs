using Koch.Services;
using Koch.ViewModels.Windows;
using Koch.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Koch.Views.Windows
{
    /// <summary>
    /// 主窗口交互逻辑
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly IThemeService _themeService;

        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel, 
            IWindowService windowService,
            IThemeService themeService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ViewModel = viewModel;
            _themeService = themeService;
            _serviceProvider = serviceProvider;

            // 订阅主题服务事件
            _themeService.ThemeChanged += OnThemeChanged;

            // 隐藏系统默认标题栏
            ExtendsContentIntoTitleBar = true;
            // 用WinUI标题栏替换系统标题栏
            SetTitleBar(AppTitleBar);

            // 设置窗口大小
            windowService.SetFixedWindowSize(this, 1200, 530);

            // 应用初始主题
            _themeService.ApplyTheme(this);

            // 默认导航栏到练习页面
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
        }

        private void OnThemeChanged(AppTheme _)
        {
            _themeService.ApplyTheme(this);
        }

        private void NavigationView_SelectionChanged(NavigationView _, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();

                if (tag == null) return;

                Page? page = tag switch
                {
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
