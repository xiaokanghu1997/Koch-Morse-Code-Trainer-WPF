using CommunityToolkit.Mvvm.ComponentModel;
using Koch.Services;
using System.Collections.ObjectModel;


namespace Koch.ViewModels.Pages
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly IAppearanceService _appearanceService;

        public SettingsPageViewModel(IAppearanceService appearanceService)
        {
            _appearanceService = appearanceService;

            // 初始化主题选项
            ThemeOptions = ["Light", "Dark", "Follow System"];

            // 根据当前主题设置选中项
            _selectedTheme = _appearanceService.CurrentTheme switch
            {
                AppTheme.Light => "Light",
                AppTheme.Dark => "Dark",
                AppTheme.System => "Follow System",
                _ => "Follow System"
            };

            // 初始化透明度
            _windowOpacity = _appearanceService.CurrentOpacity;
        }

        #region 主题设置

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
                    _appearanceService.CurrentTheme = value switch
                    {
                        "Light" => AppTheme.Light,
                        "Dark" => AppTheme.Dark,
                        "Follow System" => AppTheme.System,
                        _ => AppTheme.System
                    };
                }
            }
        }

        #endregion

        #region 透明度设置

        // 窗口透明度（0.1 到 1.0）
        private double _windowOpacity;

        public double WindowOpacity
        {
            get => _windowOpacity;
            set
            {
                if (SetProperty(ref _windowOpacity, value))
                {
                    _appearanceService.CurrentOpacity = value;
                    OnPropertyChanged(nameof(OpacityPercentage));
                }
            }
        }

        // 透明度百分比显示
        public string OpacityPercentage => $"{(int)(WindowOpacity * 100)}%";

        #endregion
    }
}
