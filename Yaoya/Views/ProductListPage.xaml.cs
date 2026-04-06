using System.Windows.Controls;

using Yaoya.ViewModels;

namespace Yaoya.Views;

public partial class ProductListPage : Page
{
    public ProductListPage(ProductListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
