using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using Module_2.ViewModels;

namespace Module_2.Views
{
    public sealed partial class Module_2View
    {
        public Module_2View(Module_2ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void Button_Click_CloseThisWindow(object sender, System.Windows.RoutedEventArgs e)
        {
            // Закрытие окна
            this.Close();
        }

        // Кнопка подтверждения генерации стен
        private void Button_Click_AcceptGenerateWall(object sender, System.Windows.RoutedEventArgs e)
        {
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
                // Значение является числом, можете использовать переменную radius

                TaskDialog.Show("Info", "Значение корректно, и = " + radius);
            }



            //TaskDialog.Show("Заголовок окна", "Текст внутри окна");

            // Проверить корректность ввода в числовом поле радиуса
            // Если ввод некорректен - то показать текст, что ввод неверный
        }

        private void Button_Click_Test1(object sender, System.Windows.RoutedEventArgs e)
        {
            //ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;
        }
    }
}