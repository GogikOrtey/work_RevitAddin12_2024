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

            // Выбор точки на поверхности (например, на полу)
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            // Вывод координат выбранной точки
            ShowInfoWindow($"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");

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

            if (level1 == null)
            {
                message = "Уровень 'Уровень 1' не найден.";
                return Result.Failed;
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
            ShowInfoWindow("Окружная стена успешно создана в выбранной точке!");

            return Result.Succeeded;
        }

        // Выводит окно с информацией
        public void ShowInfoWindow(string inpText)
        {
            var dockPanel = new DockPanel
            {

            };

            // Создаем окно и добавляем в него DockPanel
            var messageBox = new Window
            {
                Title = "Information",
                Content = dockPanel,
                Width = 800,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Создаем текстовый блок
            var textBlock = new TextBlock
            {
                Text = inpText,

                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20),
                FontSize = 14
            };

            // Создаем кнопку "Ок"
            var okButton = new Button
            {
                Content = "Ok",
                Width = 75,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (sender, args) => messageBox.Close();

            // Добавляем кнопку внизу с помощью DockPanel
            DockPanel.SetDock(okButton, Dock.Bottom);
            dockPanel.Children.Add(okButton);

            // Добавляем текстовый блок в центр с помощью DockPanel
            dockPanel.Children.Add(textBlock);

            // Показываем окно
            messageBox.ShowDialog();
        }
    }
}
