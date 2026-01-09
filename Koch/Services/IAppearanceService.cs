using System;
using Microsoft.UI.Xaml;

namespace Koch.Services
{
    public enum AppTheme
    {
        Light = 0,
        Dark = 1,
        System = 2
    }

    public interface IAppearanceService
    {
        // 主题相关
        AppTheme CurrentTheme { get; set; }
        event Action<AppTheme>? ThemeChanged;

        // 透明度相关
        double CurrentOpacity { get; set; }
        event Action<double>? OpacityChanged;

        // 初始化和应用
        void Initialize();
        void ApplyAppearance(Window window);
    }
}
