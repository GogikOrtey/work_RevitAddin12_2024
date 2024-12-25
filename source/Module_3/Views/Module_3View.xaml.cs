using Module_3.ViewModels;

namespace Module_3.Views
{
    public sealed partial class Module_3View
    {
        public Module_3View(Module_3ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}