﻿using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.DependencyInjection;
using Module_2.ViewModels;

namespace Module_2.Views
{
    public sealed partial class Module_2View
    {
        // Выношу переменную, для более удобного доступа к ViewModel
        Module_2ViewModel viewModel_pub;

        // Главный метод инициализации окна:
        public Module_2View(Module_2ViewModel viewModel)
        {
            viewModel_pub = viewModel;

            DataContext = viewModel;
            InitializeComponent();
            TB_inputRadius.Text = "5";                  // Устанавливаю начальное значение для поля ввода радиуса
            AddElementForComboBoxSelectWallMaterial();  // Добавляю значения в список выбора материала для стен
            SetValueFor_IsWindowCorrectCloset(false);   // По умолчанию устанавливаю false, и устанавливаю true только при корректном закрытии
        }        

        // Здесь добавляются значения в список выбора материала для стен
        void AddElementForComboBoxSelectWallMaterial()
        {
            // Проверяю, не пуст ли список на получение всех типов стен:

            // Если у нас в проекте отсутствуют доступные стены, или код получения доступных стен отработал некорректно
            // то показываем ошибку, и вызываем исключение
            if (viewModel_pub.WallTypeNames.Count == 0)
            {
                MessageBox.Show("К сожалению, возникла ошибка получения данных о доступных стенах.\n\nПродолжение работы плагина невозможно", 
                    "Ошибка получения данных о доступных стенах", MessageBoxButton.OK, MessageBoxImage.Error);

                throw new ApplicationException("Произошла программная ошибка получения данных о доступных стенах. Операция прервана");
            }

            //ComboBoxForSelectWallMaterial.Items.Clear(); // Очищает весь список

            // Перебираю все элементы из списка общего WallTypeNames, и добавляю их в поле с выбором на форме
            foreach (var item in viewModel_pub.WallTypeNames)
            {                
                ComboBoxForSelectWallMaterial.Items.Add(item);
            }

            // Выбираю самый первый вариант в выпадающем списке:
            ComboBoxForSelectWallMaterial.SelectedIndex = 0;
        }

        // Это метод установки значения для ViewModel, которое можно дальше будет использовать в Model
        private void SetValueFor_IsWindowCorrectCloset(bool val)
        {
            // Корректно ли закрылось окно по созданию стены?
            viewModel_pub.IsWindowClosetCorrect = val;

            // Дальше, в Model, стена создаётся, только если окно было закрыто корректно (а не через крестик, и не через кнопку "Отмена")
        }

        // Красная кнопка снизу "Отменить"
        private void Button_Click_CloseThisWindow(object sender, System.Windows.RoutedEventArgs e)
        {
            // Закрытие окна
            this.Close();
        }

        // Это метод, который вызывается при каждом изменении значения поля ввода радиуса пользователем
        private void TB_inputRadius_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIsValidValueFronInput();
        }

        // Проверяет корректность значения в поле ввода радиуса
        // И если значение некорректно, то показывает красный текст, а если верное - то скрывает его
        private bool CheckIsValidValueFronInput()
        {
            string inputRadius = TB_inputRadius.Text;
            double radius;
            bool isNumber = double.TryParse(inputRadius, out radius);

            if (inputRadius == "")
            {
                // Если значение пусто, то не вывожу текст с ошибкой
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;
                return true; 
            }

            if (!isNumber)
            {
                // Значение не является числом, установите переменную в false
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
            {
                // Если значение является корректным числом
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;
                return true;
            }
        }

        // Кнопка подтверждения генерации стен
        // Зелёная кнопка снизу "Создать стену"
        private void Button_Click_AcceptGenerateWall(object sender, System.Windows.RoutedEventArgs e)
        {
            //
            //  Проверка корректности поля радиуса:
            //

            string inputRadius = TB_inputRadius.Text;
            double radius;
            bool isNumber = double.TryParse(inputRadius, out radius);

            if (!isNumber)
            {
                // Если в поле всё таки введено не число, окно не закроется корректно, после нажатия пользователем кнопки "Создать стену"
                MessageBox.Show("Введите числовое значение для радиуса.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Visible;
                // Покажется окно с предупреждением, и главное окно будет ждать корректного ввода от пользователя
            }
            else
            {
                // Если значение является числом

                ErrorTextBlock_forIncorrectIngectRadius.Visibility = System.Windows.Visibility.Collapsed;

                //TaskDialog.Show("Info", "Значение корректно, и = " + radius);

                // Проверяю, если радиус слишком маленький или слишком большой
                if (radius < 1)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть больше 1 метра", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (radius > 20)
                {
                    // Радиус некорректнен
                    MessageBox.Show("Радиус должен быть меньше 20 метров", "Ошибка ввода: Радиус некорректнен!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Радиус корректен
                    //TaskDialog.Show("Info", "Радиус корректен: " + radius);

                    SetValueFor_IsWindowCorrectCloset(true);
                    this.Close();   // Закрываем это окно только тогда, когда данные, введённые пользователем корректны
                }
            }            
        }

        // Процедура для тестовой отладочной кнопки
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel_pub.WallTypeNames.Add("8 + Новый элемент");

            foreach (var item in viewModel_pub.WallTypeNames)
            {
                //ComboBoxForSelectWallMaterial.Items.Clear();
                ComboBoxForSelectWallMaterial.Items.Add(item);
            }
        }

        // Действие, которое вызывается, когда пользователь выбрал какой-либо новый элемент в выпадающем списке - выбора материала стены
        private void ComboBoxForSelectWallMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Передаю в переменную SelectedWallMaterial во ViewModel выбранный пользователем материал стены
            viewModel_pub.SelectedWallMaterial = ComboBoxForSelectWallMaterial.SelectedItem.ToString();
        }
    }
}









