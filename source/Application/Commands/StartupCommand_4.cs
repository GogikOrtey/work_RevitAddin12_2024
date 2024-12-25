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

namespace Application.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]

    // Этот модуль выводит 2 сообщения, о координатах точки, куда нажал пользователь мышкой
    public class StartupCommand_4 : IExternalCommand
    {
        // Код, который выполняется по нажатию кнопки №2
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Выбор точки на поверхности
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            // Вывод координат выбранной точки, с использованием самописного метода
            OthersMyVoid.ShowInfoWindow($"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");
            // Мой самописный метод я поместил в файл "OthersMyVoid.cs", в проекте Application

            // Создание и вывод диалогового окна, с использованием встроенного метода
            TaskDialog.Show("Информация", $"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");

            return Result.Succeeded;
        }
    }
}
