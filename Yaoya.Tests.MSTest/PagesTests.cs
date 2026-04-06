using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Yaoya.Contracts.Services;
using Yaoya.Core.Contracts.Services;
using Yaoya.Core.Services;
using Yaoya.Models;
using Yaoya.Services;
using Yaoya.ViewModels;
using Yaoya.Views;

namespace Yaoya.Tests.MSTest;

[TestClass]
public class PagesTests
{
    private readonly IHost _host;

    public PagesTests()
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => c.SetBasePath(appLocation))
            .ConfigureServices(ConfigureServices)
            .Build();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Core Services

        // Services
        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        services.AddTransient<ProductListViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<FreeSpaceViewModel>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    // TODO: Add tests for functionality you add to ProductListViewModel.
    [TestMethod]
    public void TestProductListViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(ProductListViewModel));
        Assert.IsNotNull(vm);
    }

    [TestMethod]
    public void TestGetProductListPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(ProductListViewModel).FullName);
            Assert.AreEqual(typeof(ProductListPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to MainViewModel.
    [TestMethod]
    public void TestMainViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(MainViewModel));
        Assert.IsNotNull(vm);
    }

    [TestMethod]
    public void TestGetMainPageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
            Assert.AreEqual(typeof(MainPage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }

    // TODO: Add tests for functionality you add to FreeSpaceViewModel.
    [TestMethod]
    public void TestFreeSpaceViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(FreeSpaceViewModel));
        Assert.IsNotNull(vm);
    }

    [TestMethod]
    public void TestGetFreeSpacePageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(FreeSpaceViewModel).FullName);
            Assert.AreEqual(typeof(FreeSpacePage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }
}
