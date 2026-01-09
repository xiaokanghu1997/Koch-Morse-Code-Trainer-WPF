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

    public interface IThemeService
    {
        AppTheme CurrentTheme { get; set; }

        void Initialize();

        void ApplyTheme(Window window);

        event Action<AppTheme>? ThemeChanged;
    }
}
