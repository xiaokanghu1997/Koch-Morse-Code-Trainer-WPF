using Koch.Services;
using Koch.Views.Pages;
using Koch.Views.Windows;
using Koch.ViewModels.Pages;
using Koch.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;


namespace Koch
{
    public partial class App : Application
    {
        private Window? _window;
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化应用程序
        /// </summary>
        public App()
        {
            InitializeComponent();
            _serviceProvider = ConfigureServices();

            // 初始化主题服务（加载保存的主题设置）
            var themeService = _serviceProvider.GetRequiredService<IThemeService>();
            themeService.Initialize();

            // 注册应用退出时释放资源
            UnhandledException += (sender, e) => DisposeServices();
        }

        /// <summary>
        /// 配置依赖注入服务
        /// </summary>
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // 注册 Services
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IThemeService, ThemeService>();

            // 注册 Page ViewModels
            services.AddTransient<SettingsPageViewModel>();

            // 注册 Window ViewModels
            services.AddTransient<MainWindowViewModel>();

            // 注册 Page Views
            services.AddTransient<SettingsPage>();

            // 注册 Window Views
            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 在应用程序启动时调用
        /// </summary>
        /// <param name="args">关于启动请求和流程的详细信息</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = _serviceProvider.GetRequiredService<MainWindow>();
            _window.Activate();
        }

        /// <summary>
        /// 释放依赖注入容器资源
        /// </summary>
        private void DisposeServices()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}