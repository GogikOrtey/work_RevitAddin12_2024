using Application.Commands;
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
            var panel = Application.CreatePanel("Commands", "RevitAddIn12");

            panel.AddPushButton<StartupCommand>("Тестовая кнопка")
                //.SetLargeImage("/Application;component/Resources/Icons/ico_new_1_32.png");
                //.SetLargeImage("/Application;component/Resources/Icons/ico_new_2_32.png");
                .SetLargeImage("/Application;component/Resources/Icons/ico_test_2.png");

            panel.AddPushButton<StartupCommand_2>("Генерация круга из стен")
                .SetLargeImage("/Application;component/Resources/Icons/ico_new_1_32.png");
        }
    }
}