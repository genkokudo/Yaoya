using System.Windows.Controls;

using Yaoya.ViewModels;

namespace Yaoya.Views;

public partial class ProductListDetailPage : Page
{
    public ProductListDetailPage(ProductListDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
