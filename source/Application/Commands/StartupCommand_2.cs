using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_2.ViewModels;
using Module_2.Views;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Application.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand_2 : ExternalCommand
    {
        public override void Execute()
        {
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = Brushes.Red // Заливаем элемент красным цветом
            };

            // Создаем окно и добавляем в него стековую панель
            var messageBox = new Window
            {
                Title = "Information",
                Content = stackPanel,
                Width = 300,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Создаем текстовый блок
            var textBlock = new TextBlock
            {
                Text = "Hello world",
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            // Создаем кнопку "Ок"
            var okButton = new Button
            {
                Content = "Ok",
                Width = 75,
                Margin = new Thickness(0, 0, 0, 10),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (sender, args) => messageBox.Close();

            // Создаем StackPanel для кнопки и размещаем её внизу
            var buttonPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom
            };
            buttonPanel.Children.Add(okButton);

            // Добавляем текстовый блок и панель с кнопкой в стековую панель
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(buttonPanel);

            // Показываем окно
            messageBox.ShowDialog();
        }
    }
}
