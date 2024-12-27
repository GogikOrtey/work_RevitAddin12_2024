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
        //List<string> wallTypeNames = new List<string>();

        Document doc_pub; // Для простого доступа в других процедурах

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

            //WallTypeNames.Add("Element 1");

            // Инициализируем окно и ViewModel

            // Передаю в конструктор Module_2ViewModel этот список, что бы дальше его использовать в View
            var viewModel = new Module_2ViewModel(WallTypeNames); 
            var view = new Module_2View(viewModel);

            // Показываем окно
            view.ShowDialog();

            // Старый код, потом удалить:
            {
                //// Показываем окно и вносим изменения в ViewModel после инициализации окна
                //viewModel.AddWallTypeName("3 Задаю значение в коде Model через AddWallTypeName");
                //viewModel.WallTypeNames.Add("4 Задаю значение в коде Model через метод .Add");

                //// Инициализирую окно
                //var viewModel = new Module_2ViewModel();
                //var view = new Module_2View(viewModel);

                //// Добавляю в список все типы стен
                ////GetAllWallTypes(doc);
                ////viewModel.WallTypeNames.Add("111");
                ////viewModel.WallTypeNames.Add("222");

                ////viewModel.AddWallTypeName("789");
                ////viewModel.WallTypeNames = new List<string> { "333", "444" };

                //viewModel.AddWallTypeName("3 Задаю значение в коде Model через AddWallTypeName");
                //viewModel.WallTypeNames.Add("4 Задаю значение в коде Model через метод .Add");

                //// Показываю окно
                //view.ShowDialog();

                //viewModel.AddWallTypeName("5 Задаю значение в коде Model через AddWallTypeName");
                //viewModel.WallTypeNames.Add("6 Задаю значение в коде Model через метод .Add");
            }

            //
            // Дальнейший код будет выполнятся, когда окно закроется:
            //

            

            TaskDialog.Show("Info", "Радиус = " + viewModel.InputRadius + "\nОкно закрыто корректно = " + viewModel.IsWindowClosetCorrect + 
                "\nВыбранный материал стены = " + viewModel.SelectedWallMaterial);

            // Если окно ввода информации для создания стены закрыто корректно
            if (viewModel.IsWindowClosetCorrect)
            {
                int InpRadius = Module_2View.ExRadius;

                //TaskDialog.Show("Info", "_1_InpRadius = " + InpRadius);
                //double radius = 5.0; // Радиус окружности

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

                // "Уровень" - это понятие из Revit, которе можно обозначить как "Этаж", на котором мы работаем, и создаём объекты

                if (level1 == null)
                {
                    message = "Уровень 'Уровень 1' не найден.";
                    return Result.Failed;

                    // Если мы используем return Result.Failed;, то Revit не выбросит исключение, которое закроет его без сохранения проекта, 
                    // Он просто покажет встроенноное окно с ошибкой, и программа не закроется, что удобно
                }

                Wall wall1, wall2;

                // Находим тип стены "Витраж 1" из семейства "Curtain Wall"
                WallType curtainWallType = new FilteredElementCollector(doc)
                                           .OfClass(typeof(WallType))
                                           .Cast<WallType>()
                                           .FirstOrDefault(wt => wt.FamilyName == "Curtain Wall" && wt.Name == "Витраж 1");

                if (curtainWallType != null)
                {
                    using (Transaction transaction = new Transaction(doc, "Создание окружной стены"))
                    {
                        transaction.Start();

                        // Создаю стены
                        wall1 = Wall.Create(doc, arc1, level1.Id, false);
                        wall2 = Wall.Create(doc, arc2, level1.Id, false);

                        // Задаем тип стены до завершения транзакции
                        wall1.ChangeTypeId(curtainWallType.Id);

                        transaction.Commit();
                    }

                    // Вывод сообщения о создании окружной стены
                    OthersMyVoid.ShowInfoWindow("Окружная стена успешно создана с типом 'Витраж 1'!");
                }
                else
                {
                    OthersMyVoid.ShowInfoWindow("Тип стены 'Витраж 1' не найден.");
                }

            }

            return Result.Succeeded;
        }


        // Процедура для изменения типа стены 
        void ChangeWallMaterialFrom(Wall inpWall, string nameNewWallMaterial)
        {
            Document doc = doc_pub;
            ElementId materialId = GetMaterialIdByName(doc, nameNewWallMaterial);

            // Функция для получения ID материала по его имени
            ElementId GetMaterialIdByName(Document doc, string materialName)
            {
                Material material = new FilteredElementCollector(doc)
                    .OfClass(typeof(Material))
                    .Cast<Material>()
                    .FirstOrDefault(m => m.Name.Equals(materialName));
                return material?.Id;
            }

            // Изменение материала стен
            void SetWallMaterial(Wall wall, ElementId materialId)
            {
                // Получаем тип стены
                WallType wallType = wall.WallType;

                // Начинаем транзакцию
                using (Transaction transaction = new Transaction(doc, "Изменение материала стены"))
                {
                    transaction.Start();

                    // Установка материала через параметр
                    Parameter materialParam = wallType.LookupParameter("Material");
                    if (materialParam != null && !materialParam.IsReadOnly)
                    {
                        materialParam.Set(materialId);
                    }

                    transaction.Commit();
                }
            }

            // Применение изменения материала к вашей новой стене
            SetWallMaterial(inpWall, materialId);
        }




        // Переводит числовое значение из метров в футы
        public double ConvertMetersToFoot(double inputVal)
        {
            inputVal = inputVal / 3.048f;
            inputVal = Math.Round(inputVal, 2);

            return (inputVal);
        }

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
