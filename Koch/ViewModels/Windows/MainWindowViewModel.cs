using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Koch.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public string ApplicationTitle { get; } = "Koch - Morse Code Trainer";

        public string ApplicationVersion { get; } = "v0.0.0";
    }
}
