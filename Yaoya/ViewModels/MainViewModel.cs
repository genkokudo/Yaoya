using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Yaoya.Contracts.ViewModels;
using Yaoya.Core.Contracts.Services;
using Yaoya.Core.Models;
using Yaoya.Core.Services;

namespace Yaoya.ViewModels;

public class MainViewModel : ObservableObject, INavigationAware
{
    private readonly IProductService _productService;
    public ObservableCollection<Product> Products { get; } = new();

    public MainViewModel(IProductService productService)
    {
        _productService = productService;
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

    public bool RemoveProduct(string name)
    {
        var result = _productService.RemoveProduct(name);
        if (result) LoadProducts();
        return result;
    }
}
