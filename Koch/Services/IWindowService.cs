using Microsoft.UI.Xaml;

namespace Koch.Services
{
    public interface IWindowService
    {
        void SetFixedWindowSize(Window window, double logicalWidth, double logicalHeight);

        void ResizeWindow(Window window, double logicalWidth, double logicalHeight);
    }
}
