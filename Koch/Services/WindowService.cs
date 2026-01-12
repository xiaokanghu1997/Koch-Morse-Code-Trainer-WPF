using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.Graphics;
using WinRT.Interop;

namespace Koch.Services
{
    public class WindowService : IWindowService
    {
        // Win32 API 声明
        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        /// <summary>
        /// 设置固定窗口大小（使用逻辑像素）
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="logicalWidth">窗口逻辑宽度</param>
        /// <param name="logicalHeight">窗口逻辑高度</param>
        public void SetFixedWindowSize(Window window, double logicalWidth, double logicalHeight)
        {
            IntPtr hwnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // 获取缩放因子
            double scaleFactor = GetScaleFactor(window, hwnd);

            // 计算物理像素大小（向上取整）
            int physicalWidth = (int)Math.Ceiling(logicalWidth * scaleFactor);
            int physicalHeight = (int)Math.Ceiling(logicalHeight * scaleFactor);

            // 设置窗口大小
            appWindow.Resize(new SizeInt32(physicalWidth, physicalHeight));
            appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsMaximizable = false;  // 禁用最大化按钮
                presenter.IsResizable = false;  // 禁用窗口大小调整
            }

            // 居中窗口
            CenterWindow(appWindow, windowId);
        }

        /// <summary>
        /// 重新调整窗口大小（用于 DPI 变化时）
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="logicalWidth">窗口逻辑宽度</param>
        /// <param name="logicalHeight">窗口逻辑高度</param>
        public void ResizeWindow(Window window, double logicalWidth, double logicalHeight)
        {
            IntPtr hwnd = WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // 获取新的缩放因子
            double scaleFactor = GetScaleFactor(window, hwnd);

            // 计算新的物理像素
            int physicalWidth = (int)Math.Ceiling(logicalWidth * scaleFactor);
            int physicalHeight = (int)Math.Ceiling(logicalHeight * scaleFactor);

            // 调整窗口大小
            appWindow.Resize(new SizeInt32(physicalWidth, physicalHeight));
        }

        /// <summary>
        /// 居中窗口
        /// </summary>
        private static void CenterWindow(AppWindow appWindow, WindowId windowId)
        {
            var displayArea = DisplayArea.GetFromWindowId(
                windowId,
                DisplayAreaFallback.Primary);

            var windowSize = appWindow.Size;
            int x = displayArea.WorkArea.X +
                    (displayArea.WorkArea.Width - windowSize.Width) / 2;
            int y = displayArea.WorkArea.Y +
                    (displayArea.WorkArea.Height - windowSize.Height) / 2;

            appWindow.Move(new PointInt32(x, y));
        }

        /// <summary>
        /// 获取当前窗口的 DPI 缩放因子
        /// </summary>
        private static double GetScaleFactor(Window window, IntPtr hwnd)
        {
            // 优先使用 XamlRoot（WinUI 3 推荐）
            if (window.Content?.XamlRoot != null)
            {
                return window.Content.XamlRoot.RasterizationScale;
            }
            // 使用 Win32 API
            else
            {
                uint dpi = GetDpiForWindow(hwnd);
                return dpi / 96.0;
            }
        }
    }
}
