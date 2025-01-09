using Module_5.ViewModels;

namespace Module_5.Views
{
    public sealed partial class Module_5View
    {
        public Module_5View(Module_5ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}