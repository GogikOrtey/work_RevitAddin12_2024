﻿using System.ComponentModel;
using System.Xml.Linq;
//using Microsoft.VisualStudio.PlatformUI;

namespace Module_2.ViewModels
{
    public sealed partial class Module_2ViewModel : ObservableObject
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

        // Значение радиуса, которое вводит пользователь

        private string _selectedWallMaterial;
        public string SelectedWallMaterial
        {
            get => _selectedWallMaterial;
            set => SetProperty(ref _selectedWallMaterial, value);
        }

        //
        // Список для хранения всех доступных материалов стен:
        //

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

        // Создаю новый список, при инициализации
        public Module_2ViewModel(List<string> inputString)
        {
            //WallTypeNames = new List<string>();
            WallTypeNames = inputString;
        }

        // Отдельный метод для добавления значений в список
        public void AddWallTypeName(string name)
        {
            WallTypeNames.Add(name);
            OnPropertyChanged("WallTypeNames");
        }

        // Событие, которое я вызываю, при изменени значения переменной
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}