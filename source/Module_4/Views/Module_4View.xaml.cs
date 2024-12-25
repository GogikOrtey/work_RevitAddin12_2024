using Module_4.ViewModels;

namespace Module_4.Views
{
    public sealed partial class Module_4View
    {
        public Module_4View(Module_4ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}