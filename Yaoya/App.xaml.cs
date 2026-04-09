using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Yaoya.Contracts.Services;
using Yaoya.Contracts.Views;
using Yaoya.Core.Contracts.Services;
using Yaoya.Core.Services;
using Yaoya.Models;
using Yaoya.Services;
using Yaoya.ViewModels;
using Yaoya.Views;
using Microsoft.Extensions.Logging;

namespace Yaoya;

public partial class App : Application
{
    private IHost _wpfHost;        // WPF用ホスト
    private WebApplication _webApp; // Kestrel用

    public T GetService<T>()
        where T : class
        => _wpfHost.Services.GetService(typeof(T)) as T;

    public App()
    {
    }


    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        // ★ WPF用ホスト（元々のやつ）
        _wpfHost = Host.CreateDefaultBuilder(e.Args)
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(appLocation);
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole(options =>
                {
                    options.LogToStandardErrorThreshold = LogLevel.Trace;
                });
            })
            .ConfigureServices(ConfigureWpfServices)
            .Build();

        // WPFホストはUIスレッドで起動（元々の動作）
        await _wpfHost.StartAsync();

        // ★ Kestrel用ホストは完全に別で作って別スレッドで起動
        var builder = WebApplication.CreateBuilder(e.Args);
        builder.WebHost.UseUrls("http://localhost:5000");
        builder.Logging.AddConsole(options =>
        {
            options.LogToStandardErrorThreshold = LogLevel.Trace;
        });

        // MCPに必要なサービスだけ登録
        // IProductServiceはWPFホストのものを共有したい場合は引数で渡す
        builder.Services.AddSingleton(
            _wpfHost.Services.GetRequiredService<IProductService>()
        );
        builder.Services.AddMcpServer()
            .WithHttpTransport()
            .WithTools<YaoyaMcpTools>();

        _webApp = builder.Build();
        _webApp.UseRouting();
        _webApp.MapMcp();

        // ★ Kestrelだけ別スレッドで起動！WPFに一切干渉せん！
        _ = Task.Factory.StartNew(
            async () => await _webApp.StartAsync(),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default
        );
    }

    // WPF用サービス（MCPなし！）
    private void ConfigureWpfServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddHostedService<ApplicationHostService>();

        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IProductService, ProductService>();

        // Views and ViewModels
        services.AddTransient<IShellWindow, ShellWindow>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainPage>();
        services.AddTransient<FreeSpaceViewModel>();
        services.AddTransient<FreeSpacePage>();
        services.AddTransient<ProductListViewModel>();
        services.AddTransient<ProductListPage>();
        services.AddTransient<ProductListDetailViewModel>();
        services.AddTransient<ProductListDetailPage>();

        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _wpfHost.StopAsync();
        _wpfHost.Dispose();

        await _webApp.StopAsync();
        await _webApp.DisposeAsync();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
    }

}
