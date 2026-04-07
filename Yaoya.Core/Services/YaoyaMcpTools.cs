using ModelContextProtocol.Server;
using System.ComponentModel;
using Yaoya.Core.Contracts.Services;

namespace Yaoya.Core.Services
{
    // App.xaml.csでWithTools<ProductService>が勝手にMCP用のインスタンスをつくってしまい、状態が2つになるので対策する。
    [McpServerToolType]
    public class YaoyaMcpTools
    {
        private readonly IProductService _productService;

        // DIでSingletonのProductServiceが注入されるので、MCP用のインスタンスは生成されない。
        // （MCP用のツールインスタンスはデータを持ってはいけない。DIでシングルトンオブジェクトを取り寄せて更新をかける。）
        public YaoyaMcpTools(IProductService productService)
        {
            _productService = productService;
        }

        [McpServerTool, Description("指定した名前の商品を一覧から削除する")]
        public bool RemoveProduct(string name)
        {
            return _productService.RemoveProduct(name);
        }
    }
}
