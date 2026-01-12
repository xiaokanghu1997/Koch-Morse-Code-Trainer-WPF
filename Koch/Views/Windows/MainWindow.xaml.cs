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
        private readonly IWindowService _windowService;
        private readonly IAppearanceService _appearanceService;
        private readonly IServiceProvider _serviceProvider;

        private const double LogicalWidth = 800.0;
        private const double LogicalHeight = 353.3;

        private double _currentScaleFactor = 1.0;

        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel, 
            IWindowService windowService,
            IAppearanceService appearanceService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            ViewModel = viewModel;
            _windowService = windowService;
            _appearanceService = appearanceService;
            _serviceProvider = serviceProvider;

            // 订阅主题服务事件
            _appearanceService.ThemeChanged += OnAppearanceChanged;
            _appearanceService.OpacityChanged += OnAppearanceChanged;

            // 隐藏系统默认标题栏
            ExtendsContentIntoTitleBar = true;
            // 用WinUI标题栏替换系统标题栏
            SetTitleBar(AppTitleBar);

            // 设置窗口大小（1200，530）(1500，710)
            _windowService.SetFixedWindowSize(this, LogicalWidth, LogicalHeight);

            // 应用外观
            _appearanceService.ApplyAppearance(this);

            // 默认导航栏到练习页面
            NavigationView.SelectedItem = NavigationView.MenuItems[0];

            // 订阅 DPI 变化事件
            if (Content is FrameworkElement rootElement)
            {
                rootElement.Loaded += OnRootElementLoaded;
            }
        }

        /// <summary>
        /// 根元素加载完成后订阅 DPI 变化
        /// </summary>
        private void OnRootElementLoaded(object sender, RoutedEventArgs e)
        {
            if (Content?.XamlRoot != null)
            {
                // 记录初始缩放因子
                _currentScaleFactor = Content.XamlRoot.RasterizationScale;

                // 订阅 XamlRoot 的 Changed 事件
                Content.XamlRoot.Changed += OnXamlRootChanged;
            }
        }

        /// <summary>
        /// 处理 DPI 变化（跨显示器移动或缩放改变）
        /// </summary>
        private void OnXamlRootChanged(XamlRoot sender, XamlRootChangedEventArgs args)
        {
            // 获取新的缩放因子
            double newScaleFactor = sender.RasterizationScale;

            // 只有缩放因子变化时才调整窗口
            if (Math.Abs(newScaleFactor - _currentScaleFactor) > 0.01)
            {
                // 更新缩放因子
                _currentScaleFactor = newScaleFactor;

                // 调整窗口大小并应用外观
                _windowService.ResizeWindow(this, LogicalWidth, LogicalHeight);
                _appearanceService.ApplyAppearance(this);
            }
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
                    "Practice" => _serviceProvider.GetRequiredService<PracticePage>(),
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
