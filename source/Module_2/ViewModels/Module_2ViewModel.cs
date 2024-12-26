using System.ComponentModel;

namespace Module_2.ViewModels
{
    public sealed class Module_2ViewModel : ObservableObject
    {
        // Значение радиуса, которое вводит пользователь

        private string _inputRadius;
        public string InputRadius
        {
            get => _inputRadius;
            set => SetProperty(ref _inputRadius, value);
        }

        // Переменная bool, которую я просмотриваю в Model

        private bool _isWindowClosetCorrect;
        public bool IsWindowClosetCorrect
        {
            get => _isWindowClosetCorrect;
            set => SetProperty(ref _isWindowClosetCorrect, value);
        }

        // Список, который можно изменять из Model:

        private List<string> _wallTypeNames;
        public List<string> WallTypeNames
        {
            get { return _wallTypeNames; }
            set
            {
                _wallTypeNames = value;
                OnPropertyChanged("WallTypeNames");
            }
        }

        public Module_2ViewModel()
        {
            WallTypeNames = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}