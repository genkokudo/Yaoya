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
    private IHost _host;

    public T GetService<T>()
        where T : class
        => _host.Services.GetService(typeof(T)) as T;

    public App()
    {
    }


    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        var builder = WebApplication.CreateBuilder(e.Args);

        builder.Configuration.SetBasePath(appLocation);

        builder.WebHost.UseUrls("http://localhost:5000");

        // サービス登録
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // ★ ルーティング明示
        app.UseRouting();

        // ★ MapMcp()は絶対StartAsyncの前！
        app.MapMcp("/sse");  // ← パス明示してみる

        // ★ 確認用エンドポイント
        app.MapGet("/health", () => "yaoya MCP server is running!");

        _host = app;
        await _host.StartAsync();
    }

    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // App Host
        services.AddHostedService<ApplicationHostService>();    // 起動はApplicationHostServiceで行う

        // Activation Handlers

        // Core Services

        // Services
        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IProductService, ProductService>();

        // ★ HTTP Transport に変更！
        services.AddMcpServer()
                .WithHttpTransport(options =>
                {
                    options.Stateless = true;
                })
            .WithTools<YaoyaMcpTools>();

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

        // Configuration
        services.Configure<AppConfig>(configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
    }
}
