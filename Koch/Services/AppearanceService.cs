using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using System;
using Windows.UI;
using WinRT.Interop;
using System.Runtime.InteropServices;

namespace Koch.Services
{
    public class AppearanceService(ISettingsService settingsService) : IAppearanceService
    {
        private AppTheme _currentTheme = AppTheme.System;
        private double _currentOpacity = 1.0;

        private const string ThemeSettingKey = "AppTheme";

        #region Win32 API 声明

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const uint LWA_ALPHA = 0x2;

        #endregion

        #region 主题相关

        public AppTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    // 保存到注册表
                    settingsService.SetValue(ThemeSettingKey, (int)value);
                    ThemeChanged?.Invoke(value);
                }
            }
        }

        public event Action<AppTheme>? ThemeChanged;

        #endregion

        #region 透明度相关

        public double CurrentOpacity
        {
            get => _currentOpacity;
            set
            {
                // 限制范围在 0.1 到 1.0 之间
                value = Math.Clamp(value, 0.1, 1.0);
                if (Math.Abs(_currentOpacity - value) > 0.001)
                {
                    _currentOpacity = value;
                    OpacityChanged?.Invoke(value);
                }
            }
        }

        public event Action<double>? OpacityChanged;

        #endregion

        /// <summary>
        /// 初始化外观设置，从注册表加载主题和透明度
        /// </summary>
        public void Initialize()
        {
            // 加载主题，初始默认为 System
            var savedTheme = settingsService.GetValue(ThemeSettingKey, (int)AppTheme.System);
            _currentTheme = (AppTheme)savedTheme;
            // 不加载透明度，使用默认值 1.0
        }

        /// <summary>
        /// 应用外观设置到窗口（包括主题和透明度）
        /// </summary>
        /// <param name="window">目标窗口</param>
        public void ApplyAppearance(Window window)
        {
            ApplyTheme(window);
            ApplyOpacity(window);
        }

        /// <summary>
        /// 应用窗口主题
        /// </summary>
        /// <param name="window">目标窗口</param>
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

        /// <summary>
        /// 应用窗口透明度
        /// </summary>
        /// <param name="window">目标窗口</param>
        private void ApplyOpacity(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);

            // 获取当前窗口样式
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

            // 添加 WS_EX_LAYERED 样式（支持透明度）
            if ((exStyle & WS_EX_LAYERED) == 0)
            {
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
            }

            // 设置透明度（0 - 255，255 为完全不透明）
            byte alpha = (byte)(CurrentOpacity * 255);
            SetLayeredWindowAttributes(hWnd, 0, alpha, LWA_ALPHA);
        }
    }
}
