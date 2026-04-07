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

    /// <summary>
    /// 表示する商品一覧
    /// </summary>
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

    /// <summary>
    /// 現在の商品一覧状態を取得してProductsに反映する
    /// </summary>
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

    /// <summary>
    /// 更新ボタン
    /// </summary>
    [RelayCommand]
    private void Refrash()
    {
        LoadProducts();
    }
}
