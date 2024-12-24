using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_2.ViewModels;
using Module_2.Views;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;

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
            var stackPanel = new StackPanel();

            // Создаем окно и добавляем в него стековую панель
            var messageBox = new Window
            {
                Title = "Information",
                Content = stackPanel,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Создаем текстовый блок
            var textBlock = new TextBlock
            {
                Text = "Hello world",
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            // Создаем кнопку "Ок"
            var okButton = new Button
            {
                Content = "Ok",
                Width = 75,
                Margin = new Thickness(10)
            };
            okButton.Click += (sender, args) => messageBox.Close();

            // Создаем стековую панель и добавляем в нее текстовый блок и кнопку            
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(okButton);

            // Показываем окно
            messageBox.ShowDialog();
        }
    }
}
