using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Application
{
    internal class OthersMyVoid
    {
        // Выводит окно с информацией
        // В окне отображается входной текст, из переменной inpText
        public static void ShowInfoWindow(string inpText)
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
                Text = inpText,                                     // Устанавливаю текст 

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
