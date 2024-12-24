﻿using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_2.ViewModels;
using Module_2.Views;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Autodesk.Revit.DB;

namespace Application.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            //Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            //Element element = doc.GetElement(myRef);
            //ElementId id = element.Id;

            //ShowInfoWindow("id объекта = " + id.ToString());

            // Выбор точки на поверхности (например, на полу)
            XYZ point = uidoc.Selection.PickPoint("Выберите точку на поверхности");

            // Вывод координат выбранной точки
            ShowInfoWindow($"Координаты точки: X = {point.X}, Y = {point.Y}, Z = {point.Z}");

            return Result.Succeeded;
        }

        //// Главный метод, который вызывается по нажатию на кнопку 
        //public override void Execute(UIApplication uiApp)
        //{


        //    //SelectedElementAndGetInfo();
        //    ShowInfoWindow("Hello world!");
        //}

        // Метод, в котором прописано выделение объекта, получение данных о них, и вывод их
        void SelectedElementAndGetInfo()
        {

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