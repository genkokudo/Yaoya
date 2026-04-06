using System.Windows.Controls;

using Yaoya.ViewModels;

namespace Yaoya.Views;

public partial class FreeSpacePage : Page
{
    public FreeSpacePage(FreeSpaceViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
