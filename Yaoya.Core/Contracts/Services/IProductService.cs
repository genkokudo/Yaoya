using System;
using System.Collections.Generic;
using System.Text;
using Yaoya.Core.Models;

namespace Yaoya.Core.Contracts.Services;

public interface IProductService
{
    IReadOnlyList<Product> GetProducts();
    bool RemoveProduct(string name);
}
