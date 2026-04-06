using Yaoya.Core.Contracts.Services;
using Yaoya.Core.Models;
namespace Yaoya.Core.Services;

public class ProductService: IProductService
{
    private readonly List<Product> _products;

    public ProductService()
    {
        _products = GetData();
    }

    // ダミーデータ
    private static List<Product> GetData() =>
    [
        new Product { Name = "りんご",   Origin = "青森", Price = 150, Category = ProductCategory.Fruit },
        new Product { Name = "みかん",   Origin = "愛媛", Price = 80,  Category = ProductCategory.Fruit },
        new Product { Name = "トマト",   Origin = "熊本", Price = 120, Category = ProductCategory.Vegetable },
        new Product { Name = "きゅうり", Origin = "宮崎", Price = 90,  Category = ProductCategory.Vegetable },
        new Product { Name = "ぶどう",   Origin = "山梨", Price = 300, Category = ProductCategory.Fruit },
        new Product { Name = "ほうれん草", Origin = "千葉", Price = 100, Category = ProductCategory.Vegetable },
    ];

    public IReadOnlyList<Product> GetProducts() => _products.AsReadOnly();

    // MCPから呼ばれるメソッド
    public bool RemoveProduct(string name)
    {
        var target = _products.FirstOrDefault(p => p.Name == name);
        if (target is null) return false;
        _products.Remove(target);
        return true;
    }
}