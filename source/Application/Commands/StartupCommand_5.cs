using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

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
            #if REVIT2024
                TaskDialog.Show("Информация", "Это версия Revit 24");
            #elif REVIT2025
                TaskDialog.Show("Информация", "Это версия Revit 25");
            #else
                TaskDialog.Show("Информация", "Версия Revit неизвестна");
            #endif

            return Result.Succeeded;
        }
    }
}



