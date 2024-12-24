using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_2.ViewModels;
using Module_2.Views;

namespace Application.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand_2 : ExternalCommand
    {
        public override void Execute()
        {
            var viewModel = new Module_2ViewModel();
            var view = new Module_2View(viewModel);
            view.ShowDialog();
        }
    }
}