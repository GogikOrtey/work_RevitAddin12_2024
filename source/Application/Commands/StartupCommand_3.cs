using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Module_3.ViewModels;
using Module_3.Views;
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

    // Этот модуль выводит информацию об объекте по нажатию на него мышкой
    public class StartupCommand_3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
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

                string wallTypeNamesString = string.Join("\n\n", wallTypeNames);
                TaskDialog.Show("Wall Types", "Все доступные материалы стен: " + wallTypeNamesString);

                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

            //// Вывод id выбранного элемента
            //Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            //Element element = doc.GetElement(myRef);
            //ElementId id = element.Id;

            //try
            //{
            //    // Выбор элемента (стены)
            //    Reference pickedObj = uiDoc.Selection.PickObject(ObjectType.Element, "Выберите стену");
            //    Element elem = doc.GetElement(pickedObj);

            //    // Проверка, что выбранный элемент является стеной
            //    if (elem is Wall wall)
            //    {
            //        // Получение Id выбранного элемента
            //        TaskDialog.Show("Element Id", "Id элемента: " + wall.Id);

            //        // Получение всех типов стен в документе
            //        FilteredElementCollector collector = new FilteredElementCollector(doc);
            //        ICollection<Element> wallTypes = collector.OfClass(typeof(WallType)).ToElements();

            //        List<string> wallTypeNames = new List<string>();

            //        foreach (Element wallTypeElem in wallTypes)
            //        {
            //            WallType wallType = wallTypeElem as WallType;
            //            if (wallType != null)
            //            {
            //                wallTypeNames.Add(wallType.Name);
            //            }
            //        }

            //        string wallTypeNamesString = string.Join(", ", wallTypeNames);
            //        TaskDialog.Show("Wall Types", "Доступные материалы стен: " + wallTypeNamesString);
            //    }
            //    else
            //    {
            //        TaskDialog.Show("Error", "Выбранный элемент не является стеной.");
            //    }

            //    return Result.Succeeded;
            //}
            //catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            //{
            //    // Обработка отмены выбора пользователем
            //    return Result.Cancelled;
            //}
            //catch (System.Exception ex)
            //{
            //    message = ex.Message;
            //    return Result.Failed;
            //}


            //OthersMyVoid.ShowInfoWindow("id элемента = " + id);

            return Result.Succeeded;
        }

        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        //{
        //    UIDocument uidoc = commandData.Application.ActiveUIDocument;
        //    Document doc = uidoc.Document;

        //    // Вывод id выбранного элемента
        //    Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
        //    Element element = doc.GetElement(myRef);
        //    ElementId id = element.Id;

        //    OthersMyVoid.ShowInfoWindow("id элемента = " + id);

        //    return Result.Succeeded;
        //}
    }
}
