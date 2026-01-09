using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using System;
using Windows.UI;
using WinRT.Interop;

namespace Koch.Services
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingsService _settingsService;

        private AppTheme _currentTheme = AppTheme.System;

        private const string ThemeSettingKey = "AppTheme";

        public ThemeService(ISettingsService settingsService)
        {
            _settingsService = settingsService; 
        }

        public AppTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;

                    // 保存到注册表
                    _settingsService.SetValue(ThemeSettingKey, (int)value);

                    ThemeChanged?.Invoke(value);
                }
            }
        }

        public event Action<AppTheme>? ThemeChanged;

        /// <summary>
        /// 初始化主题，从注册表加载主题
        /// </summary>
        public void Initialize()
        {
            // 从注册表加载主题，初始默认为 System
            var savedTheme = _settingsService.GetValue(ThemeSettingKey, (int)AppTheme.System);
            _currentTheme = (AppTheme)savedTheme;
        }

        /// <summary>
        /// 应用窗口主题
        /// </summary>
        /// <param name="window">窗口</param>
        public void ApplyTheme(Window window)
        {
            ElementTheme theme = CurrentTheme switch
            {
                AppTheme.Light => ElementTheme.Light,
                AppTheme.Dark => ElementTheme.Dark,
                AppTheme.System => ElementTheme.Default,
                _ => ElementTheme.Default
            };

            // 应用主题到根元素
            if (window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }

            // 获取实际主题
            bool isDark = theme == ElementTheme.Dark || 
                (theme == ElementTheme.Default && 
                Application.Current.RequestedTheme == ApplicationTheme.Dark);

            // 更新标题栏按钮颜色
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            if (appWindow != null)
            {
                var titleBar = appWindow.TitleBar;

                if (isDark)
                {
                    // 深色主题 - 统一激活和非激活状态
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonHoverForegroundColor = Colors.White;
                    titleBar.ButtonPressedForegroundColor = Colors.White;
                    titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(20, 255, 255, 255);
                    titleBar.ButtonPressedBackgroundColor = Color.FromArgb(30, 255, 255, 255);
                }
                else
                {
                    // 浅色主题 - 统一激活和非激活状态
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ButtonHoverForegroundColor = Colors.Black;
                    titleBar.ButtonPressedForegroundColor = Colors.Black;
                    titleBar.ButtonInactiveForegroundColor = Colors.Gray;

                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(20, 0, 0, 0);
                    titleBar.ButtonPressedBackgroundColor = Color.FromArgb(30, 0, 0, 0);
                }
            }
        }
    }
}
