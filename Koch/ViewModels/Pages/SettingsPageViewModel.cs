using CommunityToolkit.Mvvm.ComponentModel;
using Koch.Services;
using System.Collections.ObjectModel;


namespace Koch.ViewModels.Pages
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly IThemeService _themeService;

        public SettingsPageViewModel(IThemeService themeService)
        {
            _themeService = themeService;

            // 初始化主题选项
            ThemeOptions = ["Light", "Dark", "Follow System"];

            // 根据当前主题设置选中项
            _selectedTheme = _themeService.CurrentTheme switch
            {
                AppTheme.Light => "Light",
                AppTheme.Dark => "Dark",
                AppTheme.System => "Follow System",
                _ => "Follow System"
            };
        }

        // 主题选项列表
        public ObservableCollection<string> ThemeOptions { get; }

        // 选中的主题
        private string _selectedTheme;

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    _themeService.CurrentTheme = value switch
                    {
                        "Light" => AppTheme.Light,
                        "Dark" => AppTheme.Dark,
                        "Follow System" => AppTheme.System,
                        _ => AppTheme.System
                    };
                }
            }
        }
    }
}
