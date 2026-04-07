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

        // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
        //_host = Host.CreateDefaultBuilder(e.Args)
        //        .ConfigureAppConfiguration(c =>
        //        {
        //            c.SetBasePath(appLocation);
        //        })
        //        .ConfigureServices(ConfigureServices)
        //        .Build();

        _host = Host.CreateDefaultBuilder(e.Args)
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(appLocation);
            })
            .ConfigureLogging(logging =>
            {
            // 全ログをstderrに向ける（stdoutはMCP専用にする）
            logging.AddConsole(options =>
            {
                options.LogToStandardErrorThreshold = LogLevel.Trace;
            });
        })
        .ConfigureServices(ConfigureServices)
        .Build();

        await _host.StartAsync();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
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

        // MCPサーバ登録：上記サービスよりも後に登録しないとMCP受信時に画面更新できないはず。
        services.AddMcpServer()
                .WithStdioServerTransport()
                .WithTools<YaoyaMcpTools>();   // アセンブリを検索するのではなく、この方法で登録しないとTool認識しない。ここではYaoyaMcpToolsのインスタンスがMCP用に必ず生成される。AddSingletonしてもそうなるので更新不整合のリスク回避としてデータを持たせない事。

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
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}
