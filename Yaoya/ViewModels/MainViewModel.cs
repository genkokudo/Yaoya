using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Yaoya.Contracts.ViewModels;
using Yaoya.Core.Contracts.Services;
using Yaoya.Core.Models;
using Yaoya.Core.Services;

namespace Yaoya.ViewModels;

public partial class MainViewModel : ObservableObject, INavigationAware
{
    private readonly IProductService _productService;
    public ObservableCollection<Product> Products { get; } = new();

    public MainViewModel(IProductService productService)
    {
        _productService = productService;

        _productService.ProductsChanged += OnProductsChanged;  // MCP受信時に画面更新する
    }

    public async void OnNavigatedTo(object parameter)
    {
        LoadProducts();

    }
    public void OnNavigatedFrom()
    {
    }

    public void LoadProducts()
    {
        Products.Clear();
        foreach (var p in _productService.GetProducts())
            Products.Add(p);
    }

    private void OnProductsChanged()
    {
        // MCPはUIスレッド外から来るのでDispatcher必須！
        App.Current.Dispatcher.Invoke(LoadProducts);
    }

    [RelayCommand]
    private void Refrash()
    {
        LoadProducts();
    }
}
