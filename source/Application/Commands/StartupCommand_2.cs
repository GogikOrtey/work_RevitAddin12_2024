using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.DependencyInjection;
using Module_2.ViewModels;
using Module_2.Views;

namespace Application.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]

    // Этот модуль создаёт окружную стену, в точке нажатия пользователем
    public class StartupCommand_2 : IExternalCommand
    {
        // Код, который выполняется по нажатию кнопки №2
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Выбор точки на поверхности
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            // Открываю визуальное окно
            var viewModel = new Module_2ViewModel();
            var view = new Module_2View(viewModel);
            view.ShowDialog();

            //ShowInfoAndGetDataWindow();

            //double radius = 5.0; // Радиус окружности

            //// Определение центра и радиуса для окружности
            //XYZ center = new XYZ(point.X + 5, point.Y, point.Z); // Можно изменять смещение по оси X            

            //// Создание окружности из арок
            //// 1я арка
            //Arc arc1 = Arc.Create(new XYZ(center.X - radius, center.Y, center.Z),   // С левого края, по х
            //                      new XYZ(center.X + radius, center.Y, center.Z),   // До правого края, по х
            //                      new XYZ(center.X, center.Y + radius, center.Z));  // Через верх по у
            //// 2я арка
            //Arc arc2 = Arc.Create(new XYZ(center.X - radius, center.Y, center.Z),   // То же самое
            //                      new XYZ(center.X + radius, center.Y, center.Z),
            //                      new XYZ(center.X, center.Y - radius, center.Z));  // Через низ по у

            //// Выбор уровня
            //Level level1 = new FilteredElementCollector(doc)
            //    .OfClass(typeof(Level))
            //    .Cast<Level>()
            //    .FirstOrDefault(level => level.Name.Equals("Уровень 1"));

            //// "Уровень" - это понятие из Revit, которе можно обозначить как "Этаж", на котором мы работаем, и создаём объекты

            //if (level1 == null)
            //{
            //    message = "Уровень 'Уровень 1' не найден.";
            //    return Result.Failed;

            //    // Если мы используем return Result.Failed;, то Revit не выбросит исключение, которое закроет его без сохранения проекта, 
            //    // Он просто покажет встроенноное окно с ошибкой, и программа не закроется, что удобно
            //}

            //// Создание объекта на основе окружности
            //using (Transaction transaction = new Transaction(doc, "Создание окружной стены"))
            //{
            //    transaction.Start();

            //    Wall.Create(doc, arc1, level1.Id, false);
            //    Wall.Create(doc, arc2, level1.Id, false);

            //    transaction.Commit();
            //}

            //// Вывод сообщения о создании окружной стены
            //OthersMyVoid.ShowInfoWindow("Окружная стена успешно создана в выбранной точке!");

            return Result.Succeeded;
        }

        public void ShowInfoAndGetDataWindow()
        {
            /*
                Структура окна:

                Стандартное окно - messageBox. Размер - 800 х 250px
                    Панель внутри окна, растянутая на весь его размер - dockPanel. Она нужна, что бы мы могли как хотим располагать элементы внутри окна.
                        Текстовый блок с надписью - textBlock. Он центрирован по середине окна
                        Кнопка с надписью Ок - okButton. По нажатию на неё - окно закрывается
            */

            var dockPanel = new DockPanel
            {
                // Это конструктор панели внутри окна
                // Тут можно например задать для неё заливку, и другие параметры
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
                Text = "В выбранной точке будет создана окружная стена",

                TextAlignment = TextAlignment.Center,               // Выравнивание текста по середине, в текстовом поле
                HorizontalAlignment = HorizontalAlignment.Center,   // Центрирование смого элемента по центру, по горизонтали
                VerticalAlignment = VerticalAlignment.Center,       // Центрирование по вертикали
                Margin = new Thickness(20),                         // Margin 20
                FontSize = 14                                       // Размер шрифта 14
            };

            // Создаем кнопку "Ок"
            var okButton = new Button
            {
                Content = "Ok",
                Width = 75,
                Margin = new Thickness(0, 0, 0, 10),                // Устанавливаем, что бы кнопка была на 10px от нижней границы окна
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (sender, args) => messageBox.Close(); // Задаём событие по нажатию кнопки

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
