using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_1.ViewModels;
using Module_1.Views;

namespace Application.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new Module_1ViewModel();
            var view = new Module_1View(viewModel);
            view.ShowDialog();
        }
    }
}