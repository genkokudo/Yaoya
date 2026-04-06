using System.Windows.Controls;

using Yaoya.ViewModels;

namespace Yaoya.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
