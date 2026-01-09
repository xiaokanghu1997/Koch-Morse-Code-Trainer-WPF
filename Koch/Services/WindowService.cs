using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using WinRT.Interop;

namespace Koch.Services
{
    public class WindowService : IWindowService
    {
        /// <summary>
        /// 设置固定窗口大小
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="width">窗口宽度</param>
        /// <param name="height">窗口高度</param>
        public void SetFixedWindowSize(Window window, int width, int height)
        {
            IntPtr hwnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new SizeInt32(width, height));  // 设置窗口大小
            appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsMaximizable = false;  // 禁用最大化按钮
                presenter.IsResizable = false;  // 禁用窗口大小调整
            }

            var displayArea = DisplayArea.GetFromWindowId(
                windowId,
                DisplayAreaFallback.Primary);

            var windowSize = appWindow.Size;

            int x = displayArea.WorkArea.X +
                    (displayArea.WorkArea.Width - windowSize.Width) / 2;

            int y = displayArea.WorkArea.Y +
                    (displayArea.WorkArea.Height - windowSize.Height) / 2;

            appWindow.Move(new PointInt32(x, y));  // 将窗口移动到屏幕中央
        }
    }
}
