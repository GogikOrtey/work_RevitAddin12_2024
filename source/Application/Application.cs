using System.Reflection;
using System.Windows.Media.Imaging;
using Application.Commands;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace Application
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Test Task 1", "Altek Orlov");

            panel.AddPushButton<StartupCommand>("Тестовая кнопка")
                .SetLargeImage("/Application;component/Resources/Icons/ico_test_2.png");

            panel.AddPushButton<StartupCommand_2>("Отображение информации о выбранном объекте")
                .SetLargeImage("/Application;component/Resources/Icons/ico_new_1_32.png");

            panel.AddPushButton<StartupCommand_3>("Генерация круга из стен")
                .SetLargeImage("/Application;component/Resources/Icons/ico_new_2_32.png");

        }
    }
}