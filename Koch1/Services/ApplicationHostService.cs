using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Koch.Views.Windows;

namespace Koch.Services
{
    /// <summary>
    /// 应用程序的托管主机
    /// </summary>
    public class ApplicationHostService: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 当应用程序主机准备启动服务时触发
        /// </summary>
        /// <param name="cancellationToken">表示启动过程已中止</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// 当应用程序主机执行正常关机时触发
        /// </summary>
        /// <param name="cancellationToken">表示关闭过程将不再采用平滑方式进行</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 在活动期间创建主窗口
        /// </summary>
        private async Task HandleActivationAsync()
        {
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }

            await Task.CompletedTask;
        }
    }
}
