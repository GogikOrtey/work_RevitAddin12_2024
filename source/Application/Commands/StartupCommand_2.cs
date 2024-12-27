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
        private Document doc_pub; // Для простого доступа в других процедурах этого класса

        // Код, который выполняется по нажатию кнопки №2
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            doc_pub = doc;

            // Выбор точки на поверхности
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            List<string> WallTypeNames = new List<string>();

            // Получаю все доступные в документе материалы стен
            // И загружаю их в список WallTypeNames
            WallTypeNames = GetAllWallTypes(doc);

            // Инициализируем окно и ViewModel

            // Передаю в конструктор Module_2ViewModel этот список, что бы дальше его использовать в View
            var viewModel = new Module_2ViewModel(WallTypeNames); 
            var view = new Module_2View(viewModel);

            // Показываем окно
            view.ShowDialog();

            //
            // Дальнейший код будет выполнятся, когда окно закроется:
            //            

            //TaskDialog.Show("Info", "Радиус = " + viewModel.InputRadius + "\nОкно закрыто корректно = " + viewModel.IsWindowClosetCorrect + 
            //    "\nВыбранный материал стены = " + viewModel.SelectedWallMaterial);

            // Если окно ввода информации для создания стены закрыто корректно
            if (viewModel.IsWindowClosetCorrect)
            {
                //int InpRadius = Module_2View.ExRadius;
                //double radius = 5.0; // Радиус окружности

                // В переменной viewModel.InputRadius хранится радиус, который ввёл пользователь в окне
                // Его корректность уже проверена во View

                double radius;
                double.TryParse(viewModel.InputRadius, out radius);

                // Перевод из метров во внутренние единицы (футы)
                radius = ConvertMetersToFoot(radius);

                // Определение центра и радиуса для окружности
                XYZ center = new XYZ(point.X + ConvertMetersToFoot(5), point.Y, point.Z); // Можно изменять смещение по оси X            

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

                // "Уровень" - это понятие из Revit, которе можно обозначить как "Этаж", на котором мы работаем и создаём объекты

                if (level1 == null)
                {
                    message = "Уровень 'Уровень 1' не найден.";
                    return Result.Failed;

                    // Если мы используем return Result.Failed;, то Revit не выбросит исключение, которое закроет его без сохранения проекта, 
                    // Он просто покажет встроенноное окно с ошибкой, и программа не закроется, что удобно
                }

                // Создаю пустые экземпляры стен, для более удобного их дальнейшего использования
                Wall wall1, wall2;

                // Мы получаем значение выбранного пользователем типа для стены, из переменной viewModel.SelectedWallMaterial

                // Находим заданный тип стены, и создаём экземпляр WallType для назначения его новым созданным стенам далее
                WallType curtainWallType = new FilteredElementCollector(doc)
                                           .OfClass(typeof(WallType))
                                           .Cast<WallType>()
                                           .FirstOrDefault(wt => wt.Name == viewModel.SelectedWallMaterial);

                // Корректность значения viewModel.SelectedWallMaterial уже проверена во View

                if (curtainWallType != null)
                {
                    using (Transaction transaction = new Transaction(doc, "Создание окружной стены"))
                    {
                        transaction.Start();

                        // Создаю стены
                        wall1 = Wall.Create(doc, arc1, level1.Id, false);
                        wall2 = Wall.Create(doc, arc2, level1.Id, false);
                        // Здесь важно отметить, что я создаю стены - новый объект типа Wall, используй заданную ранее Arc (кривую), как путь для стены

                        // Задаем тип для создаваемой стены 
                        wall1.ChangeTypeId(curtainWallType.Id);
                        wall2.ChangeTypeId(curtainWallType.Id);

                        transaction.Commit();
                    }

                    //OthersMyVoid.ShowInfoWindow("Окружная стена успешно создана с типом: " + viewModel.SelectedWallMaterial);
                }
                else
                {
                    //OthersMyVoid.ShowInfoWindow("Тип стены 'Витраж 1' не найден.");
                    MessageBox.Show($"Тип стены {viewModel.SelectedWallMaterial} не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            return Result.Succeeded;
        }


        // Переводит числовое значение из метров в футы
        public double ConvertMetersToFoot(double inputVal)
        {
            inputVal = inputVal / 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }

        // Эта процедура находит все типы стен, которые есть в документе, и записывает их в список wallTypeNames_ins
        // Затем он передаётся в конструктор ViewModel, где далее используется во View - выводится как значения списка с выбором в окне
        List<string> GetAllWallTypes(Document doc)
        {
            List<string> wallTypeNames_ins = new List<string>();

            // Получение всех типов стен в документе
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> wallTypes = collector.OfClass(typeof(WallType)).ToElements();            

            foreach (Element wallTypeElem in wallTypes)
            {
                WallType wallType = wallTypeElem as WallType;
                if (wallType != null)
                {
                    // Он их просто добавляет в список
                    wallTypeNames_ins.Add(wallType.Name);
                }
            }

            return wallTypeNames_ins;
        }
    }
}
