using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.DependencyInjection;
using Module_2.ViewModels;

namespace Module_2.Views
{
    public sealed partial class Module_2View
    {
        // Главный метод инициализации окна:
        public Module_2View(Module_2ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            TB_inputRadius.Text = "5";                  // Устанавливаю начальное значение для поля ввода радиуса
            AddElementForComboBoxSelectWallMaterial();  // Добавляю значения в список выбора материала для стен

            SetValueFor_IsWindowCorrectCloset(false);   // По умолчанию устанавливаю false, и устанавливаю true только при корректном закрытии
        }

        // Корректно ли закрылось окно по созданию стены?
        // Это метод установки значения для ViewModel, которое можно дальше будет использоватьв Model
        private void SetValueFor_IsWindowCorrectCloset(bool val)
        {
            var viewModel = (Module_2ViewModel)DataContext;
            viewModel.IsWindowClosetCorrect = val; 
        }

        public static int ExRadius = 0;

        // Здесь добавляются значения в список выбора материала для стен
        void AddElementForComboBoxSelectWallMaterial()
        {
            var viewModel = (Module_2ViewModel)DataContext;

            viewModel.WallTypeNames.Add("444");
            viewModel.WallTypeNames.Add("555");

            string message = string.Join(", ", viewModel.WallTypeNames);
            TaskDialog.Show("Wall Type Names: ", "_" + message + "_");

            //foreach (var item in viewModel.WallTypeNames)
            //{
            //    ComboBoxForSelectWallMaterial.Items.Add(item);
            //}

            ComboBoxForSelectWallMaterial.Items.Add("1");
            //ComboBoxForSelectWallMaterial.Items.Add("2");
            //ComboBoxForSelectWallMaterial.Items.Add("3");
        }

        // Красная кнопка снизу "Отменить"
        private void Button_Click_CloseThisWindow(object sender, System.Windows.RoutedEventArgs e)
        {
            // Закрытие окна
            this.Close();
        }

        // Метод, который вызывается при каждом изменении значений поля, пользователем
        private void TB_inputRadius_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIsValidValueFronInput();
        }

        // Метод проверяет корректность значения в поле ввода радиуса
        // И если значение некорректно, то показывает красный текст, а если верное - то скрывает
        private bool CheckIsValidValueFronInput()
        {
            string inputRadius = TB_inputRadius.Text;
            double radius;
            bool isNumber = double.TryParse(inputRadius, out radius);

            //MessageBox.Show("Значение TB_inputRadius.Text = _" + inputRadius + "_", "Info", MessageBoxButton.OK);

            if (inputRadius == "")
            {
                // Если значение пусто, то не вывожу текст с ошибкой
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;
                return true; 
            }

            if (!isNumber)
            {
                // Значение не является числом, установите переменную в false
                // Дополнительно, вы можете уведомить пользователя, что введено некорректное значение
                //MessageBox.Show("Введите числовое значение для радиуса.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;

                return false;
            }
            else
            {
                // Если значение является числом
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;
                return true;
            }
        }



        /*
        
                // Получение всех типов стен в документе
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> wallTypes = collector.OfClass(typeof(WallType)).ToElements();

                List<string> wallTypeNames = new List<string>();

                foreach (Element wallTypeElem in wallTypes)
                {
                    WallType wallType = wallTypeElem as WallType;
                    if (wallType != null)
                    {
                        wallTypeNames.Add(wallType.Name);
                    }
                }

        */

        // Кнопка подтверждения генерации стен
        // Зелёная кнопка снизу "Создать стену"
        private void Button_Click_AcceptGenerateWall(object sender, System.Windows.RoutedEventArgs e)
        {

            //
            //  Проверка корректности поля радиуса:
            //

            string inputRadius = TB_inputRadius.Text;
            double radius;
            bool isNumber = double.TryParse(inputRadius, out radius);

            if (!isNumber)
            {
                // Значение не является числом, установите переменную в false
                // Дополнительно, вы можете уведомить пользователя, что введено некорректное значение
                MessageBox.Show("Введите числовое значение для радиуса.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // Если значение является числом

                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;

                //TaskDialog.Show("Info", "Значение корректно, и = " + radius);

                if (radius < 1)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть больше 1 метра", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (radius > 20)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть меньше 20 метров", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Радиус корректен
                    //TaskDialog.Show("Info", "Радиус корректен: " + radius);

                    SetValueFor_IsWindowCorrectCloset(true);
                    this.Close();   // Закрываем это окно только тогда, когда данные, введённые пользователем корректны
                }
            }            
        }
    }
}









