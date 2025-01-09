using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_2.ViewModels;
using Module_2.Views;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.DependencyInjection;

#if REVIT_24
#define REVIT_24
#elif REVIT_25
#define REVIT_25
#endif

namespace Application.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]

    // Этот модуль выводит разные сообщения, в зависимости от версии программы Revit
    public class StartupCommand_5 : IExternalCommand
    {
        // Код, который выполняется по нажатию кнопки 4
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #if REVIT_24
                TaskDialog.Show("Информация", "Это версия Revit 24");
            #elif REVIT_25
                TaskDialog.Show("Информация", "Это версия Revit 25");
            #else
                TaskDialog.Show("Информация", "Неизвестная версия Revit");
            #endif

            return Result.Succeeded;
        }
    }
}
