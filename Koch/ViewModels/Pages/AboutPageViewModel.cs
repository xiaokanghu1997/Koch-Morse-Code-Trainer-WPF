using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Koch.ViewModels.Pages
{
    public partial class AboutPageViewModel : ObservableObject
    {
        public string ApplicationTitle { get; } = "Koch - Morse Code Trainer";

        public string ApplicationVersion { get; } = "Version 0.0.0";
    }
}
