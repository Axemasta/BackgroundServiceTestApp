using BackgroundServiceTestApp.ViewModels;

namespace BackgroundServiceTestApp.Pages;

public partial class BackgroundTestPage : BasePage<BackgroundTestViewModel>
{
    public BackgroundTestPage(BackgroundTestViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}