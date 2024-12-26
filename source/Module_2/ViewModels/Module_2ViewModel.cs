﻿namespace Module_2.ViewModels
{
    public sealed class Module_2ViewModel : ObservableObject
    {
        private string _inputRadius;
        public string InputRadius
        {
            get => _inputRadius;
            set => SetProperty(ref _inputRadius, value);
        }

        private bool _isWindowClosetCorrect;
        public bool IsWindowClosetCorrect
        {
            get => _isWindowClosetCorrect;
            set => SetProperty(ref _isWindowClosetCorrect, value);
        }

    }
}