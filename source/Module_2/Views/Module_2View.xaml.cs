using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using Module_2.ViewModels;

namespace Module_2.Views
{
    public sealed partial class Module_2View
    {
        // Корректно ли закрылось окно по созданию стены?
        // Это метод установки значения для ViewModel, которое можно дальше будет использоватьв Model
        private void SetValueFor_IsWindowCorrectCloset(bool val)
        {
            var viewModel = (Module_2ViewModel)DataContext;
            viewModel.IsWindowClosetCorrect = val; 
        }

        public static int ExRadius = 0;

        public Module_2View(Module_2ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            TB_inputRadius.Text = "5"; // Устанавливаю начальное значение для поля ввода радиуса

            SetValueFor_IsWindowCorrectCloset(true); // Тестово устанавливаю значение false, для общей переменной
        }

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


        // Кнопка подтверждения генерации стен
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

                if (radius <= 0)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть больше 0", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (radius > 10)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть меньше 10", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Радиус корректен
                    TaskDialog.Show("Info", "Радиус корректен: " + radius);
                    
                    this.Close();   // Закрываем это окно только тогда, когда данные, введённые пользователем корректны
                }
            }            
        }

        private void Button_Click_Test1(object sender, System.Windows.RoutedEventArgs e)
        {
            //ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;
        }
    }
}