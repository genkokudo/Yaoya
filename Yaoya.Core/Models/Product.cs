namespace Yaoya.Core.Models;

public class Product
{
    public string Name { get; set; } = "";
    public string Origin { get; set; } = "";
    public int Price { get; set; }
    public ProductCategory Category { get; set; }
}
