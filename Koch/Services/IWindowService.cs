using Microsoft.UI.Xaml;

namespace Koch.Services
{
    public interface IWindowService
    {
        void SetFixedWindowSize(Window window, int width, int height);
    }
}
