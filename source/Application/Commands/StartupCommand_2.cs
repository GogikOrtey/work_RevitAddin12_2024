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
    public class StartupCommand_2 : IExternalCommand
    {
        // Код, который выполняется по нажатию кнопки №2
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Старый код - для вывода id выбранного элемента
            //Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            //Element element = doc.GetElement(myRef);
            //ElementId id = element.Id;

            // Выбор точки на поверхности
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            // Вывод координат выбранной точки, с использованием самописного метода
            OthersMyVoid.ShowInfoWindow($"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");
            // Мой самописный метод я поместил в файл "OthersMyVoid.cs", в проекте Application

            // Создание и вывод диалогового окна, с использованием встроенного метода
            TaskDialog.Show("Информация", $"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");

            // Определение центра и радиуса для окружности
            XYZ center = new XYZ(point.X + 5, point.Y, point.Z); // Можно изменять смещение по оси X
            double radius = 5.0; // Радиус окружности

            // Создание окружности из арок
            // 1я арка
            Arc arc1 = Arc.Create(new XYZ(center.X - radius, center.Y, center.Z),   // С левого края, по х
                                  new XYZ(center.X + radius, center.Y, center.Z),   // До правого края, по х
                                  new XYZ(center.X, center.Y + radius, center.Z));  // Через верх по у
            // 2я арка
            Arc arc2 = Arc.Create(new XYZ(center.X - radius, center.Y, center.Z),   // То же самое
                                  new XYZ(center.X + radius, center.Y, center.Z),
                                  new XYZ(center.X, center.Y - radius, center.Z));  // Через низ по у

            // Выбор уровня
            Level level1 = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault(level => level.Name.Equals("Уровень 1"));

            // "Уровень" - это понятие из Revit, которе можно обозначить как "Этаж", на котором мы работаем, и создаём объекты

            if (level1 == null)
            {
                message = "Уровень 'Уровень 1' не найден.";
                return Result.Failed;

                // Если мы используем return Result.Failed;, то Revit не выбросит исключение, которое закроет его без сохранения проекта, 
                // Он просто покажет встроенноное окно с ошибкой, и программа не закроется, что удобно
            }

            // Создание объекта на основе окружности
            using (Transaction transaction = new Transaction(doc, "Создание окружной стены"))
            {
                transaction.Start();

                Wall.Create(doc, arc1, level1.Id, false);
                Wall.Create(doc, arc2, level1.Id, false);

                transaction.Commit();
            }

            // Вывод сообщения о создании окружной стены
            OthersMyVoid.ShowInfoWindow("Окружная стена успешно создана в выбранной точке!");

            return Result.Succeeded;
        }
    }
}
