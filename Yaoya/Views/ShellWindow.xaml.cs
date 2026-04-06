using System.Windows.Controls;

using MahApps.Metro.Controls;

using Yaoya.Contracts.Views;
using Yaoya.ViewModels;

namespace Yaoya.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
